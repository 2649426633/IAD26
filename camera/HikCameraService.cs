using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace _180Detection.Camera
{
    public sealed class HikCameraDevice
    {
        internal object NativeInfo { get; set; }
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// 海康机器人 MVS 相机连接层。
    /// 通过运行时反射加载 MVS .NET SDK，避免没有安装 SDK 的电脑直接编译失败。
    /// 当前负责：检测 SDK、枚举相机、连接、断开。
    /// 后续实时取流/软触发继续放在此层扩展。
    /// </summary>
    public sealed class HikCameraService : IDisposable
    {
        private Assembly _sdkAssembly;
        private bool _loadAttempted;
        private string _sdkLoadError;
        private object _connectedDevice;
        private string _connectedName;
        private readonly List<HikCameraDevice> _devices = new List<HikCameraDevice>();

        public bool IsSdkAvailable
        {
            get { return EnsureSdkLoaded(); }
        }

        public bool IsConnected
        {
            get { return _connectedDevice != null; }
        }

        public string ConnectedCameraName
        {
            get { return _connectedName ?? string.Empty; }
        }

        public IList<HikCameraDevice> RefreshDevices()
        {
            _devices.Clear();

            if (!EnsureSdkLoaded())
                return _devices.AsReadOnly();

            Type enumeratorType = FindTypeByName("DeviceEnumerator");
            if (enumeratorType == null)
            {
                _sdkLoadError = "已找到 MVS SDK，但未找到 DeviceEnumerator 接口";
                return _devices.AsReadOnly();
            }

            MethodInfo enumMethod = FindEnumDevicesMethod(enumeratorType);
            if (enumMethod == null)
            {
                _sdkLoadError = "已找到 MVS SDK，但未找到 EnumDevices 接口";
                return _devices.AsReadOnly();
            }

            object result;
            try
            {
                ParameterInfo[] parameters = enumMethod.GetParameters();
                if (parameters.Length == 0)
                {
                    result = enumMethod.Invoke(null, null);
                }
                else
                {
                    object filter = BuildDeviceLayerFilter(parameters[0].ParameterType);
                    result = enumMethod.Invoke(null, new object[] { filter });
                }
            }
            catch (TargetInvocationException ex)
            {
                Exception inner = ex.InnerException ?? ex;
                _sdkLoadError = "枚举海康相机失败：" + inner.Message;
                return _devices.AsReadOnly();
            }
            catch (Exception ex)
            {
                _sdkLoadError = "枚举海康相机失败：" + ex.Message;
                return _devices.AsReadOnly();
            }

            IEnumerable enumerable = result as IEnumerable;
            if (enumerable == null)
            {
                _sdkLoadError = "MVS 枚举接口返回了无法识别的数据";
                return _devices.AsReadOnly();
            }

            int index = 1;
            foreach (object info in enumerable)
            {
                if (info == null)
                    continue;

                _devices.Add(new HikCameraDevice
                {
                    NativeInfo = info,
                    DisplayName = BuildDisplayName(info, index)
                });
                index++;
            }

            _sdkLoadError = null;
            return _devices.AsReadOnly();
        }

        public void Connect(int deviceIndex)
        {
            if (!EnsureSdkLoaded())
                throw new InvalidOperationException(GetStatusText());

            if (IsConnected)
                return;

            if (deviceIndex < 0 || deviceIndex >= _devices.Count)
                throw new ArgumentOutOfRangeException("deviceIndex", "请选择一个可用的海康相机。 ");

            HikCameraDevice selected = _devices[deviceIndex];
            Type factoryType = FindTypeByName("DeviceFactory");
            if (factoryType == null)
                throw new InvalidOperationException("MVS SDK 中未找到 DeviceFactory 接口。请确认安装的是当前版 MVS .NET SDK。");

            MethodInfo createMethod = FindCreateDeviceMethod(factoryType, selected.NativeInfo);
            if (createMethod == null)
                throw new InvalidOperationException("MVS SDK 中未找到兼容的 CreateDevice 接口。");

            object device;
            try
            {
                device = createMethod.Invoke(null, new object[] { selected.NativeInfo });
                if (device == null)
                    throw new InvalidOperationException("MVS 创建相机对象失败。 ");

                InvokeRequiredNoArgMethod(device, "Open");
            }
            catch (TargetInvocationException ex)
            {
                Exception inner = ex.InnerException ?? ex;
                throw new InvalidOperationException("连接海康相机失败：" + inner.Message, inner);
            }

            _connectedDevice = device;
            _connectedName = selected.DisplayName;
        }

        public void Disconnect()
        {
            object device = _connectedDevice;
            _connectedDevice = null;
            _connectedName = null;

            if (device == null)
                return;

            try
            {
                MethodInfo close = device.GetType().GetMethod(
                    "Close",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    Type.EmptyTypes,
                    null);
                if (close != null)
                    close.Invoke(device, null);
            }
            catch
            {
                // 关闭阶段不阻断程序退出。
            }

            IDisposable disposable = device as IDisposable;
            if (disposable != null)
            {
                try { disposable.Dispose(); }
                catch { }
            }
        }

        public string GetStatusText()
        {
            if (IsConnected)
                return "已连接：" + ConnectedCameraName;

            if (!EnsureSdkLoaded())
                return string.IsNullOrWhiteSpace(_sdkLoadError)
                    ? "未找到海康 MVS .NET SDK"
                    : _sdkLoadError;

            if (_devices.Count == 0)
                return "MVS 已就绪，未发现相机";

            return "发现 " + _devices.Count + " 台相机";
        }

        public void Dispose()
        {
            Disconnect();
        }

        private bool EnsureSdkLoaded()
        {
            if (_sdkAssembly != null)
                return true;
            if (_loadAttempted)
                return false;

            _loadAttempted = true;

            string configured = ConfigurationManager.AppSettings["HikCameraSdkAssembly"];
            if (!string.IsNullOrWhiteSpace(configured))
            {
                string path = Environment.ExpandEnvironmentVariables(configured.Trim());
                if (TryLoadAssemblyFrom(path))
                    return true;
            }

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] localCandidates =
            {
                Path.Combine(baseDirectory, "MvCameraControl.Net.dll"),
                Path.Combine(baseDirectory, "MvCameraControl.dll")
            };
            foreach (string candidate in localCandidates)
            {
                if (TryLoadAssemblyFrom(candidate))
                    return true;
            }

            string[] assemblyNames = { "MvCameraControl.Net", "MvCameraControl" };
            foreach (string name in assemblyNames)
            {
                try
                {
                    _sdkAssembly = Assembly.Load(name);
                    if (_sdkAssembly != null)
                        return true;
                }
                catch { }
            }

            string[] roots =
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "MVS"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "MVS")
            };

            foreach (string root in roots)
            {
                if (TryFindAndLoadFromMvsDirectory(root))
                    return true;
            }

            _sdkLoadError = "未找到海康 MVS .NET SDK。请安装 MVS，或在 App.config 配置 HikCameraSdkAssembly。";
            return false;
        }

        private bool TryFindAndLoadFromMvsDirectory(string root)
        {
            if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
                return false;

            try
            {
                string[] files = Directory.GetFiles(root, "MvCameraControl*.dll", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (TryLoadAssemblyFrom(file))
                        return true;
                }
            }
            catch { }

            return false;
        }

        private bool TryLoadAssemblyFrom(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return false;

            try
            {
                Assembly assembly = Assembly.LoadFrom(path);
                if (FindTypeByName(assembly, "DeviceEnumerator") == null)
                    return false;

                _sdkAssembly = assembly;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Type FindTypeByName(string simpleName)
        {
            return FindTypeByName(_sdkAssembly, simpleName);
        }

        private static Type FindTypeByName(Assembly assembly, string simpleName)
        {
            if (assembly == null)
                return null;

            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (string.Equals(type.Name, simpleName, StringComparison.Ordinal))
                        return type;
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Type[] types = ex.Types;
                if (types != null)
                {
                    foreach (Type type in types)
                    {
                        if (type != null && string.Equals(type.Name, simpleName, StringComparison.Ordinal))
                            return type;
                    }
                }
            }

            return null;
        }

        private static MethodInfo FindEnumDevicesMethod(Type enumeratorType)
        {
            MethodInfo fallback = null;
            foreach (MethodInfo method in enumeratorType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (!string.Equals(method.Name, "EnumDevices", StringComparison.Ordinal))
                    continue;

                int count = method.GetParameters().Length;
                if (count == 0)
                    return method;
                if (count == 1)
                    fallback = method;
            }
            return fallback;
        }

        private static object BuildDeviceLayerFilter(Type parameterType)
        {
            if (!parameterType.IsEnum)
                return Activator.CreateInstance(parameterType);

            ulong mask = 0;
            Array values = Enum.GetValues(parameterType);
            foreach (object value in values)
            {
                string name = Enum.GetName(parameterType, value) ?? string.Empty;
                if (name.IndexOf("Gig", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("Usb", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("GenTL", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    mask |= Convert.ToUInt64(value);
                }
            }

            if (mask == 0 && values.Length > 0)
            {
                foreach (object value in values)
                {
                    ulong numeric = Convert.ToUInt64(value);
                    if (numeric != 0)
                    {
                        mask |= numeric;
                        break;
                    }
                }
            }

            return Enum.ToObject(parameterType, mask);
        }

        private static MethodInfo FindCreateDeviceMethod(Type factoryType, object deviceInfo)
        {
            foreach (MethodInfo method in factoryType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (!string.Equals(method.Name, "CreateDevice", StringComparison.Ordinal))
                    continue;

                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType.IsInstanceOfType(deviceInfo))
                    return method;
            }
            return null;
        }

        private static void InvokeRequiredNoArgMethod(object target, string methodName)
        {
            MethodInfo method = target.GetType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);
            if (method == null)
                throw new MissingMethodException(target.GetType().FullName, methodName);
            method.Invoke(target, null);
        }

        private static string BuildDisplayName(object info, int index)
        {
            string userName = ReadStringProperty(info, "UserDefinedName", "UserName");
            string model = ReadStringProperty(info, "ModelName", "Model");
            string serial = ReadStringProperty(info, "SerialNumber", "SerialNo", "DeviceKey");

            string text = string.Empty;
            if (!string.IsNullOrWhiteSpace(userName))
                text = userName.Trim();
            else if (!string.IsNullOrWhiteSpace(model))
                text = model.Trim();
            else
                text = "海康相机 " + index;

            if (!string.IsNullOrWhiteSpace(serial) && text.IndexOf(serial, StringComparison.OrdinalIgnoreCase) < 0)
                text += "  [" + serial.Trim() + "]";

            return text;
        }

        private static string ReadStringProperty(object target, params string[] names)
        {
            if (target == null)
                return string.Empty;

            Type type = target.GetType();
            foreach (string name in names)
            {
                PropertyInfo property = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (property == null || !property.CanRead)
                    continue;

                try
                {
                    object value = property.GetValue(target, null);
                    if (value != null)
                        return Convert.ToString(value);
                }
                catch { }
            }
            return string.Empty;
        }
    }
}

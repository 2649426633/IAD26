# IAD26

工业异常检测 WinForms 客户端。

当前推理架构：`.NET 8 WinForms -> IndustrialAnomaly.Runtime -> ONNX Runtime`，正式检测不启动 Python 或 Console 进程。

本地目录要求：

```text
180Decetion/
├─ runtime/
│  └─ IndustrialAnomaly.Runtime/
│     └─ IndustrialAnomaly.Runtime.csproj
├─ engine/
│  ├─ patchcore_feature.onnx
│  ├─ dinov2_feature.onnx
│  └─ engine_config.json
├─ products/
│  └─ phone/
│     ├─ patchcore_memory.bin
│     ├─ defect_cls.bin
│     ├─ defect_center.bin
│     └─ product_model.json
└─ config/
   └─ appsettings.json
```

`IndustrialAnomaly.Console` 仅用于算法调试，不是 WinForms 正式推理依赖。

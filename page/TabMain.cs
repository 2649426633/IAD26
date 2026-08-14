using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace _180Detection
{
    public partial class TabMain : UserControl
    {
        private const int MaxLogCount = 1000;

        public TabMain()
        {
            InitializeComponent();
            ResizeLogColumns();
        }

        private void SaveOption_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton option = sender as RadioButton;
            if (option == null || !option.Checked)
                return;

            if (option == radioNoSave)
                LogInfo("保存", "已选择：不保存");
            else if (option == radioSaveNg)
                LogInfo("保存", "已选择：保存NG");
            else if (option == radioSaveAll)
                LogInfo("保存", "已选择：保存全部");
        }

        public void LogInfo(string type, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, string>(LogInfo), type, message);
                return;
            }

            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add(type ?? string.Empty);
            item.SubItems.Add(message ?? string.Empty);
            listViewInfo.Items.Insert(0, item);

            while (listViewInfo.Items.Count > MaxLogCount)
                listViewInfo.Items.RemoveAt(listViewInfo.Items.Count - 1);
        }

        public void UpdateCapacity(int total, int ok, int ng)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int, int, int>(UpdateCapacity), total, ok, ng);
                return;
            }

            total = Math.Max(0, total);
            ok = Math.Max(0, ok);
            ng = Math.Max(0, ng);

            label5.Text = total.ToString("N0");
            label7.Text = ok.ToString("N0");
            label6.Text = ng.ToString("N0");
            label8.Text = total == 0 ? "0.00%" : ((double)ok / total).ToString("P2");
        }

        public void UpdateTestResult(double frontAngle, double rearAngle, bool isOk)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<double, double, bool>(UpdateTestResult),
                    frontAngle, rearAngle, isOk);
                return;
            }

            label11.Text = frontAngle.ToString("0.00") + "°";
            label13.Text = rearAngle.ToString("0.00") + "°";
            label15.Text = isOk ? "OK" : "NG";
            label15.ForeColor = isOk
                ? Color.FromArgb(31, 157, 85)
                : Color.FromArgb(220, 68, 68);
        }

        private void BtnOpenImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择图片";
                openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有文件|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    Image loadedImage;
                    using (Image sourceImage = Image.FromFile(openFileDialog.FileName))
                    {
                        loadedImage = new Bitmap(sourceImage);
                    }

                    Image previousImage = pictureBox1.Image;
                    pictureBox1.Image = loadedImage;
                    if (previousImage != null)
                        previousImage.Dispose();

                    LogInfo("图片", "已加载: " + Path.GetFileName(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法加载图片: " + ex.Message, "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClearCapacity_Click(object sender, EventArgs e)
        {
            UpdateCapacity(0, 0, 0);
            LogInfo("产能", "统计数据已清除");
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            listViewInfo.Items.Clear();
        }

        private void ListViewInfo_Resize(object sender, EventArgs e)
        {
            ResizeLogColumns();
        }

        private void ResizeLogColumns()
        {
            if (listViewInfo.Columns.Count < 3)
                return;

            columnHeader1.Width = 82;
            columnHeader2.Width = 82;
            int messageWidth = listViewInfo.ClientSize.Width
                - columnHeader1.Width
                - columnHeader2.Width
                - SystemInformation.VerticalScrollBarWidth
                - 8;
            columnHeader3.Width = Math.Max(120, messageWidth);
        }
    }
}

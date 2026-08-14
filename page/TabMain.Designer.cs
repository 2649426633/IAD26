using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    partial class TabMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.rootLayout = new TableLayoutPanel();
            this.leftLayout = new TableLayoutPanel();
            this.groupCapacity = new GroupBox();
            this.capacityLayout = new TableLayoutPanel();
            this.label1 = new Label();
            this.label4 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label5 = new Label();
            this.label7 = new Label();
            this.label6 = new Label();
            this.label8 = new Label();
            this.button1 = new Button();
            this.groupSave = new GroupBox();
            this.saveLayout = new TableLayoutPanel();
            this.radioNoSave = new RadioButton();
            this.radioSaveNg = new RadioButton();
            this.radioSaveAll = new RadioButton();
            this.groupResult = new GroupBox();
            this.resultLayout = new TableLayoutPanel();
            this.label12 = new Label();
            this.label11 = new Label();
            this.label14 = new Label();
            this.label13 = new Label();
            this.label16 = new Label();
            this.label15 = new Label();
            this.groupInfo = new GroupBox();
            this.infoLayout = new TableLayoutPanel();
            this.infoToolbar = new TableLayoutPanel();
            this.btnOpenImage = new Button();
            this.btnClearLog = new Button();
            this.listViewInfo = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.imagesLayout = new TableLayoutPanel();
            this.groupImage1 = new GroupBox();
            this.groupImage2 = new GroupBox();
            this.groupImage3 = new GroupBox();
            this.groupImage4 = new GroupBox();
            this.pictureBox1 = new PictureBox();
            this.pictureBox2 = new PictureBox();
            this.pictureBox3 = new PictureBox();
            this.pictureBox4 = new PictureBox();

            this.rootLayout.SuspendLayout();
            this.leftLayout.SuspendLayout();
            this.groupCapacity.SuspendLayout();
            this.capacityLayout.SuspendLayout();
            this.groupSave.SuspendLayout();
            this.saveLayout.SuspendLayout();
            this.groupResult.SuspendLayout();
            this.resultLayout.SuspendLayout();
            this.groupInfo.SuspendLayout();
            this.infoLayout.SuspendLayout();
            this.infoToolbar.SuspendLayout();
            this.imagesLayout.SuspendLayout();
            this.groupImage1.SuspendLayout();
            this.groupImage2.SuspendLayout();
            this.groupImage3.SuspendLayout();
            this.groupImage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();

            // rootLayout
            this.rootLayout.BackColor = SystemColors.Control;
            this.rootLayout.ColumnCount = 2;
            this.rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            this.rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
            this.rootLayout.Controls.Add(this.leftLayout, 0, 0);
            this.rootLayout.Controls.Add(this.imagesLayout, 1, 0);
            this.rootLayout.Dock = DockStyle.Fill;
            this.rootLayout.Location = new Point(0, 0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.Padding = new Padding(12);
            this.rootLayout.RowCount = 1;
            this.rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.rootLayout.Size = new Size(2363, 1271);
            this.rootLayout.TabIndex = 0;

            // leftLayout
            this.leftLayout.ColumnCount = 1;
            this.leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.leftLayout.Controls.Add(this.groupCapacity, 0, 0);
            this.leftLayout.Controls.Add(this.groupSave, 0, 1);
            this.leftLayout.Controls.Add(this.groupResult, 0, 2);
            this.leftLayout.Controls.Add(this.groupInfo, 0, 3);
            this.leftLayout.Dock = DockStyle.Fill;
            this.leftLayout.Margin = new Padding(0, 0, 10, 0);
            this.leftLayout.Name = "leftLayout";
            this.leftLayout.RowCount = 4;
            this.leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 24F));
            this.leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
            this.leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 24F));
            this.leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            this.leftLayout.TabIndex = 0;

            // groupCapacity
            this.groupCapacity.Controls.Add(this.capacityLayout);
            this.groupCapacity.Dock = DockStyle.Fill;
            this.groupCapacity.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupCapacity.Margin = new Padding(0, 0, 0, 8);
            this.groupCapacity.Name = "groupCapacity";
            this.groupCapacity.Padding = new Padding(10);
            this.groupCapacity.TabIndex = 0;
            this.groupCapacity.TabStop = false;
            this.groupCapacity.Text = "产能统计";

            // capacityLayout
            this.capacityLayout.ColumnCount = 4;
            this.capacityLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.capacityLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.capacityLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.capacityLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.capacityLayout.Controls.Add(this.label1, 0, 0);
            this.capacityLayout.Controls.Add(this.label4, 1, 0);
            this.capacityLayout.Controls.Add(this.label2, 2, 0);
            this.capacityLayout.Controls.Add(this.label3, 3, 0);
            this.capacityLayout.Controls.Add(this.label5, 0, 1);
            this.capacityLayout.Controls.Add(this.label7, 1, 1);
            this.capacityLayout.Controls.Add(this.label6, 2, 1);
            this.capacityLayout.Controls.Add(this.label8, 3, 1);
            this.capacityLayout.Controls.Add(this.button1, 0, 2);
            this.capacityLayout.Dock = DockStyle.Fill;
            this.capacityLayout.Name = "capacityLayout";
            this.capacityLayout.RowCount = 3;
            this.capacityLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.capacityLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            this.capacityLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            this.capacityLayout.SetColumnSpan(this.button1, 4);
            this.capacityLayout.TabIndex = 0;

            // capacity titles
            this.label1.Dock = DockStyle.Fill;
            this.label1.Name = "label1";
            this.label1.Text = "总数";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label4.Dock = DockStyle.Fill;
            this.label4.Name = "label4";
            this.label4.Text = "OK";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Dock = DockStyle.Fill;
            this.label2.Name = "label2";
            this.label2.Text = "NG";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label3.Dock = DockStyle.Fill;
            this.label3.Name = "label3";
            this.label3.Text = "良率";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;

            // capacity values
            this.label5.BackColor = SystemColors.ButtonHighlight;
            this.label5.BorderStyle = BorderStyle.FixedSingle;
            this.label5.Dock = DockStyle.Fill;
            this.label5.Font = new Font("宋体", 13.875F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.label5.Margin = new Padding(3);
            this.label5.Name = "label5";
            this.label5.Text = "0";
            this.label5.TextAlign = ContentAlignment.MiddleCenter;
            this.label7.BackColor = SystemColors.ButtonHighlight;
            this.label7.BorderStyle = BorderStyle.FixedSingle;
            this.label7.Dock = DockStyle.Fill;
            this.label7.Font = new Font("宋体", 13.875F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.label7.Margin = new Padding(3);
            this.label7.Name = "label7";
            this.label7.Text = "0";
            this.label7.TextAlign = ContentAlignment.MiddleCenter;
            this.label6.BackColor = SystemColors.ButtonHighlight;
            this.label6.BorderStyle = BorderStyle.FixedSingle;
            this.label6.Dock = DockStyle.Fill;
            this.label6.Font = new Font("宋体", 13.875F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.label6.Margin = new Padding(3);
            this.label6.Name = "label6";
            this.label6.Text = "0";
            this.label6.TextAlign = ContentAlignment.MiddleCenter;
            this.label8.BackColor = SystemColors.ButtonHighlight;
            this.label8.BorderStyle = BorderStyle.FixedSingle;
            this.label8.Dock = DockStyle.Fill;
            this.label8.Font = new Font("宋体", 13.875F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.label8.Margin = new Padding(3);
            this.label8.Name = "label8";
            this.label8.Text = "0.00%";
            this.label8.TextAlign = ContentAlignment.MiddleCenter;

            this.button1.Dock = DockStyle.Fill;
            this.button1.Font = new Font("宋体", 11.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.button1.Margin = new Padding(3, 5, 3, 3);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "清除产能";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnClearCapacity_Click);

            // groupSave
            this.groupSave.Controls.Add(this.saveLayout);
            this.groupSave.Dock = DockStyle.Fill;
            this.groupSave.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupSave.Margin = new Padding(0, 0, 0, 8);
            this.groupSave.Name = "groupSave";
            this.groupSave.Padding = new Padding(10);
            this.groupSave.TabIndex = 1;
            this.groupSave.TabStop = false;
            this.groupSave.Text = "图像保存";

            this.saveLayout.ColumnCount = 3;
            this.saveLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            this.saveLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            this.saveLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            this.saveLayout.Controls.Add(this.radioNoSave, 0, 0);
            this.saveLayout.Controls.Add(this.radioSaveNg, 1, 0);
            this.saveLayout.Controls.Add(this.radioSaveAll, 2, 0);
            this.saveLayout.Dock = DockStyle.Fill;
            this.saveLayout.Name = "saveLayout";
            this.saveLayout.RowCount = 1;
            this.saveLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.saveLayout.TabIndex = 0;

            this.radioNoSave.Checked = true;
            this.radioNoSave.Dock = DockStyle.Fill;
            this.radioNoSave.Name = "radioNoSave";
            this.radioNoSave.TabIndex = 0;
            this.radioNoSave.TabStop = true;
            this.radioNoSave.Text = "不保存";
            this.radioNoSave.TextAlign = ContentAlignment.MiddleCenter;
            this.radioNoSave.UseVisualStyleBackColor = true;
            this.radioNoSave.CheckedChanged += new System.EventHandler(this.SaveOption_CheckedChanged);
            this.radioSaveNg.Dock = DockStyle.Fill;
            this.radioSaveNg.Name = "radioSaveNg";
            this.radioSaveNg.TabIndex = 1;
            this.radioSaveNg.Text = "保存NG";
            this.radioSaveNg.TextAlign = ContentAlignment.MiddleCenter;
            this.radioSaveNg.UseVisualStyleBackColor = true;
            this.radioSaveNg.CheckedChanged += new System.EventHandler(this.SaveOption_CheckedChanged);
            this.radioSaveAll.Dock = DockStyle.Fill;
            this.radioSaveAll.Name = "radioSaveAll";
            this.radioSaveAll.TabIndex = 2;
            this.radioSaveAll.Text = "保存全部";
            this.radioSaveAll.TextAlign = ContentAlignment.MiddleCenter;
            this.radioSaveAll.UseVisualStyleBackColor = true;
            this.radioSaveAll.CheckedChanged += new System.EventHandler(this.SaveOption_CheckedChanged);

            // groupResult
            this.groupResult.Controls.Add(this.resultLayout);
            this.groupResult.Dock = DockStyle.Fill;
            this.groupResult.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupResult.Margin = new Padding(0, 0, 0, 8);
            this.groupResult.Name = "groupResult";
            this.groupResult.Padding = new Padding(10);
            this.groupResult.TabIndex = 2;
            this.groupResult.TabStop = false;
            this.groupResult.Text = "检测结果";

            this.resultLayout.ColumnCount = 2;
            this.resultLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            this.resultLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            this.resultLayout.Controls.Add(this.label12, 0, 0);
            this.resultLayout.Controls.Add(this.label11, 1, 0);
            this.resultLayout.Controls.Add(this.label14, 0, 1);
            this.resultLayout.Controls.Add(this.label13, 1, 1);
            this.resultLayout.Controls.Add(this.label16, 0, 2);
            this.resultLayout.Controls.Add(this.label15, 1, 2);
            this.resultLayout.Dock = DockStyle.Fill;
            this.resultLayout.Name = "resultLayout";
            this.resultLayout.RowCount = 3;
            this.resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
            this.resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
            this.resultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.334F));
            this.resultLayout.TabIndex = 0;

            this.label12.Dock = DockStyle.Fill;
            this.label12.Name = "label12";
            this.label12.Text = "前相机展平角";
            this.label12.TextAlign = ContentAlignment.MiddleCenter;
            this.label14.Dock = DockStyle.Fill;
            this.label14.Name = "label14";
            this.label14.Text = "后相机展平角";
            this.label14.TextAlign = ContentAlignment.MiddleCenter;
            this.label16.Dock = DockStyle.Fill;
            this.label16.Name = "label16";
            this.label16.Text = "最终判定";
            this.label16.TextAlign = ContentAlignment.MiddleCenter;

            this.label11.BackColor = SystemColors.ButtonHighlight;
            this.label11.BorderStyle = BorderStyle.FixedSingle;
            this.label11.Dock = DockStyle.Fill;
            this.label11.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.label11.Margin = new Padding(3);
            this.label11.Name = "label11";
            this.label11.Text = "--";
            this.label11.TextAlign = ContentAlignment.MiddleCenter;
            this.label13.BackColor = SystemColors.ButtonHighlight;
            this.label13.BorderStyle = BorderStyle.FixedSingle;
            this.label13.Dock = DockStyle.Fill;
            this.label13.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.label13.Margin = new Padding(3);
            this.label13.Name = "label13";
            this.label13.Text = "--";
            this.label13.TextAlign = ContentAlignment.MiddleCenter;
            this.label15.BackColor = SystemColors.ButtonHighlight;
            this.label15.BorderStyle = BorderStyle.FixedSingle;
            this.label15.Dock = DockStyle.Fill;
            this.label15.Font = new Font("宋体", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.label15.Margin = new Padding(3);
            this.label15.Name = "label15";
            this.label15.Text = "--";
            this.label15.TextAlign = ContentAlignment.MiddleCenter;

            // groupInfo
            this.groupInfo.Controls.Add(this.infoLayout);
            this.groupInfo.Dock = DockStyle.Fill;
            this.groupInfo.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupInfo.Margin = new Padding(0);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Padding = new Padding(10);
            this.groupInfo.TabIndex = 3;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "实时信息";

            this.infoLayout.ColumnCount = 1;
            this.infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.infoLayout.Controls.Add(this.infoToolbar, 0, 0);
            this.infoLayout.Controls.Add(this.listViewInfo, 0, 1);
            this.infoLayout.Dock = DockStyle.Fill;
            this.infoLayout.Name = "infoLayout";
            this.infoLayout.RowCount = 2;
            this.infoLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            this.infoLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.infoLayout.TabIndex = 0;

            this.infoToolbar.ColumnCount = 2;
            this.infoToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.infoToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.infoToolbar.Controls.Add(this.btnOpenImage, 0, 0);
            this.infoToolbar.Controls.Add(this.btnClearLog, 1, 0);
            this.infoToolbar.Dock = DockStyle.Fill;
            this.infoToolbar.Margin = new Padding(0);
            this.infoToolbar.Name = "infoToolbar";
            this.infoToolbar.RowCount = 1;
            this.infoToolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.infoToolbar.TabIndex = 0;

            this.btnOpenImage.Dock = DockStyle.Fill;
            this.btnOpenImage.Margin = new Padding(0, 3, 4, 5);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.TabIndex = 0;
            this.btnOpenImage.Text = "打开图片";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.BtnOpenImage_Click);
            this.btnClearLog.Dock = DockStyle.Fill;
            this.btnClearLog.Margin = new Padding(4, 3, 0, 5);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);

            this.listViewInfo.Columns.AddRange(new ColumnHeader[] {
                this.columnHeader1,
                this.columnHeader2,
                this.columnHeader3});
            this.listViewInfo.Dock = DockStyle.Fill;
            this.listViewInfo.FullRowSelect = true;
            this.listViewInfo.GridLines = true;
            this.listViewInfo.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewInfo.HideSelection = false;
            this.listViewInfo.Name = "listViewInfo";
            this.listViewInfo.TabIndex = 1;
            this.listViewInfo.UseCompatibleStateImageBehavior = false;
            this.listViewInfo.View = View.Details;
            this.listViewInfo.Resize += new System.EventHandler(this.ListViewInfo_Resize);
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 82;
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "类型";
            this.columnHeader2.Width = 82;
            this.columnHeader3.Name = "columnHeader3";
            this.columnHeader3.Text = "信息";
            this.columnHeader3.Width = 340;

            // imagesLayout
            this.imagesLayout.BackColor = SystemColors.Control;
            this.imagesLayout.ColumnCount = 2;
            this.imagesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.imagesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.imagesLayout.Controls.Add(this.groupImage1, 0, 0);
            this.imagesLayout.Controls.Add(this.groupImage2, 1, 0);
            this.imagesLayout.Controls.Add(this.groupImage3, 0, 1);
            this.imagesLayout.Controls.Add(this.groupImage4, 1, 1);
            this.imagesLayout.Dock = DockStyle.Fill;
            this.imagesLayout.Margin = new Padding(0);
            this.imagesLayout.Name = "imagesLayout";
            this.imagesLayout.RowCount = 2;
            this.imagesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.imagesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.imagesLayout.TabIndex = 1;

            this.groupImage1.Controls.Add(this.pictureBox1);
            this.groupImage1.Dock = DockStyle.Fill;
            this.groupImage1.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupImage1.Margin = new Padding(0, 0, 5, 5);
            this.groupImage1.Name = "groupImage1";
            this.groupImage1.Padding = new Padding(8);
            this.groupImage1.TabIndex = 0;
            this.groupImage1.TabStop = false;
            this.groupImage1.Text = "前相机原图";
            this.groupImage2.Controls.Add(this.pictureBox2);
            this.groupImage2.Dock = DockStyle.Fill;
            this.groupImage2.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupImage2.Margin = new Padding(5, 0, 0, 5);
            this.groupImage2.Name = "groupImage2";
            this.groupImage2.Padding = new Padding(8);
            this.groupImage2.TabIndex = 1;
            this.groupImage2.TabStop = false;
            this.groupImage2.Text = "前相机结果";
            this.groupImage3.Controls.Add(this.pictureBox3);
            this.groupImage3.Dock = DockStyle.Fill;
            this.groupImage3.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupImage3.Margin = new Padding(0, 5, 5, 0);
            this.groupImage3.Name = "groupImage3";
            this.groupImage3.Padding = new Padding(8);
            this.groupImage3.TabIndex = 2;
            this.groupImage3.TabStop = false;
            this.groupImage3.Text = "后相机原图";
            this.groupImage4.Controls.Add(this.pictureBox4);
            this.groupImage4.Dock = DockStyle.Fill;
            this.groupImage4.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.groupImage4.Margin = new Padding(5, 5, 0, 0);
            this.groupImage4.Name = "groupImage4";
            this.groupImage4.Padding = new Padding(8);
            this.groupImage4.TabIndex = 3;
            this.groupImage4.TabStop = false;
            this.groupImage4.Text = "后相机结果";

            this.pictureBox1.BackColor = Color.FromArgb(40, 40, 40);
            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox2.BackColor = Color.FromArgb(40, 40, 40);
            this.pictureBox2.Dock = DockStyle.Fill;
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox3.BackColor = Color.FromArgb(40, 40, 40);
            this.pictureBox3.Dock = DockStyle.Fill;
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            this.pictureBox4.BackColor = Color.FromArgb(40, 40, 40);
            this.pictureBox4.Dock = DockStyle.Fill;
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;

            // TabMain
            this.AutoScaleDimensions = new SizeF(192F, 192F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = SystemColors.Control;
            this.Controls.Add(this.rootLayout);
            this.Name = "TabMain";
            this.Size = new Size(2363, 1271);

            this.rootLayout.ResumeLayout(false);
            this.leftLayout.ResumeLayout(false);
            this.groupCapacity.ResumeLayout(false);
            this.capacityLayout.ResumeLayout(false);
            this.groupSave.ResumeLayout(false);
            this.saveLayout.ResumeLayout(false);
            this.groupResult.ResumeLayout(false);
            this.resultLayout.ResumeLayout(false);
            this.groupInfo.ResumeLayout(false);
            this.infoLayout.ResumeLayout(false);
            this.infoToolbar.ResumeLayout(false);
            this.imagesLayout.ResumeLayout(false);
            this.groupImage1.ResumeLayout(false);
            this.groupImage2.ResumeLayout(false);
            this.groupImage3.ResumeLayout(false);
            this.groupImage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private TableLayoutPanel leftLayout;
        private GroupBox groupCapacity;
        private TableLayoutPanel capacityLayout;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Button button1;
        private GroupBox groupSave;
        private TableLayoutPanel saveLayout;
        private RadioButton radioNoSave;
        private RadioButton radioSaveNg;
        private RadioButton radioSaveAll;
        private GroupBox groupResult;
        private TableLayoutPanel resultLayout;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private GroupBox groupInfo;
        private TableLayoutPanel infoLayout;
        private TableLayoutPanel infoToolbar;
        private Button btnOpenImage;
        private Button btnClearLog;
        private ListView listViewInfo;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private TableLayoutPanel imagesLayout;
        private GroupBox groupImage1;
        private GroupBox groupImage2;
        private GroupBox groupImage3;
        private GroupBox groupImage4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
    }
}

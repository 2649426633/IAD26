using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    partial class TabTemplate
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelImageHost = new System.Windows.Forms.Panel();
            this.panelImageViewport = new _180Detection.ZoomViewportPanel();
            this.pictureBoxTemplate = new _180Detection.SmoothZoomPictureBox();
            this.imageStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblImageSizeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPixelInfoStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.innerLayout = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxWindow = new System.Windows.Forms.GroupBox();
            this.tblWindow = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.btnFitWindow = new System.Windows.Forms.Button();
            this.btnFlipImage = new System.Windows.Forms.Button();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnLoadROI = new System.Windows.Forms.Button();
            this.btnCropROI = new System.Windows.Forms.Button();
            this.groupBoxDrawROI = new System.Windows.Forms.GroupBox();
            this.tblDrawROI = new System.Windows.Forms.TableLayoutPanel();
            this.btnLine = new System.Windows.Forms.Button();
            this.btnParallelRect = new System.Windows.Forms.Button();
            this.btnCircle = new System.Windows.Forms.Button();
            this.btnRotatedRect = new System.Windows.Forms.Button();
            this.btnDrawRegion = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.btnClearROI = new System.Windows.Forms.Button();
            this.btnSaveROI = new System.Windows.Forms.Button();
            this.groupBoxTemplate = new System.Windows.Forms.GroupBox();
            this.tblTemplate = new System.Windows.Forms.TableLayoutPanel();
            this.btnCreateTemplate = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.btnLoadTemplate = new System.Windows.Forms.Button();
            this.btnMatchTemplate = new System.Windows.Forms.Button();
            this.lblMatchScore = new System.Windows.Forms.Label();
            this.txtMatchScore = new System.Windows.Forms.TextBox();
            this.groupBoxParam = new System.Windows.Forms.GroupBox();
            this.tblParam = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadParam = new System.Windows.Forms.Button();
            this.btnSaveParam = new System.Windows.Forms.Button();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.txtScore = new System.Windows.Forms.TextBox();
            this.lblElements = new System.Windows.Forms.Label();
            this.txtElements = new System.Windows.Forms.TextBox();
            this.lblDetectHeight = new System.Windows.Forms.Label();
            this.txtDetectHeight = new System.Windows.Forms.TextBox();
            this.lblDetectWidth = new System.Windows.Forms.Label();
            this.txtDetectWidth = new System.Windows.Forms.TextBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.lblTransition = new System.Windows.Forms.Label();
            this.cmbTransition = new System.Windows.Forms.ComboBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.cmbSelect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel.SuspendLayout();
            this.panelImageHost.SuspendLayout();
            this.panelImageViewport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTemplate)).BeginInit();
            this.imageStatusStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.innerLayout.SuspendLayout();
            this.groupBoxWindow.SuspendLayout();
            this.tblWindow.SuspendLayout();
            this.groupBoxDrawROI.SuspendLayout();
            this.tblDrawROI.SuspendLayout();
            this.groupBoxTemplate.SuspendLayout();
            this.tblTemplate.SuspendLayout();
            this.groupBoxParam.SuspendLayout();
            this.tblParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tableLayoutPanel.Controls.Add(this.panelImageHost, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(12);
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(2363, 1271);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // panelImageHost
            // 
            this.panelImageHost.BackColor = System.Drawing.SystemColors.Control;
            this.panelImageHost.Controls.Add(this.panelImageViewport);
            this.panelImageHost.Controls.Add(this.imageStatusStrip);
            this.panelImageHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageHost.Location = new System.Drawing.Point(12, 12);
            this.panelImageHost.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelImageHost.Name = "panelImageHost";
            this.panelImageHost.Size = new System.Drawing.Size(1676, 1247);
            this.panelImageHost.TabIndex = 0;
            // 
            // panelImageViewport
            // 
            this.panelImageViewport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelImageViewport.Controls.Add(this.pictureBoxTemplate);
            this.panelImageViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageViewport.Location = new System.Drawing.Point(0, 0);
            this.panelImageViewport.Margin = new System.Windows.Forms.Padding(0);
            this.panelImageViewport.Name = "panelImageViewport";
            this.panelImageViewport.Size = new System.Drawing.Size(1676, 1209);
            this.panelImageViewport.TabIndex = 0;
            this.panelImageViewport.TabStop = true;
            // 
            // pictureBoxTemplate
            // 
            this.pictureBoxTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pictureBoxTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTemplate.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxTemplate.Name = "pictureBoxTemplate";
            this.pictureBoxTemplate.Size = new System.Drawing.Size(1676, 1209);
            this.pictureBoxTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTemplate.TabIndex = 0;
            this.pictureBoxTemplate.TabStop = false;
            this.pictureBoxTemplate.Click += new System.EventHandler(this.pictureBoxTemplate_Click);
            // 
            // imageStatusStrip
            // 
            this.imageStatusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.imageStatusStrip.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageStatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.imageStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblImageSizeStatus,
            this.lblPixelInfoStatus});
            this.imageStatusStrip.Location = new System.Drawing.Point(0, 1209);
            this.imageStatusStrip.Name = "imageStatusStrip";
            this.imageStatusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.imageStatusStrip.Size = new System.Drawing.Size(1676, 38);
            this.imageStatusStrip.SizingGrip = false;
            this.imageStatusStrip.TabIndex = 1;
            // 
            // lblImageSizeStatus
            // 
            this.lblImageSizeStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblImageSizeStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.lblImageSizeStatus.Name = "lblImageSizeStatus";
            this.lblImageSizeStatus.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.lblImageSizeStatus.Size = new System.Drawing.Size(192, 28);
            this.lblImageSizeStatus.Text = "图像: -- × --";
            // 
            // lblPixelInfoStatus
            // 
            this.lblPixelInfoStatus.Name = "lblPixelInfoStatus";
            this.lblPixelInfoStatus.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblPixelInfoStatus.Size = new System.Drawing.Size(536, 28);
            this.lblPixelInfoStatus.Text = "Column: ----  Row: ----,  Val: (--, --, --)";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.innerLayout);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1696, 12);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 1247);
            this.panel2.TabIndex = 1;
            // 
            // innerLayout
            // 
            this.innerLayout.ColumnCount = 1;
            this.innerLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.innerLayout.Controls.Add(this.groupBoxWindow, 0, 0);
            this.innerLayout.Controls.Add(this.groupBoxDrawROI, 0, 1);
            this.innerLayout.Controls.Add(this.groupBoxTemplate, 0, 2);
            this.innerLayout.Controls.Add(this.groupBoxParam, 0, 3);
            this.innerLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerLayout.Location = new System.Drawing.Point(0, 0);
            this.innerLayout.Name = "innerLayout";
            this.innerLayout.RowCount = 4;
            this.innerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.innerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.innerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.innerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.innerLayout.Size = new System.Drawing.Size(655, 1247);
            this.innerLayout.TabIndex = 0;
            // 
            // groupBoxWindow
            // 
            this.groupBoxWindow.Controls.Add(this.tblWindow);
            this.groupBoxWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxWindow.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxWindow.Location = new System.Drawing.Point(0, 0);
            this.groupBoxWindow.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.groupBoxWindow.Name = "groupBoxWindow";
            this.groupBoxWindow.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxWindow.Size = new System.Drawing.Size(655, 216);
            this.groupBoxWindow.TabIndex = 0;
            this.groupBoxWindow.TabStop = false;
            this.groupBoxWindow.Text = "窗口";
            // 
            // tblWindow
            // 
            this.tblWindow.ColumnCount = 2;
            this.tblWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblWindow.Controls.Add(this.btnLoadImage, 0, 0);
            this.tblWindow.Controls.Add(this.btnFitWindow, 1, 0);
            this.tblWindow.Controls.Add(this.btnFlipImage, 0, 1);
            this.tblWindow.Controls.Add(this.btnSaveImage, 1, 1);
            this.tblWindow.Controls.Add(this.btnLoadROI, 0, 2);
            this.tblWindow.Controls.Add(this.btnCropROI, 1, 2);
            this.tblWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblWindow.Location = new System.Drawing.Point(10, 42);
            this.tblWindow.Name = "tblWindow";
            this.tblWindow.RowCount = 3;
            this.tblWindow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblWindow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblWindow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblWindow.Size = new System.Drawing.Size(635, 164);
            this.tblWindow.TabIndex = 0;
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadImage.Location = new System.Drawing.Point(3, 3);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(311, 48);
            this.btnLoadImage.TabIndex = 0;
            this.btnLoadImage.Text = "加载图片";
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnFitWindow
            // 
            this.btnFitWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFitWindow.Location = new System.Drawing.Point(320, 3);
            this.btnFitWindow.Name = "btnFitWindow";
            this.btnFitWindow.Size = new System.Drawing.Size(312, 48);
            this.btnFitWindow.TabIndex = 1;
            this.btnFitWindow.Text = "适应窗口";
            // 
            // btnFlipImage
            // 
            this.btnFlipImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFlipImage.Location = new System.Drawing.Point(3, 57);
            this.btnFlipImage.Name = "btnFlipImage";
            this.btnFlipImage.Size = new System.Drawing.Size(311, 48);
            this.btnFlipImage.TabIndex = 2;
            this.btnFlipImage.Text = "翻转图像";
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveImage.Location = new System.Drawing.Point(320, 57);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(312, 48);
            this.btnSaveImage.TabIndex = 3;
            this.btnSaveImage.Text = "保存图片";
            // 
            // btnLoadROI
            // 
            this.btnLoadROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadROI.Location = new System.Drawing.Point(3, 111);
            this.btnLoadROI.Name = "btnLoadROI";
            this.btnLoadROI.Size = new System.Drawing.Size(311, 50);
            this.btnLoadROI.TabIndex = 4;
            this.btnLoadROI.Text = "加载ROI";
            // 
            // btnCropROI
            // 
            this.btnCropROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropROI.Location = new System.Drawing.Point(320, 111);
            this.btnCropROI.Name = "btnCropROI";
            this.btnCropROI.Size = new System.Drawing.Size(312, 50);
            this.btnCropROI.TabIndex = 5;
            this.btnCropROI.Text = "截取ROI图像";
            // 
            // groupBoxDrawROI
            // 
            this.groupBoxDrawROI.Controls.Add(this.tblDrawROI);
            this.groupBoxDrawROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDrawROI.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDrawROI.Location = new System.Drawing.Point(0, 224);
            this.groupBoxDrawROI.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.groupBoxDrawROI.Name = "groupBoxDrawROI";
            this.groupBoxDrawROI.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxDrawROI.Size = new System.Drawing.Size(655, 266);
            this.groupBoxDrawROI.TabIndex = 1;
            this.groupBoxDrawROI.TabStop = false;
            this.groupBoxDrawROI.Text = "绘制ROI";
            // 
            // tblDrawROI
            // 
            this.tblDrawROI.ColumnCount = 2;
            this.tblDrawROI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblDrawROI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblDrawROI.Controls.Add(this.btnLine, 0, 0);
            this.tblDrawROI.Controls.Add(this.btnParallelRect, 1, 0);
            this.tblDrawROI.Controls.Add(this.btnCircle, 0, 1);
            this.tblDrawROI.Controls.Add(this.btnRotatedRect, 1, 1);
            this.tblDrawROI.Controls.Add(this.btnDrawRegion, 0, 2);
            this.tblDrawROI.Controls.Add(this.btnErase, 1, 2);
            this.tblDrawROI.Controls.Add(this.btnClearROI, 0, 3);
            this.tblDrawROI.Controls.Add(this.btnSaveROI, 1, 3);
            this.tblDrawROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblDrawROI.Location = new System.Drawing.Point(10, 42);
            this.tblDrawROI.Name = "tblDrawROI";
            this.tblDrawROI.RowCount = 4;
            this.tblDrawROI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblDrawROI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblDrawROI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblDrawROI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblDrawROI.Size = new System.Drawing.Size(635, 214);
            this.tblDrawROI.TabIndex = 0;
            // 
            // btnLine
            // 
            this.btnLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLine.Location = new System.Drawing.Point(3, 3);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(311, 47);
            this.btnLine.TabIndex = 0;
            this.btnLine.Text = "线段";
            // 
            // btnParallelRect
            // 
            this.btnParallelRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnParallelRect.Location = new System.Drawing.Point(320, 3);
            this.btnParallelRect.Name = "btnParallelRect";
            this.btnParallelRect.Size = new System.Drawing.Size(312, 47);
            this.btnParallelRect.TabIndex = 1;
            this.btnParallelRect.Text = "平行矩形";
            // 
            // btnCircle
            // 
            this.btnCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCircle.Location = new System.Drawing.Point(3, 56);
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(311, 47);
            this.btnCircle.TabIndex = 2;
            this.btnCircle.Text = "圆形";
            // 
            // btnRotatedRect
            // 
            this.btnRotatedRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRotatedRect.Location = new System.Drawing.Point(320, 56);
            this.btnRotatedRect.Name = "btnRotatedRect";
            this.btnRotatedRect.Size = new System.Drawing.Size(312, 47);
            this.btnRotatedRect.TabIndex = 3;
            this.btnRotatedRect.Text = "旋转矩形";
            // 
            // btnDrawRegion
            // 
            this.btnDrawRegion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDrawRegion.Location = new System.Drawing.Point(3, 109);
            this.btnDrawRegion.Name = "btnDrawRegion";
            this.btnDrawRegion.Size = new System.Drawing.Size(311, 47);
            this.btnDrawRegion.TabIndex = 4;
            this.btnDrawRegion.Text = "画区域";
            // 
            // btnErase
            // 
            this.btnErase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnErase.Location = new System.Drawing.Point(320, 109);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(312, 47);
            this.btnErase.TabIndex = 5;
            this.btnErase.Text = "擦除";
            // 
            // btnClearROI
            // 
            this.btnClearROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearROI.Location = new System.Drawing.Point(3, 162);
            this.btnClearROI.Name = "btnClearROI";
            this.btnClearROI.Size = new System.Drawing.Size(311, 49);
            this.btnClearROI.TabIndex = 6;
            this.btnClearROI.Text = "清除ROI";
            // 
            // btnSaveROI
            // 
            this.btnSaveROI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveROI.Location = new System.Drawing.Point(320, 162);
            this.btnSaveROI.Name = "btnSaveROI";
            this.btnSaveROI.Size = new System.Drawing.Size(312, 49);
            this.btnSaveROI.TabIndex = 7;
            this.btnSaveROI.Text = "保存ROI";
            // 
            // groupBoxTemplate
            // 
            this.groupBoxTemplate.Controls.Add(this.tblTemplate);
            this.groupBoxTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTemplate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxTemplate.Location = new System.Drawing.Point(0, 498);
            this.groupBoxTemplate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.groupBoxTemplate.Name = "groupBoxTemplate";
            this.groupBoxTemplate.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxTemplate.Size = new System.Drawing.Size(655, 241);
            this.groupBoxTemplate.TabIndex = 2;
            this.groupBoxTemplate.TabStop = false;
            this.groupBoxTemplate.Text = "模板";
            // 
            // tblTemplate
            // 
            this.tblTemplate.ColumnCount = 2;
            this.tblTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblTemplate.Controls.Add(this.btnCreateTemplate, 0, 0);
            this.tblTemplate.Controls.Add(this.btnSaveTemplate, 1, 0);
            this.tblTemplate.Controls.Add(this.btnLoadTemplate, 0, 1);
            this.tblTemplate.Controls.Add(this.btnMatchTemplate, 1, 1);
            this.tblTemplate.Controls.Add(this.lblMatchScore, 0, 2);
            this.tblTemplate.Controls.Add(this.txtMatchScore, 1, 2);
            this.tblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTemplate.Location = new System.Drawing.Point(10, 42);
            this.tblTemplate.Name = "tblTemplate";
            this.tblTemplate.RowCount = 3;
            this.tblTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblTemplate.Size = new System.Drawing.Size(635, 189);
            this.tblTemplate.TabIndex = 0;
            // 
            // btnCreateTemplate
            // 
            this.btnCreateTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateTemplate.Location = new System.Drawing.Point(3, 3);
            this.btnCreateTemplate.Name = "btnCreateTemplate";
            this.btnCreateTemplate.Size = new System.Drawing.Size(248, 50);
            this.btnCreateTemplate.TabIndex = 0;
            this.btnCreateTemplate.Text = "创建模板";
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveTemplate.Location = new System.Drawing.Point(257, 3);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(375, 50);
            this.btnSaveTemplate.TabIndex = 1;
            this.btnSaveTemplate.Text = "保存模板";
            // 
            // btnLoadTemplate
            // 
            this.btnLoadTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadTemplate.Location = new System.Drawing.Point(3, 59);
            this.btnLoadTemplate.Name = "btnLoadTemplate";
            this.btnLoadTemplate.Size = new System.Drawing.Size(248, 50);
            this.btnLoadTemplate.TabIndex = 2;
            this.btnLoadTemplate.Text = "加载模板";
            // 
            // btnMatchTemplate
            // 
            this.btnMatchTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMatchTemplate.Location = new System.Drawing.Point(257, 59);
            this.btnMatchTemplate.Name = "btnMatchTemplate";
            this.btnMatchTemplate.Size = new System.Drawing.Size(375, 50);
            this.btnMatchTemplate.TabIndex = 3;
            this.btnMatchTemplate.Text = "模板匹配";
            // 
            // lblMatchScore
            // 
            this.lblMatchScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMatchScore.Location = new System.Drawing.Point(3, 112);
            this.lblMatchScore.Name = "lblMatchScore";
            this.lblMatchScore.Size = new System.Drawing.Size(248, 77);
            this.lblMatchScore.TabIndex = 4;
            this.lblMatchScore.Text = "匹配得分";
            this.lblMatchScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMatchScore
            // 
            this.txtMatchScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMatchScore.Location = new System.Drawing.Point(257, 115);
            this.txtMatchScore.Name = "txtMatchScore";
            this.txtMatchScore.ReadOnly = true;
            this.txtMatchScore.Size = new System.Drawing.Size(375, 39);
            this.txtMatchScore.TabIndex = 5;
            this.txtMatchScore.Text = "--";
            this.txtMatchScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxParam
            // 
            this.groupBoxParam.Controls.Add(this.tblParam);
            this.groupBoxParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParam.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxParam.Location = new System.Drawing.Point(0, 747);
            this.groupBoxParam.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxParam.Name = "groupBoxParam";
            this.groupBoxParam.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxParam.Size = new System.Drawing.Size(655, 500);
            this.groupBoxParam.TabIndex = 3;
            this.groupBoxParam.TabStop = false;
            this.groupBoxParam.Text = "参数";
            // 
            // tblParam
            // 
            this.tblParam.ColumnCount = 2;
            this.tblParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tblParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tblParam.Controls.Add(this.btnLoadParam, 0, 0);
            this.tblParam.Controls.Add(this.btnSaveParam, 1, 0);
            this.tblParam.Controls.Add(this.lblType, 0, 1);
            this.tblParam.Controls.Add(this.cmbType, 1, 1);
            this.tblParam.Controls.Add(this.lblScore, 0, 2);
            this.tblParam.Controls.Add(this.txtScore, 1, 2);
            this.tblParam.Controls.Add(this.lblElements, 0, 3);
            this.tblParam.Controls.Add(this.txtElements, 1, 3);
            this.tblParam.Controls.Add(this.lblDetectHeight, 0, 4);
            this.tblParam.Controls.Add(this.txtDetectHeight, 1, 4);
            this.tblParam.Controls.Add(this.lblDetectWidth, 0, 5);
            this.tblParam.Controls.Add(this.txtDetectWidth, 1, 5);
            this.tblParam.Controls.Add(this.lblThreshold, 0, 6);
            this.tblParam.Controls.Add(this.txtThreshold, 1, 6);
            this.tblParam.Controls.Add(this.lblTransition, 0, 7);
            this.tblParam.Controls.Add(this.cmbTransition, 1, 7);
            this.tblParam.Controls.Add(this.lblSelect, 0, 8);
            this.tblParam.Controls.Add(this.cmbSelect, 1, 8);
            this.tblParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblParam.Location = new System.Drawing.Point(10, 42);
            this.tblParam.Name = "tblParam";
            this.tblParam.RowCount = 9;
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.111F));
            this.tblParam.Size = new System.Drawing.Size(635, 448);
            this.tblParam.TabIndex = 0;
            // 
            // btnLoadParam
            // 
            this.btnLoadParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadParam.Location = new System.Drawing.Point(3, 3);
            this.btnLoadParam.Name = "btnLoadParam";
            this.btnLoadParam.Size = new System.Drawing.Size(216, 43);
            this.btnLoadParam.TabIndex = 0;
            this.btnLoadParam.Text = "加载参数";
            // 
            // btnSaveParam
            // 
            this.btnSaveParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveParam.Location = new System.Drawing.Point(225, 3);
            this.btnSaveParam.Name = "btnSaveParam";
            this.btnSaveParam.Size = new System.Drawing.Size(407, 43);
            this.btnSaveParam.TabIndex = 1;
            this.btnSaveParam.Text = "保存参数";
            // 
            // lblType
            // 
            this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblType.Location = new System.Drawing.Point(3, 49);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(216, 49);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Type";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbType
            // 
            this.cmbType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Items.AddRange(new object[] {
            "null"});
            this.cmbType.Location = new System.Drawing.Point(225, 52);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(407, 36);
            this.cmbType.TabIndex = 3;
            // 
            // lblScore
            // 
            this.lblScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScore.Location = new System.Drawing.Point(3, 98);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(216, 49);
            this.lblScore.TabIndex = 4;
            this.lblScore.Text = "Score";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtScore
            // 
            this.txtScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScore.Location = new System.Drawing.Point(225, 101);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(407, 39);
            this.txtScore.TabIndex = 5;
            this.txtScore.Text = "0.7";
            // 
            // lblElements
            // 
            this.lblElements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblElements.Location = new System.Drawing.Point(3, 147);
            this.lblElements.Name = "lblElements";
            this.lblElements.Size = new System.Drawing.Size(216, 49);
            this.lblElements.TabIndex = 6;
            this.lblElements.Text = "Elements";
            this.lblElements.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtElements
            // 
            this.txtElements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtElements.Location = new System.Drawing.Point(225, 150);
            this.txtElements.Name = "txtElements";
            this.txtElements.Size = new System.Drawing.Size(407, 39);
            this.txtElements.TabIndex = 7;
            this.txtElements.Text = "15";
            // 
            // lblDetectHeight
            // 
            this.lblDetectHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetectHeight.Location = new System.Drawing.Point(3, 196);
            this.lblDetectHeight.Name = "lblDetectHeight";
            this.lblDetectHeight.Size = new System.Drawing.Size(216, 49);
            this.lblDetectHeight.TabIndex = 8;
            this.lblDetectHeight.Text = "DetectHeight";
            this.lblDetectHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDetectHeight
            // 
            this.txtDetectHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetectHeight.Location = new System.Drawing.Point(225, 199);
            this.txtDetectHeight.Name = "txtDetectHeight";
            this.txtDetectHeight.Size = new System.Drawing.Size(407, 39);
            this.txtDetectHeight.TabIndex = 9;
            this.txtDetectHeight.Text = "50";
            // 
            // lblDetectWidth
            // 
            this.lblDetectWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetectWidth.Location = new System.Drawing.Point(3, 245);
            this.lblDetectWidth.Name = "lblDetectWidth";
            this.lblDetectWidth.Size = new System.Drawing.Size(216, 49);
            this.lblDetectWidth.TabIndex = 10;
            this.lblDetectWidth.Text = "DetectWidth";
            this.lblDetectWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDetectWidth
            // 
            this.txtDetectWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetectWidth.Location = new System.Drawing.Point(225, 248);
            this.txtDetectWidth.Name = "txtDetectWidth";
            this.txtDetectWidth.Size = new System.Drawing.Size(407, 39);
            this.txtDetectWidth.TabIndex = 11;
            this.txtDetectWidth.Text = "15";
            // 
            // lblThreshold
            // 
            this.lblThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblThreshold.Location = new System.Drawing.Point(3, 294);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(216, 49);
            this.lblThreshold.TabIndex = 12;
            this.lblThreshold.Text = "Threshold";
            this.lblThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtThreshold
            // 
            this.txtThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtThreshold.Location = new System.Drawing.Point(225, 297);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new System.Drawing.Size(407, 39);
            this.txtThreshold.TabIndex = 13;
            this.txtThreshold.Text = "20";
            // 
            // lblTransition
            // 
            this.lblTransition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransition.Location = new System.Drawing.Point(3, 343);
            this.lblTransition.Name = "lblTransition";
            this.lblTransition.Size = new System.Drawing.Size(216, 49);
            this.lblTransition.TabIndex = 14;
            this.lblTransition.Text = "Transition";
            this.lblTransition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTransition
            // 
            this.cmbTransition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTransition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransition.Items.AddRange(new object[] {
            "all"});
            this.cmbTransition.Location = new System.Drawing.Point(225, 346);
            this.cmbTransition.Name = "cmbTransition";
            this.cmbTransition.Size = new System.Drawing.Size(407, 36);
            this.cmbTransition.TabIndex = 15;
            // 
            // lblSelect
            // 
            this.lblSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelect.Location = new System.Drawing.Point(3, 392);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(216, 56);
            this.lblSelect.TabIndex = 16;
            this.lblSelect.Text = "Select";
            this.lblSelect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSelect
            // 
            this.cmbSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelect.Items.AddRange(new object[] {
            "first",
            "last",
            "max"});
            this.cmbSelect.Location = new System.Drawing.Point(225, 395);
            this.cmbSelect.Name = "cmbSelect";
            this.cmbSelect.Size = new System.Drawing.Size(407, 36);
            this.cmbSelect.TabIndex = 17;
            // 
            // TabTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "TabTemplate";
            this.Size = new System.Drawing.Size(2363, 1271);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panelImageHost.ResumeLayout(false);
            this.panelImageHost.PerformLayout();
            this.panelImageViewport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTemplate)).EndInit();
            this.imageStatusStrip.ResumeLayout(false);
            this.imageStatusStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.innerLayout.ResumeLayout(false);
            this.groupBoxWindow.ResumeLayout(false);
            this.tblWindow.ResumeLayout(false);
            this.groupBoxDrawROI.ResumeLayout(false);
            this.tblDrawROI.ResumeLayout(false);
            this.groupBoxTemplate.ResumeLayout(false);
            this.tblTemplate.ResumeLayout(false);
            this.tblTemplate.PerformLayout();
            this.groupBoxParam.ResumeLayout(false);
            this.tblParam.ResumeLayout(false);
            this.tblParam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // ========== 原有控件及新增布局控件引用 ========== 
        private SmoothZoomPictureBox pictureBoxTemplate;
        private TableLayoutPanel tableLayoutPanel;
        private Panel panelImageHost;
        private ZoomViewportPanel panelImageViewport;
        private StatusStrip imageStatusStrip;
        private ToolStripStatusLabel lblImageSizeStatus;
        private ToolStripStatusLabel lblPixelInfoStatus;
        private Panel panel2;
        private TableLayoutPanel innerLayout;

        // 1. 窗口
        private GroupBox groupBoxWindow;
        private TableLayoutPanel tblWindow;
        private Button btnLoadImage;
        private Button btnFitWindow;
        private Button btnFlipImage;
        private Button btnSaveImage;
        private Button btnLoadROI;
        private Button btnCropROI;

        // 2. 绘制ROI
        private GroupBox groupBoxDrawROI;
        private TableLayoutPanel tblDrawROI;
        private Button btnLine;
        private Button btnParallelRect;
        private Button btnCircle;
        private Button btnRotatedRect;
        private Button btnDrawRegion;
        private Button btnErase;
        private Button btnClearROI;
        private Button btnSaveROI;

        // 3. 模板
        private GroupBox groupBoxTemplate;
        private TableLayoutPanel tblTemplate;
        private Button btnCreateTemplate;
        private Button btnSaveTemplate;
        private Button btnLoadTemplate;
        private Button btnMatchTemplate;
        private Label lblMatchScore;
        private TextBox txtMatchScore;

        // 4. 参数
        private GroupBox groupBoxParam;
        private TableLayoutPanel tblParam;
        private Button btnLoadParam;
        private Button btnSaveParam;
        private Label lblType;
        private ComboBox cmbType;
        private Label lblScore;
        private TextBox txtScore;
        private Label lblElements;
        private TextBox txtElements;
        private Label lblDetectHeight;
        private TextBox txtDetectHeight;
        private Label lblDetectWidth;
        private TextBox txtDetectWidth;
        private Label lblThreshold;
        private TextBox txtThreshold;
        private Label lblTransition;
        private ComboBox cmbTransition;
        private Label lblSelect;
        private ComboBox cmbSelect;
    }
}

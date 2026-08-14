using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    partial class Index
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
            this.sigin = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.tabMain = new _180Detection.TabMain();
            this.tabPageTemplate = new System.Windows.Forms.TabPage();
            this.tabTemplate = new _180Detection.TabTemplate();
            this.tabPageAnalysis = new System.Windows.Forms.TabPage();
            this.tabAnalysis = new _180Detection.TabAnalysis();
            this.sigin.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageTemplate.SuspendLayout();
            this.tabPageAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // sigin
            // 
            this.sigin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sigin.Controls.Add(this.tabPageMain);
            this.sigin.Controls.Add(this.tabPageTemplate);
            this.sigin.Controls.Add(this.tabPageAnalysis);
            this.sigin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sigin.Location = new System.Drawing.Point(0, 0);
            this.sigin.Name = "sigin";
            this.sigin.SelectedIndex = 0;
            this.sigin.Size = new System.Drawing.Size(2379, 1326);
            this.sigin.TabIndex = 2;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.tabMain);
            this.tabPageMain.Location = new System.Drawing.Point(8, 47);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(2363, 1271);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "主页";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tabMain
            // 
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(3, 3);
            this.tabMain.Name = "tabMain";
            this.tabMain.Size = new System.Drawing.Size(2357, 1265);
            this.tabMain.TabIndex = 0;
            // 
            // tabPageTemplate
            // 
            this.tabPageTemplate.Controls.Add(this.tabTemplate);
            this.tabPageTemplate.Location = new System.Drawing.Point(8, 47);
            this.tabPageTemplate.Name = "tabPageTemplate";
            this.tabPageTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTemplate.Size = new System.Drawing.Size(2363, 1271);
            this.tabPageTemplate.TabIndex = 1;
            this.tabPageTemplate.Text = "模板匹配";
            this.tabPageTemplate.UseVisualStyleBackColor = true;
            // 
            // tabTemplate
            // 
            this.tabTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTemplate.Location = new System.Drawing.Point(3, 3);
            this.tabTemplate.Name = "tabTemplate";
            this.tabTemplate.Size = new System.Drawing.Size(2357, 1265);
            this.tabTemplate.TabIndex = 0;
            this.tabTemplate.Load += new System.EventHandler(this.tabTemplate_Load);
            // 
            // tabPageAnalysis
            // 
            this.tabPageAnalysis.Controls.Add(this.tabAnalysis);
            this.tabPageAnalysis.Location = new System.Drawing.Point(8, 47);
            this.tabPageAnalysis.Name = "tabPageAnalysis";
            this.tabPageAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalysis.Size = new System.Drawing.Size(2363, 1271);
            this.tabPageAnalysis.TabIndex = 2;
            this.tabPageAnalysis.Text = "数据分析";
            this.tabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // tabAnalysis
            // 
            this.tabAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAnalysis.Location = new System.Drawing.Point(3, 3);
            this.tabAnalysis.Name = "tabAnalysis";
            this.tabAnalysis.Size = new System.Drawing.Size(2357, 1265);
            this.tabAnalysis.TabIndex = 0;
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2379, 1326);
            this.Controls.Add(this.sigin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "Index";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "180Detection";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Index_Load);
            this.sigin.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageTemplate.ResumeLayout(false);
            this.tabPageAnalysis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl sigin;
        private TabPage tabPageMain;
        private TabPage tabPageTemplate;
        private TabPage tabPageAnalysis;

        private TabMain tabMain;
        private TabTemplate tabTemplate;
        private TabAnalysis tabAnalysis;
    }
}

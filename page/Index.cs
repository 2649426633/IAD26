using System;
using System.Windows.Forms;

namespace _180Detection
{
    public partial class Index : Form
    {
        public TabMain TabMainPage { get; private set; }
        public TabTemplate TabTemplatePage { get; private set; }
        public TabAnalysis TabAnalysisPage { get; private set; }

        public Index()
        {
            InitializeComponent();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void tabTemplate_Load(object sender, EventArgs e)
        {

        }
    }
}

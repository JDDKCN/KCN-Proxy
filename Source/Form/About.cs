using System;

namespace KCNProxy
{
    public partial class About : Sunny.UI.UIForm
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.Text = $"关于 {Ver.APPName}";
            uiLabel1.Text = Ver.APPName;
            uiLabel2.Text = Ver.Version;
            uiLabel3.Text = Ver.copyright;
        }

        private void uiButton9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void uiLinkLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Ver.githubURL);
        }

        private void uiLinkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Ver.biliURL);
        }
    }
}

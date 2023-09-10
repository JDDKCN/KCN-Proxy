using System;

namespace KCNProxy
{
    public partial class Help : Sunny.UI.UIForm
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            uiRichTextBox1.Text = "1. 如果您的服务端是在本地运行的，请选择单机模式，否则请选择联机模式自定义IP。\r\n2. 若是单机模式，直接点击启动代理即可，如果是联机模式，请输入服务器IP/域名地址和端口号，再点击连接。若服务器是https连接，请开启使用SSL选项。\r\n3. 若进入游戏显示连接超时，请点击左上角设置-手动安装CA证书，再次开启代理即可。\r\n4. 如果您是第一次运行程序，请使用管理员模式启动，否则CA证书无法安装。";
        }
    }
}

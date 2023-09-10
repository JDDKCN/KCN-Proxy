using System;
using System.IO;
using Newtonsoft.Json;
using Sunny.UI;

namespace KCNProxy
{
    public partial class Home : Sunny.UI.UIForm
    {
        private bool isRunning = false; // 用于跟踪是否正在运行

        public Home()
        {
            InitializeComponent();

            UIButton btn = new UIButton();
            // 绑定按钮的点击事件
            btn.Click += uiButton2_Click;
        }

        private void Home_Load(object sender, EventArgs e)
        {
            string json = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);

            string IP = config.HTTP.Server_IP;
            int port = config.HTTP.Server_Port;
            int Mode = config.Mode.Server_Mode;
            bool SSL = config.Mode.EnableSSL;
            bool RunSilently = config.Mode.RunSilently;

            textBox1.Text = IP;
            textBox2.Text = $"{port}";

            radioButton1.Checked = (Mode == 1);
            radioButton2.Checked = (Mode == 2);

            uiSwitch1.Active = RunSilently;
            uiSwitch2.Active = SSL;

            RBSet();
            this.Text = Ver.APPName + Ver.Version;
        }

        private void RBSet()
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                uiSwitch2.Enabled = false;
            }
            else if (radioButton2.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                uiSwitch2.Enabled = true;
            }
        }

        private void RBSave()
        {
            string json = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);

            if (radioButton1.Checked)
            {
                config.Mode.Server_Mode = 1;
            }
            else if (radioButton2.Checked)
            {
                config.Mode.Server_Mode = 2;
            }

            string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText("config.json", updatedJson);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RBSet();
            RBSave();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RBSet();
            RBSave();
        }

        private void SwitchSave()
        {
            string json = File.ReadAllText("config.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);

            config.Mode.RunSilently = uiSwitch1.Active;
            config.Mode.EnableSSL = uiSwitch2.Active;

            string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText("config.json", updatedJson);
        }

        private void uiSwitch1_ValueChanged(object sender, bool value)
        {
            SwitchSave();
        }

        private void uiSwitch2_ValueChanged(object sender, bool value)
        {
            SwitchSave();
        }

        /// <summary>
        /// 保存代理设置
        /// </summary>
        public void ProxySave()
        {
            // 读取 JSON 文件内容
            string json = File.ReadAllText("config.json");

            // 解析 JSON
            Config config = JsonConvert.DeserializeObject<Config>(json);
            int.TryParse(textBox2.Text, out int port);

            // 修改配置内容
            config.HTTP.Server_IP = textBox1.Text;
            config.HTTP.Server_Port = port;

            // 将修改后的配置对象转为 JSON 格式
            string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);

            // 将修改后的 JSON 写回文件
            File.WriteAllText("config.json", updatedJson);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        private async void Start()
        {
            if (!InputChecker.IPCheckClassic(textBox1))
            {
                return;
            }

            if (!InputChecker.PortCheck(textBox2))
            {
                return;
            }

            if (!ProxyUtility.IsCAAsync())
            {
                if (ShowAskDialog("Error", "在您的计算机上未检测到CA证书，这会导致游戏代理失败。是否现在安装？按确定尝试安装，按取消跳过。"))
                {
                    await CAInstall.CAStart();
                    return;
                }
            }

            if (radioButton1.Checked)
            {
                if (!await WebUtility.CheckNetConnect("127.0.0.1", "443", false, false, true, false))
                {
                    return;
                }

                ProxyUtility.StartProxyConfig("127.0.0.1", 443, false);
                ProxyUtility.StartProxy(uiSwitch1.Active);

            }
            else if (radioButton2.Checked)
            {
                if (!await WebUtility.CheckNetConnect(textBox1.Text, textBox2.Text, uiSwitch2.Active, false, true, true))
                {
                    return;
                }

                ProxySave();

                int.TryParse(textBox2.Text, out int port);
                ProxyUtility.StartProxyConfig(textBox1.Text, port, uiSwitch2.Active);
                ProxyUtility.StartProxy(uiSwitch1.Active);

            }

            uiButton2.Text = "结束代理";
            uiButton2.Style = UIStyle.Red;
            isRunning = true;
        }

        private void Stop()
        {
            FileUtility.KillAll();
            ProxyUtility.CloseProxy();

            uiButton2.Text = "启动代理";
            uiButton2.Style = UIStyle.Green;
            isRunning = false;
        }

        private async void 手动安装CA证书ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!InputChecker.UserMode())
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog("Error", $"要使用此功能，您必须使用管理员模式打开程序！");
                return;
            }

            await CAInstall.CAStart();
        }

        private void uiLinkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Ver.biliURL);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void Home_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            FileUtility.KillAll();
            ProxyUtility.CloseProxy();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
    }
}

using System;
using System.IO;
using System.Windows.Forms;
using Sunny.UI;

namespace KCNProxy
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UIForm uIForm = new UIForm();

            if (File.Exists(".\\config.json") == false)
            {
                Config.ConfigWrite();
            }

            if (!File.Exists(".\\mitmdump.exe"))
            {
                uIForm.ShowErrorDialog("Error", $"在设置路径下未找到 Mitmdump 组件!");
                Application.Exit();
                return;
            }

            Application.Run(new Home());
        }
    }
}

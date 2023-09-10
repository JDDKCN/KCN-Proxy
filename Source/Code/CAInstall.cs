using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Sunny.UI;

namespace KCNProxy
{
    class CAInstall
    {
        public static async Task CAStart()
        {
            UIForm uIForm = new UIForm();
            string certPath = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.mitmproxy\\mitmproxy-ca-cert.cer";
            int Num = 0;

            if (File.Exists(certPath))
            {
                // 调用安装证书的方法
                InstallCertificate(certPath, string.Empty);
            }
            else
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe", // 指定cmd.exe作为启动的程序
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false, // 必须设置为false以便重定向输入和输出
                    CreateNoWindow = true,   // 不创建cmd窗口
                    WorkingDirectory = ".\\" // 设置工作目录
                };

                Process process = new Process { StartInfo = psi };
                process.Start();
                process.StandardInput.WriteLine("mitmdump");
                process.StandardInput.Flush();
                process.StandardInput.Close();

                while (true)
                {
                    if (File.Exists(certPath))
                    {
                        InstallCertificate(certPath, string.Empty);
                        break;
                    }

                    // 暂停一段时间再继续检查
                    await Task.Delay(1000);

                    int Time = Num++;
                    if (Time > 60)
                    {
                        uIForm.ShowErrorDialog("KCN-Server", "证书生成失败，请重试。");
                        break;
                    }
                }

                FileUtility.KillAll();
            }
        }

        /// <summary>
        /// 安装证书的方法
        /// </summary>
        /// <param name="certPath">证书路径</param>
        /// <param name="password">证书密码(如果有的话)</param>
        public static void InstallCertificate(string certPath, string password)
        {
            try
            {
                // 创建证书对象
                X509Certificate2 cert = new X509Certificate2(certPath, password);

                // 打开本地计算机的受信任的根证书存储区
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);

                // 将证书添加到存储区中
                store.Add(cert);

                // 关闭存储区
                store.Close();

                UIForm uIForm = new UIForm();
                uIForm.ShowSuccessDialog("提示", "CA证书已安装。");

            }
            catch (Exception ex)
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog("Error", ex.Message);
            }

        }
    }
}

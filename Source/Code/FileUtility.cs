using System;
using System.Diagnostics;
using System.IO;
using Sunny.UI;

namespace KCNProxy
{
    public class FileUtility
    {
        /// <summary>
        /// 程序工作目录
        /// </summary>
        public static string ThisDir = System.Environment.CurrentDirectory;

        /// <summary>
        /// 系统临时目录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TempIN(string name)
        {
            string TempPath = $"{System.IO.Path.GetTempPath()}KCN\\{name}";

            if (!Directory.Exists($"{System.IO.Path.GetTempPath()}KCN"))
            {
                Directory.CreateDirectory($"{System.IO.Path.GetTempPath()}KCN");
            }

            return TempPath;
        }

        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="Files">文件路径</param>
        /// <param name="Scr">是否显示窗口，flase为不显示</param>
        public static void OpenProcess(string Files, bool Scr = true)
        {
            try
            {
                Process proc = null;
                string targetDir = string.Format(Files);
                proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = Files;
                proc.StartInfo.Arguments = string.Format("10");
                if (Scr == true)//设置DOS窗口不显示
                {
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
                proc.Start();
            }
            catch (Exception ex)
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog("Error", ex.Message);
                return;
            }
        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool KillProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                    return true;
                }
                catch (Exception ex)
                {
                    UIForm uIForm = new UIForm();
                    uIForm.ShowErrorDialog("KCN-Server", $"无法结束进程 {process.ProcessName}: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// kill服务
        /// </summary>
        /// <param name="Src"></param>
        public static void KillAll(bool Src = true)
        {
            string FilePath = TempIN("KillAll.cmd");
            StreamWriter sw = new StreamWriter(FilePath, false);
            sw.WriteLine("@echo off");
            sw.WriteLine("title Loading...");
            sw.WriteLine("%1 mshta vbscript:CreateObject(\"Shell.Application\").ShellExecute(\"cmd.exe\",\" / c % ~s0::\",\"\",\"runas\",1)(window.close)&&exit");
            sw.WriteLine("cd /d %~dp0");
            sw.WriteLine("chcp 65001");
            sw.WriteLine("cls");
            sw.WriteLine("@echo off");
            sw.WriteLine("echo Made By KCN");
            sw.WriteLine("SET NAME=停止服务");
            sw.WriteLine("TITLE %NAME%");
            sw.WriteLine("REM COLOR C");
            sw.WriteLine("set mod=%1");
            sw.WriteLine("echo 正在关闭服务...");
            sw.WriteLine("taskkill /f /im mitmdump.exe");
            sw.WriteLine("taskkill /f /im ProxyMain.exe");
            sw.WriteLine("taskkill /f /im cmd.exe");
            sw.WriteLine("pause & exit");
            sw.Close();

            FileUtility.OpenProcess(FilePath, Src);
        }

    }
}

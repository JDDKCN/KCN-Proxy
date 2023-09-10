using System;
using System.IO;
using Microsoft.Win32;

namespace KCNProxy
{
    public class ProxyUtility
    {
        /// <summary>
        /// 启动代理(经典)
        /// </summary>
        /// <param name="Src">是否静默运行</param>
        public static void StartProxy(bool Src = true, bool proxy = true)
        {
            string FilePath = FileUtility.TempIN("Proxy.cmd");
            StreamWriter sw = new StreamWriter(FilePath, false);
            sw.WriteLine("@echo off");
            sw.WriteLine("title Loading...");
            sw.WriteLine("%1 mshta vbscript:CreateObject(\"Shell.Application\").ShellExecute(\"cmd.exe\",\" / c % ~s0::\",\"\",\"runas\",1)(window.close)&&exit");
            sw.WriteLine("cd /d %~dp0");
            sw.WriteLine("chcp 65001");
            sw.WriteLine("cls");
            sw.WriteLine("@echo off");
            sw.WriteLine("echo Made By KCN");
            sw.WriteLine("SET NAME=Mitmdump");
            sw.WriteLine("TITLE %NAME%");
            sw.WriteLine("REM COLOR C");
            sw.WriteLine("set mod=%1");
            sw.WriteLine("cd /d " + FileUtility.ThisDir);
            sw.WriteLine("mitmdump.exe -s proxy.py -k");
            sw.WriteLine("pause & exit");
            sw.Close();

            if (proxy == true)
            {
                //开启代理至127.0.0.1：8080
                Microsoft.Win32.RegistryKey RootKey = Microsoft.Win32.Registry.CurrentUser;
                Microsoft.Win32.RegistryKey SubKey = RootKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings\", true);
                SubKey.SetValue("ProxyEnable", 0x1);
                SubKey.SetValue("ProxyServer", "127.0.0.1:8080");
                SubKey.SetValue("ProxyEnable", 0x1);
            }

            FileUtility.OpenProcess(FilePath, Src);
        }

        /// <summary>
        /// 经典代理-改变配置
        /// </summary>
        /// <param name="HOST"></param>
        /// <param name="PORT"></param>
        /// <param name="USE_SSL"></param>
        public static void StartProxyConfig(string HOST, int PORT, bool USE_SSL = true)
        {
            string FilePath = $".\\proxy_config.py";
            using (StreamWriter sw = new StreamWriter(FilePath, false))
            {
                sw.WriteLine("import os");
                sw.WriteLine($"USE_SSL = {USE_SSL}");
                sw.WriteLine($"REMOTE_HOST = \"{HOST}\"");
                sw.WriteLine($"REMOTE_PORT = {PORT}");
                sw.WriteLine("REMOTE_HOST = os.getenv('MITM_REMOTE_HOST') if os.getenv('MITM_REMOTE_HOST') != None else REMOTE_HOST");
                sw.WriteLine("REMOTE_PORT = int(os.getenv('MITM_REMOTE_PORT')) if os.getenv('MITM_REMOTE_PORT') != None else REMOTE_PORT");
                sw.WriteLine("USE_SSL = bool(os.getenv('MITM_USE_SSL')) if os.getenv('MITM_USE_SSL') != None else USE_SSL");
                sw.WriteLine("print(f'MITM Remote Host: {REMOTE_HOST}')");
                sw.WriteLine("print(f'MITM Remote Port: {str(REMOTE_PORT)}')");
                sw.WriteLine("print(f'MITM Use SSL {str(USE_SSL)}')");
            }
        }

        /// <summary>
        /// 关闭代理
        /// </summary>
        public static void CloseProxy()
        {
            RegistryKey regKey = Registry.CurrentUser;
            string SubKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            RegistryKey optionKey = regKey.OpenSubKey(SubKeyPath, true);
            optionKey.SetValue("ProxyEnable", 0);
        }

        /// <summary>
        /// 检测CA证书是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsCAAsync()
        {
            string CAExist = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.mitmproxy\\mitmproxy-ca-cert.cer";
            if (!File.Exists(CAExist))
            {
                return false;
            }
            return true;
        }

    }
}

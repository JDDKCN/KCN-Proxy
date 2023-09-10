using System.IO;
using Newtonsoft.Json;

namespace KCNProxy
{
    public class Config
    {
        public int Version { get; set; }
        public HTTPConfig HTTP { get; set; }
        public ModeConfig Mode { get; set; }

        public class HTTPConfig
        {
            public string Server_IP { get; set; }
            public int Server_Port { get; set; }
        }

        public class ModeConfig
        {
            public int Server_Mode { get; set; }
            public bool EnableSSL { get; set; }
            public bool RunSilently { get; set; }

        }

        public static readonly string Config_Path = "./config.json";
        public static readonly Config Default_Config = new Config
        {
            Version = 1,
            HTTP = new HTTPConfig
            {
                Server_IP = "127.0.0.1",
                Server_Port = 443,
            },
            Mode = new ModeConfig
            {
                Server_Mode = 1,
                EnableSSL = false,
                RunSilently = true,
            }
        };

        public static void ConfigWrite()
        {
            Config config = Default_Config;
            UpdateConfig(config);
        }

        public static void UpdateConfig(Config config)
        {
            File.WriteAllText(Config_Path, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

    }
}

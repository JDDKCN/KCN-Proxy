using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sunny.UI;

namespace KCNProxy
{
    public class WebUtility
    {
        /// <summary>
        /// 检查服务器是否可用
        /// </summary>
        /// <param name="urlText"></param>
        /// <param name="portText"></param>
        /// <param name="sslCheck"></param>
        /// <param name="src"></param>
        /// <param name="err"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public static async Task<bool> CheckNetConnect(string urlText, string portText, bool sslCheck, bool src = true, bool err = true, bool save = false)
        {
            string url = (sslCheck ? "https://" : "http://") + urlText + ":" + portText;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    UIForm uIForm = new UIForm();

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Ensure the response is successful (HTTP 2xx status code)

                    stopwatch.Stop();

                    if (src)
                    {
                        using var httpClient = new HttpClient();
                        var apiUrl = $"{url}/status/server";

                        try
                        {
                            var resOnline = await httpClient.GetAsync(apiUrl);
                            var onlineData = await resOnline.Content.ReadAsStringAsync();
                            var resJson = JObject.Parse(onlineData);
                            var resOnlineCount = resJson["status"]["version"].Value<string>();
                            var resPlayerOnlineCount = resJson["status"]["playerCount"].Value<int>();

                            uIForm.ShowSuccessDialog($"测试连接：{url}", $"恭喜，连接成功！测试延迟：\n{stopwatch.Elapsed}\n\n[服务器版本] {resOnlineCount}\n[在线玩家数量] {resPlayerOnlineCount}人");

                        }
                        catch
                        {
                            uIForm.ShowSuccessDialog($"测试连接：{url}", $"恭喜，连接成功！测试延迟：\n{stopwatch.Elapsed}\n\n[错误]未获取到服务器信息！");
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (err)
                {
                    UIForm uIForm = new UIForm();
                    uIForm.ShowErrorDialog($"测试连接：{url}", $"服务器通讯错误:\n{ex.Message}\n请检查服务器地址是否拼写无误，或者更换其它服务器。");
                }

                return false;
            }
        }

    }
}

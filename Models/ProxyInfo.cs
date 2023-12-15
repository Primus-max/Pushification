using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

public class ProxyInfo
{
    public string IP { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ExternalIP { get; set; }

    public static ProxyInfo Parse(string proxyInfo)
    {
        if (string.IsNullOrEmpty(proxyInfo)) return null;

        string[] proxyParts = proxyInfo.Split(':');
        if (proxyParts.Length != 4)
        {
            // TODO здесь будет логирование
        }

        return new ProxyInfo
        {
            IP = proxyParts[0],
            Port = int.Parse(proxyParts[1]),
            Username = proxyParts[2],
            Password = proxyParts[3]
        };
    }

    // Модель для ответа JSON
    public class ExternalIPInfo
    {
        [JsonProperty("ip")]
        public string IP { get; set; }
    }

    /// <summary>
    /// Метод получения валидного прокси, если подходит внешний IP
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>ProxyInfo</returns>
    public static async Task<ProxyInfo> GetProxy(string filePath, int externalIpTimeoutInSeconds)
    {
        while (true)
        {
            string[] proxies = File.ReadAllLines(filePath);

            foreach (var proxyString in proxies)
            {
                if (string.IsNullOrWhiteSpace(proxyString)) continue;

                ProxyInfo proxy = Parse(proxyString);

                // Устанавливаем время ожидания получения внешнего IP
                proxy.ExternalIP = await GetExternalIP(proxy, externalIpTimeoutInSeconds);

                if (!IsIPInBlacklist(proxy.ExternalIP))
                {
                    return proxy;
                }

                // Пауза перед следующей попыткой
                await Task.Delay(10000); // Пауза в 10 секунд (10000 миллисекунд)
            }

            // Если нужно возвращаться к первому прокси после последнего, раскомментируйте следующую строку
            // currentProxyIndex = 0;
        }
    }


    // Проверка наличи IP в блеклисте
    private static bool IsIPInBlacklist(string proxy)
    {
        string blacklistFilePath = "blacklistproxy.txt";
        string[] blacklist = null;

        try
        {
            blacklist = File.ReadAllLines(blacklistFilePath);
            return blacklist.Contains(proxy);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Добавляет прокси в blacklist
    /// </summary>
    /// <param name="proxy"></param>
    public static void AddProxyToBlacklist(string proxy)
    {
        string blacklistFilePath = "blacklistproxy.txt";

        if (!IsIPInBlacklist(proxy))
        {
            try
            {
                if (!File.Exists(blacklistFilePath))
                    File.Create(blacklistFilePath);

                File.AppendAllLines(blacklistFilePath, new[] { proxy });
            }
            catch (Exception)
            {
                // Обработка ошибок записи в файл
            }
        }
    }

    // Получаю внешний IP
    private static async Task<string> GetExternalIP(ProxyInfo proxy, int timeoutWaitingIp)
    {
        using (HttpClientHandler handler = new HttpClientHandler())
        {
            handler.Proxy = new WebProxy(proxy.IP, proxy.Port)
            {
                Credentials = new System.Net.NetworkCredential(proxy.Username, proxy.Password)
            };

            using (HttpClient client = new HttpClient(handler))
            {
                try
                {
                    // Установка тайм-аута на получение внешнего IP
                    client.Timeout = TimeSpan.FromSeconds(timeoutWaitingIp);

                    // Получаю внешний IP 
                    HttpResponseMessage response = await client.GetAsync("https://api64.ipify.org?format=json");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        ExternalIPInfo externalIPInfo = JsonConvert.DeserializeObject<ExternalIPInfo>(responseBody);
                        return externalIPInfo.IP;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }

        return null;
    }

    // Удаление прокси из черного списка
    public static void RemoveProxyFromBlacklist(string proxy)
    {
        string blacklistFilePath = "blacklistproxy.txt";

        // Удаляем прокси из черного списка
        string[] updatedBlacklist = File.ReadAllLines(blacklistFilePath)
            .Where(line => line != proxy)
            .ToArray();

        File.WriteAllLines(blacklistFilePath, updatedBlacklist);
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
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
    public static async Task<ProxyInfo> GetProxy(string filePath, int externalIpTimeoutInSeconds, bool notificationMode = false)
    {
        string[] proxies = File.ReadAllLines(filePath);

        var cts = new CancellationTokenSource();
        var tasks = proxies
            .Where(proxyString => !string.IsNullOrWhiteSpace(proxyString))
            .Select(proxyString => Parse(proxyString))
            .Select(proxy => notificationMode ? Task.FromResult(proxy) : GetProxyWithExternalIP(proxy, externalIpTimeoutInSeconds, cts.Token))
            .ToList();

        try
        {
            // Ожидаем завершения любой из задач
            var completedTask = await Task.WhenAny(tasks);

            // Отменяем все оставшиеся задачи
            cts.Cancel();

            return await completedTask;
        }
        catch (OperationCanceledException)
        {
            // Возникает, если задачи были отменены
            return null;
        }
    }

    private static async Task<ProxyInfo> GetProxyWithExternalIP(ProxyInfo proxy, int externalIpTimeoutInSeconds, CancellationToken cancellationToken)
    {
        // Устанавливаем время ожидания получения внешнего IP
        proxy.ExternalIP = await GetExternalIP(proxy, externalIpTimeoutInSeconds);

        if (!IsIPInBlacklist(proxy.ExternalIP))
        {
            return proxy;
        }

        // Если задача была отменена, выбрасываем OperationCanceledException
        cancellationToken.ThrowIfCancellationRequested();

        return null;
    }


    private static async Task<string> GetExternalIP(ProxyInfo proxy, int timeoutInSeconds)
    {
        using (HttpClientHandler handler = new HttpClientHandler())
        {
            handler.Proxy = new WebProxy(proxy.IP, proxy.Port)
            {
                Credentials = new System.Net.NetworkCredential(proxy.Username, proxy.Password)
            };

            using (HttpClient client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
                try
                {
                    // Получаем внешний IP 
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




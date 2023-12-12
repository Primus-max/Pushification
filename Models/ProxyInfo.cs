using System;
using System.IO;
using System.Linq;

public class ProxyInfo
{
    public string IP { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public static ProxyInfo Parse(string proxyInfo)
    {
        if (string.IsNullOrEmpty(proxyInfo)) return null;

        string[] proxyParts = proxyInfo.Split(':');
        if (proxyParts.Length != 4)
        {
            throw new ArgumentException("Invalid proxy information format.");
        }

        return new ProxyInfo
        {
            IP = proxyParts[0],
            Port = int.Parse(proxyParts[1]),
            Username = proxyParts[2],
            Password = proxyParts[3]
        };
    }

    // Получаю прокси
    public static string GetProxy(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return null;

        string[] proxies = File.ReadAllLines(filePath);

        foreach (var proxy in proxies)
        {
            if (IsProxyInBlacklist(proxy)) continue;

            return proxy;
        }

        return null;
    }

    // Проверка, находится ли прокси в черном списке
    public static bool IsProxyInBlacklist(string proxy)
    {
        string blacklistFilePath = "blacklistproxy.txt";
        string[] blacklist = null;

        try
        {
            blacklist = File.ReadAllLines(blacklistFilePath);
            return blacklist.Contains(proxy);
        }
        catch (Exception) { return false; }
    }

    // Добавление прокси в черный список
    public static void AddProxyToBlacklist(string proxy)
    {
        string blacklistFilePath = "blacklistproxy.txt";

        // Проверяем, не находится ли прокси уже в черном списке
        if (!IsProxyInBlacklist(proxy))
        {
            try
            {
                if (!File.Exists(blacklistFilePath))
                    File.Create(blacklistFilePath);

                File.AppendAllLines(blacklistFilePath, new[] { proxy });

                // Теперь можно удалить прокси из основного списка (если это нужно)
                RemoveProxyFromMainList(proxy);
            }
            catch (Exception) { }
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

    // Удаление прокси из основного списка
    private static void RemoveProxyFromMainList(string proxy)
    {
        string mainListFilePath = "proxylist.txt";

        // Удаляем прокси из основного списка
        string[] updatedMainList = File.ReadAllLines(mainListFilePath)
            .Where(line => line != proxy)
            .ToArray();

        File.WriteAllLines(mainListFilePath, updatedMainList);
    }
}

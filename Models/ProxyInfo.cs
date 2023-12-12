using System;

public class ProxyInfo
{
    public string IP { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public static ProxyInfo Parse(string proxyInfo)
    {
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
}

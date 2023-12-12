using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public async Task<IBrowser> CreateDriver(string profilePath, ProxyInfo proxyInfo, string userAgent)
        {
            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = false,
                Args = new List<string>
        {
            "--start-maximized",
            $"--proxy-server=http://{proxyInfo.Username}:{proxyInfo.Password}@{proxyInfo.IP}:{proxyInfo.Port}"
        }.ToArray(),
                UserDataDir = profilePath,
            };

            if (!string.IsNullOrEmpty(userAgent))
            {
                launchOptions.Args.Append($"--user-agent={userAgent}");
            }

            return await Puppeteer.LaunchAsync(launchOptions);
        }


    }
}

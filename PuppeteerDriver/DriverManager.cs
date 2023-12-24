using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static async Task<IPage> CreatePageAsync(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
        {
            try
            {             
                Proxy proxy = new Proxy();
                proxy.Server = "http://"+ proxyInfo.IP + proxyInfo.Port;
                proxy.Username = proxyInfo.Username;
                proxy.Password = proxyInfo.Password;

                var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchPersistentContextAsync(profilePath, new BrowserTypeLaunchPersistentContextOptions
                {
                    Headless = useHeadlessMode,
                    UserAgent = userAgent,
                    Proxy = proxy
                });
                              

                var page = await browser.NewPageAsync();
                
                return page;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось создать страницу: {ex.Message}");
                // TODO: логирование
                return null;
            }
        }
    }
}

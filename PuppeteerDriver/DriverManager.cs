using Microsoft.Playwright;
using Pushification.Manager;
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
                Proxy proxy = null;

                if (proxyInfo != null)
                {
                    proxy = new Proxy();
                    proxy.Server = "http://" + proxyInfo.IP + ":" + proxyInfo.Port;
                    proxy.Username = proxyInfo.Username;
                    proxy.Password = proxyInfo.Password;
                }

                var playwright = await Playwright.CreateAsync();

                ViewportSize viewportSize = new ViewportSize();
                viewportSize.Width = 1920;
                viewportSize.Height = 1080;

                var browser = await playwright.Chromium.LaunchPersistentContextAsync(profilePath, new BrowserTypeLaunchPersistentContextOptions
                {
                    Headless = useHeadlessMode,
                    UserAgent = userAgent,
                    Proxy = proxy != null ? proxy : null,
                    ViewportSize = viewportSize
                }); ;


                var page = await browser.NewPageAsync();
                return page;
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать браузер: {ex.Message}");
                return null;
            }
        }
    }
}

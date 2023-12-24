using Microsoft.Playwright;
using Pushification.Manager;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.PuppeteerDriver
{
    public class DriverManager
    {
        public static async Task<IBrowserContext> CreatePageAsync(string profilePath, ProxyInfo proxyInfo = null, string userAgent = null, bool useHeadlessMode = false)
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

                //Screen screen = Screen.PrimaryScreen;

                //ViewportSize viewportSize = new ViewportSize();
                //viewportSize.Width = screen.Bounds.Width;
                //viewportSize.Height = screen.Bounds.Height;\
              
                var browser = await playwright.Chromium.LaunchPersistentContextAsync(profilePath, new BrowserTypeLaunchPersistentContextOptions
                {
                    Headless = useHeadlessMode,
                    UserAgent = userAgent,
                    Proxy = proxy != null ? proxy : null,
                   ViewportSize = ViewportSize.NoViewport,
                    Args = new[] { "--start-maximized" }
                }) ;

                return browser;
            }
            catch (Exception ex)
            {
                EventPublisherManager.RaiseUpdateUIMessage($"Не удалось создать браузер: {ex.Message}");
                return null;
            }
        }
    }
}

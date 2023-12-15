using PuppeteerSharp;
using Pushification.Manager;
using Pushification.Models;
using Pushification.PuppeteerDriver;
using Pushification.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.Services
{
    public class SubscribeService : IServiceWorker
    {
        private SubscriptionModeSettings _subscribeSettings = null;
        private IBrowser _browser = null;
        private IPage _page = null;

        public SubscribeService()
        {
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
        }

        public async Task Run()
        {
            int workingTime = _subscribeSettings.TimeOptionOne * 60 * 1000; // Преобразуем минуты в миллисекунды                       
            string url = _subscribeSettings.URL;

            // Цикл будет выполняться указанное в настройках время
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < workingTime)
            {
                string profilePath = ProfilesManager.CreateProfileFolderPath(); // Создаю папку профиля

                // Получаю прокси 
                string proxyFilePath = _subscribeSettings.ProxyList;
                ProxyInfo proxy = await ProxyInfo.GetProxy(proxyFilePath, _subscribeSettings.MaxTimeGettingOutIP);

                string userAgent = GetRandomUserAgent();

                _browser = await DriverManager.CreateDriver(profilePath, proxy, userAgent);

                if (_browser == null)
                {
                    // TODO здесь будет логирование
                    return;
                }

                _page = await _browser.NewPageAsync();

                // Авторизую прокси
                await _page.AuthenticateAsync(new Credentials() { Password = proxy.Password, Username = proxy.Username });
                try
                {
                    // Устанавливаю время ожидания загрузки страницы
                    int timeOutMillisecond = _subscribeSettings.MaxTimePageLoading * 1000;
                    await _page.WaitForTimeoutAsync(timeOutMillisecond);
                    await _page.GoToAsync(url);

                    // Извлекаем хост (домен) для передачи в виде простой строки без схемы
                    Uri uri = new Uri(_subscribeSettings?.URL);                   
                    string siteName = uri.Host;

                    // Подписываюсь на уведомление
                    bool IsSuccess = AutoItHandler.SubscribeToWindow(siteName, _subscribeSettings.BeforeAllowTimeout);

                    if (IsSuccess)
                    {
                        // Если успешно, то убираю прокси в блеклист
                        ProxyInfo.AddProxyToBlacklist(proxy.ExternalIP);

                        // Время ожидания после подписки
                        int afterAllowTimeoutMillisecond = _subscribeSettings.AfterAllowTimeout * 1000;
                        await Task.Delay(afterAllowTimeoutMillisecond);
                    }
                }
                catch (Exception)
                {
                    await StopAsync(profilePath);
                }               

                 await StopAsync(profilePath);
            }
        }

        // Закрываю браузер
        public async Task StopAsync(string profilePath)
        {
            // Закрыть браузер после прошествия времени
            await _browser.CloseAsync();
            await _page.DisposeAsync();

            // Удаляю лишние папки и файлы из профиля
            await Task.Delay(1000);
           ProfilesManager.RemoveCashFolders(profilePath);
        }
               

        // Метод получения рандомного юзер агента
        private string GetRandomUserAgent()
        {
            try
            {
                // Загрузка списка User-Agent'ов из файла
                string[] userAgents = LoadUserAgentsFromFile("useragents.txt");

                if (userAgents is null) return null;

                // Получение случайного User-Agent'а из списка
                Random random = new Random();
                int randomIndex = random.Next(userAgents.Length);
                return userAgents[randomIndex];
            }
            catch (Exception ex)
            {
                // Обработка ошибок при загрузке или выборе User-Agent'а
                MessageBox.Show($"Ошибка при получении User-Agent'а: {ex.Message}");
                return string.Empty;
            }
        }

        // Вспомогательный метод получения юзер агента
        private string[] LoadUserAgentsFromFile(string filePath)
        {
            try
            {
                // Чтение всех строк из файла
                string[] lines = File.ReadAllLines(filePath);

                // Фильтрация и удаление пустых строк
                string[] nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                return nonEmptyLines;
            }
            catch (Exception ex)
            {
                // Обработка ошибок при чтении файла
                MessageBox.Show($"Ошибка при загрузке User-Agent'ов: {ex.Message}");
                return new string[0];
            }
        }
    }
}

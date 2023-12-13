using PuppeteerSharp;
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
        private readonly DriverManager _driverManager = null;
        private SubscriptionModeSettings _subscribeSettings = null;
        private IBrowser _browser = null;
        private IPage _page = null;

        public SubscribeService(DriverManager driverManager)
        {
            _driverManager = driverManager;
            _subscribeSettings = SubscriptionModeSettings.LoadSubscriptionSettingsFromJson();
        }

        public async void Run()
        {
            int workingTime = _subscribeSettings.TimeOptionOne * 60 * 1000; // Преобразуем минуты в миллисекунды

            string profilePath = GetProfileFolderPath();
            string url = _subscribeSettings.URL;
            string proxyInfoString = ProxyInfo.GetProxy(_subscribeSettings.ProxyList);
            ProxyInfo proxyInfo = ProxyInfo.Parse(proxyInfoString);

            string userAgent = GetRandomUserAgent();

            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < workingTime)
            {
                _browser = await _driverManager.CreateDriver(profilePath, proxyInfo, userAgent);

                _page = await _browser.NewPageAsync();

                // Авторизую прокси
                await _page.AuthenticateAsync(new Credentials() { Password = proxyInfo.Password, Username = proxyInfo.Username });
                try
                {
                    // Уставливаю время ожидания загрузки страницы
                    int timeOutMillisecond = _subscribeSettings.MaxTimePageLoading * 1000;
                    await _page.WaitForTimeoutAsync(timeOutMillisecond);
                    await _page.GoToAsync(url);

                  await  AutoItHandler.SubscribeToWindow(_subscribeSettings?.URL, 10, _subscribeSettings.BeforeAllowTimeout);


                }
                catch (Exception)
                {
                    await StopAsync();
                }
                

                // Если успешно, то записываю этот прокси в блеклист               
               // ProxyInfo.AddProxyToBlacklist(proxyInfoString);

                // Ожидание перед следующей итерацией
                await Task.Delay(1000); // Подождать 1 секунду перед следующей итерацией
            }
        }

        public async Task StopAsync()
        {
            // Закрыть браузер после прошествия времени
            await _browser.CloseAsync();
            await _page.DisposeAsync();
        }

        // Метод получения пути профиля
        private string GetProfileFolderPath()
        {
            // Получаем текущее время в формате Unix timestamp
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            // Получаем текущую дату в формате dd-MM-yyyy
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");

            // Формируем название папки профиля
            string profileFolderName = $"{unixTimestamp}_{currentDate}";

            // Сформируйте полный путь к папке профиля в папке "profiles" в корне проекта
            string profilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles", profileFolderName);

            // Возвращаем полученный путь
            return profilePath;
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

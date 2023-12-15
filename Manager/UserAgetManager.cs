using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pushification.Manager
{
    public static class UserAgetManager
    {
        // Метод получения рандомного юзер агента
        public static string GetRandomUserAgent()
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
        private static string[] LoadUserAgentsFromFile(string filePath)
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

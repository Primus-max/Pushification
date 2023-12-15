using System.IO;
using System;
using System.Linq;

namespace Pushification.Manager
{
    public static class ProfilesManager
    {

        /// <summary>
        /// Метод создания пути профиля
        /// </summary>
        /// <returns>Путь к профилю</returns>
        public static string CreateProfileFolderPath()
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

        /// <summary>
        /// Метод получения самого старого профиля
        /// </summary>
        /// <returns>Путь к профилю</returns>
        public static string GetOldProfile()
        {
            string profilesDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles");

            if (!Directory.Exists(profilesDirectoryPath))
            {
                // Директория с профилями не существует
                return null;
            }

            // Получаем все поддиректории (профили) в директории profiles
            var profileDirectories = Directory.GetDirectories(profilesDirectoryPath);

            if (profileDirectories.Length == 0)
            {
                // В директории profiles нет поддиректорий (профилей)
                return null;
            }

            // Находим самую старую директорию
            string oldestProfileDirectory = profileDirectories
                .OrderBy(d => Directory.GetCreationTime(d))
                .FirstOrDefault();

            return oldestProfileDirectory;
        }
    }
}

using System.IO;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Pushification.Manager
{
    public static class ProfilesManager
    {
        /// <summary>
        /// Метод получения всех профилей
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllProfiles() 
        {
            List<string> profiles = new List<string>();
            string profilesDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profiles");

            if (!Directory.Exists(profilesDirectoryPath))
            {
                // Директория с профилями не существует
                return null;
            }
            // Получаем все поддиректории (профили) в директории profiles
            profiles = Directory.GetDirectories(profilesDirectoryPath).ToList();

            if (profiles.Count == 0)
            {
                // В директории profiles нет поддиректорий (профилей)
                return null;
            }

            return profiles;

        }

        /// <summary>
        /// Метод удаления профиля(директории)
        /// </summary>
        /// <param name="folderName"></param>
        public static void RemoveProfile(string folderName)
        {
            string rootPath = "profiles";

            try
            {
                string folderPath = Path.Combine(rootPath, folderName);

                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove folder {folderName}: {ex.Message}");
            }
        }


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

        // Прохожу по всем профилям и удаляю лишнее
        public static void RemoveCash()
        {
            string rootProfilesPath = "profiles";

            try
            {
                // Получаем все поддиректории в корневой папке профилей
                string[] profileDirectories = Directory.GetDirectories(rootProfilesPath);

                foreach (var profilePath in profileDirectories)
                {
                    RemoveCashFolders(profilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove folders from profiles: {ex.Message}");
            }
        }


        // Удаление папки кэша, занимает место
        public static void RemoveCashFolders(string profilePath)
        {
            string[] foldersToDelete = {
            "GPUCache",
            "Cache",
            "Code Cache",
            "DawnCache",
            "Extension Rules",
            "Extension Scripts",
            "Extension State",
            "Extensions",
            "Local Extension Settings",
            "GrShaderCache",
            "ShaderCache",
            "Crashpad",
            "GraphiteDawnCache"
        };

            try
            {
                foreach (var folderName in foldersToDelete)
                {
                    string folderPath = Path.Combine(profilePath, folderName);

                    // Проверяем, существует ли папка в корне профиля
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                        Thread.Sleep(100);
                    }
                    else
                    {
                        // Папка не найдена в корне профиля, поэтому пробуем внутри "Default"
                        folderPath = Path.Combine(profilePath, "Default", folderName);
                        if (Directory.Exists(folderPath))
                        {
                            Directory.Delete(folderPath, true);
                            Thread.Sleep(100);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove folders: {ex.Message}");
            }
        }
    }
}

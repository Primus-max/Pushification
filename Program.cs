using Microsoft.Extensions.DependencyInjection;
using Pushification.PuppeteerDriver;
using System;
using System.Windows.Forms;

namespace Pushification
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DateTime targetDate = DateTime.Now.AddDays(3);

            // Проверка, что текущая дата меньше целевой даты (завтра)
            if (DateTime.Now >= targetDate)
            {
                MessageBox.Show("Свяжитесь с разработчиком");
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());

            // Внедрение зависимостей
            var serviceProvider = new ServiceCollection()
             .AddScoped<DriverManager>()             
             .BuildServiceProvider();
        }
    }
}

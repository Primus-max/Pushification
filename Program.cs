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

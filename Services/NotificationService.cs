using Pushification.Services.Interfaces;
using System.Threading.Tasks;

namespace Pushification.Services
{
    public class NotificationService : IServiceWorker
    {
        public void Run()
        {
            throw new System.NotImplementedException();
        }

        

        Task IServiceWorker.StopAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}

using System.Threading.Tasks;

namespace Pushification.Services.Interfaces
{
    public interface IServiceWorker
    {
        Task Run();
        void StopBrowser();
    }
}

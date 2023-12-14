using System.Threading.Tasks;

namespace Pushification.Services.Interfaces
{
    public interface IServiceWorker
    {
        void Run();
        Task StopAsync(string profilePath);
    }
}

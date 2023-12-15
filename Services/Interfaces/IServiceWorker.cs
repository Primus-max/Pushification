using System.Threading.Tasks;

namespace Pushification.Services.Interfaces
{
    public interface IServiceWorker
    {
        Task Run();
        Task StopAsync(string profilePath);
    }
}

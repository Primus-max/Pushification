using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushification.Services.Interfaces
{
    public interface IServiceWorker
    {
        void Run();
        Task  StopAsync();
    }
}

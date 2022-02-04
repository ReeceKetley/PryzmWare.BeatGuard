using System.Threading;
using System.Threading.Tasks;

namespace BeatGuard_Engine
{
    public interface ISignalSource
    {
        Task<double[]> ReceiveAsync(CancellationToken cancellationToken);
    }
}
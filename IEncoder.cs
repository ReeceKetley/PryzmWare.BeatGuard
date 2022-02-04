using System.Threading;
using System.Threading.Tasks;
using Accord.Audio;

namespace BeatGuard_Engine
{
    public interface IEncoder
    {
        Task<double[]> EncodeAsync(double[] signal, FingerPrint fingerPrint, CancellationToken cancellationToken);
    }
}
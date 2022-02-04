using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accord.Audio;
using Accord.Math;

namespace BeatGuard_Engine
{
    public class Decoder : IDecoder
    {
        public static string Dss_Decode(Signal signal, int messageLength)
        {
            var lMsg = 8 * messageLength;
            var lMin = 8 * 1024;
            var sLen = signal.Length;
            var l2 = Math.Floor((double)sLen / (double)lMsg);
            var l = Math.Max(lMin, l2);
            var nFrame = Math.Floor(sLen / l);
            var sCh = signal.NumberOfChannels;
            var n = nFrame - (nFrame % 8);
            var xSig = new double[(int)n][];
            for (int i = 0; i < n; i++)
            {
                var segment = new double[(int)l];
                for (int j = 0; j < l; j++)
                {
                    segment[j] = signal.GetSample(0, (i * (int)l) + j);
                }

                xSig[i] = segment;
            }

            var averages = new double[(int)xSig.Length];
            var bits = new int[(int)xSig.Length];
            for (int i = 0; i < xSig.Length; i++)
            {
                var segment = xSig[i].Sum();
                var total = xSig[i].Average();
                total = segment / xSig[i].Length;
                if (total < 0)
                {
                    bits[i] = 0;
                }
                else
                {
                    bits[i] = 1;
                }

                averages[i] = total;
            }

            return Encoding.ASCII.GetString(ArrayExtension.GetBytes(bits));
        }
    }
}
using System.Collections;
using System.Linq;
using System.Text;

namespace BeatGuard_Engine
{
    public static class StringExtensions
    {
        public static int[] GetBits(this string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            var allBits = new int[bytes.Length * 8];
            for (int n = 0; n < bytes.Length; n++)
            {
                BitArray b = new BitArray(new byte[] { bytes[n] });
                int[] bits = b.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();
                bits = bits.Reverse().ToArray();
                bits.CopyTo(allBits, n * 8);
            }

            return allBits;
        }
    }
}
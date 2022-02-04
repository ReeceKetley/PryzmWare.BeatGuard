using System;
using System.Collections;
using System.Linq;

namespace BeatGuard_Engine
{
    class FingerPrintModulator : IFingerPrintModulator
    {
        public override int[] Modulate(FingerPrint fingerPrint)
        {
            var bytes = new Packet(fingerPrint.Value).GetBytes();
            Console.WriteLine(bytes.Length);
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
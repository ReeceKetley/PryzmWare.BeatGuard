using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatGuard_Engine
{
    public class Packet
    {
        public Packet(Guid guid) : this(guid.ToByteArray())
        {
        }

        public byte[] Data { get; }
        public int Checksum { get; }


        public Packet(byte[] data)
        {
            Data = data;
            Checksum = data.Select(x => (int) x).Sum();
        }

        public byte[] GetBytes()
        {
//            var header = new byte[] {0xff, 0xfe};
//            using (var ms = new MemoryStream())
//            using (var writer = new BinaryWriter(ms))
//            {
//                writer.Write(header);
//                writer.Write(Data.Length);
//                writer.Write(Data);
//                writer.Write(Checksum);
//                return ms.GetBuffer();
//            }
            return Data;
        }
    }

    public interface IBitFactory
    {
        int[] Create();
    }

    public class PacketBitFactory : IBitFactory
    {
        private readonly Packet _packet;

        public PacketBitFactory(Packet packet)
        {
            _packet = packet;
        }

        public int[] Create()
        {
            //var bytes = Encoding.ASCII.GetBytes(input);
            var bytes = _packet.GetBytes();
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

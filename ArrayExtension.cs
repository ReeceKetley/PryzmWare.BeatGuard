using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeatGuard_Engine
{
    public static class ArrayExtension
    {

        public static double[] ToDouble(this float[] arr) =>
            Array.ConvertAll(arr, x => (double)x);
        public static float[] ToFloat(this double[] arr) =>
            Array.ConvertAll(arr, x => (float)x);
        public static byte[] GetBytes(int[] bits)
        {
            string str = "";
            int[] numArray = bits;
            for (int i = 0; i < (int)numArray.Length; i++)
            {
                int num = numArray[i];
                str = string.Concat(str, num.ToString());
            }
            byte[] array = (
                from pos in Enumerable.Range(0, str.Length / 8)
                select Convert.ToByte(str.Substring(pos * 8, 8), 2)).ToArray<byte>();
            List<byte> nums = new List<byte>();
            for (int j = 0; j < (int)array.Length; j++)
            {
                nums.Add(array[j]);
            }
            return nums.ToArray();
        }

        public static int[] GetBits(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            int[] numArray = new int[(int)bytes.Length * 8];
            for (int i = 0; i < (int)bytes.Length; i++)
            {
                int[] array = (
                    from bool bit in new BitArray(new byte[] { bytes[i] })
                    select (bit ? 1 : 0)).ToArray<int>();
                array = array.Reverse<int>().ToArray<int>();
                array.CopyTo(numArray, i * 8);
            }
            return numArray;
        }

    }
}
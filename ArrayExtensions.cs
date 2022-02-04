using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Accord.Audio;
using Accord.Audio.Windows;
using Accord.Math;
using NAudio.Wave;

namespace BeatGuard_Engine
{
    public static class ArrayExtensions
    {
        

        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private static byte[] GetBytes(int[] bits)
        {
            var bitsString = "";
            foreach (var bit in bits)
            {
                bitsString += bit.ToString();
            }

            byte[] result = Enumerable.Range(0, bitsString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitsString.Substring(pos * 8, 8),
                    2)
                ).ToArray();

            List<byte> mahByteArray = new List<byte>();
            for (int i = 0; i < result.Length; i++)
            {
                mahByteArray.Add(result[i]);
            }

            return mahByteArray.ToArray();
        }


    }
}
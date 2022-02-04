using System;
using System.Reflection;
using Accord.Audio.Windows;
using Accord.Math;

namespace BeatGuard_Engine
{
    public class SignalMixer
    {
        public SignalMixer()
        {
        }

        public  double[] Mix(double l, int[] bits, double lower = -1, double upper = 1, double k = 256)
        {
            if (2 * k > l)
            {
                k = Math.Floor(l / 4) - (Math.Floor(l / 4) % 4);
            }
            else
            {
                k = k - (k % 4);
            }

            double[] mSig = RepeatBits(l, bits);

            double[] smoothSignal = SmoothBits(k, mSig);
            var abs1 = new double[smoothSignal.Length];
            for (int i = 0; i < smoothSignal.Length; i++)
            {
                abs1[i] = Math.Abs(smoothSignal[i]);
            }

            var max1 = abs1.Max();
            var start = (int)k / 2;
            var end = (int)smoothSignal.Length - k / 2;
            var wNorm = smoothSignal.SubArray(start, (int)end);
            for (int i = 0; i < wNorm.Length; i++)
            {
                wNorm[i] = wNorm[i] / max1;
            }

            for (int i = 0; i < wNorm.Length; i++)
            {
                wNorm[i] = wNorm[i] * (upper - lower) + lower;
            }

            return wNorm;
        }

        private  double[] SmoothBits(double length, double[] mSig)
        {
            //var sig = Signal.FromArray(mSig, mSig.Length);
            var han = RaisedCosineWindow.Hann((int)length);
            var window = (float[])typeof(WindowBase).GetField("window", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(han);
            var c = mSig.Convolve(window.ToDouble());
            return c;
        }

        private  double[] RepeatBits(double count, int[] bits)
        {
            var mSig = new double[bits.Length * (int)count];
            for (int i = 0; i < bits.Length; i++)
            {
                for (int x = 0; x < count; x++)
                {
                    mSig[(i * (int)count) + x] = bits[i];
                }
            }

            return mSig;
        }
    }
}
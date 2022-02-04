using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Accord.Audio;
using Accord.Math;
using NAudio.Wave;

namespace BeatGuard_Engine
{
    public class Encoder : IEncoder
    {
        private readonly SignalMixer _signalMixer;
        private readonly IFingerPrintModulator _fingerPrintModulator;
        private readonly Signal _signalSource;

        public Encoder(SignalMixer signalMixer, IFingerPrintModulator fingerPrintModulator, Signal signalSource)
        {
            _signalMixer = signalMixer;
            _fingerPrintModulator = fingerPrintModulator;
            _signalSource = signalSource;
        }

        public async Task<double[]> EncodeAsync(double[] signal, FingerPrint fingerPrint, CancellationToken cancellationToken)
        {
            return await Task.Run(async () => Encode(signal, fingerPrint, cancellationToken), cancellationToken);
        }

        private double[] Encode(double[] signal, FingerPrint fingerPrint, CancellationToken cancellationToken)
        {
            const double alpha = 0.0015;
            var mix = CreateSignal(fingerPrint, signal);
#if DEBUG
            WaveFileWriter waveFileWriter = new WaveFileWriter("watermark.wav", new WaveFormat(_signalSource.SampleRate, _signalSource.NumberOfChannels));
            waveFileWriter.WriteSamples(mix.ToFloat(), 0, (int)mix.Length);
            waveFileWriter.Close();
#endif
            cancellationToken.ThrowIfCancellationRequested();
            var result = LayerSignals(signal, mix, alpha);
            return result;
        }

        private double[] CreateSignal(FingerPrint fingerPrint, double[] signal)
        {
            var bits = _fingerPrintModulator.Modulate(fingerPrint);
            var signalLength = signal.Length;
            var bitLength = Math.Floor((double)(signalLength / bits.Length));
            return _signalMixer.Mix(bitLength, bits);
        }

        private double[] LayerSignals(double[] signal, double[] mix, double alpha)
        {
            //var j = 0;
            for (var i = 0; i < mix.Length; i++)
            {
                signal[i] += (alpha * mix[i]);
                //j += numberOfChannels;
            }

            return signal;
        }

        public float[] Dss_Encoding(Signal signal, string text, int l_min = 8192)
        {
            int[] numArray; 
            var length = signal.Length;
            var numberOfChannels = signal.NumberOfChannels;
//            var newBits = "";
//            foreach (var c in text)
//            {
//                newBits = newBits + c + c + c;
//            }
//            Console.WriteLine(newBits);
            var bits = ArrayExtension.GetBits(text);
            var num = Math.Floor((double)(length / (int)bits.Length));
            var num1 = Math.Max(num, (double)l_min);
            var num2 = Math.Floor((double)length / num1);
            var num3 = num2 - num2 % 8;
            Console.WriteLine("- Creating carrier signal");
            if ((double)((int)bits.Length) <= num3)
            {
                numArray = new int[(int)num3];
                numArray.Clear();
                bits.CopyTo(numArray, 0);
            }
            else
            {
                Debug.WriteLine("Message is too long.");
                numArray = new int[(int)num3];
                bits.CopyTo(numArray, 0);
            }
            var numArray1 = new int[(int)num3 * (int)num1];
            var num4 = 0.005;
            Console.WriteLine(num1);
            var numArray2 = _signalMixer.Mix(num1, numArray, -1, 1, 256);
#if DEBUG
            WaveFileWriter FileWriter = new WaveFileWriter("watermark.wav", new WaveFormat(_signalSource.SampleRate, _signalSource.NumberOfChannels));
            FileWriter.WriteSamples(numArray2.ToFloat(), 0, (int)numArray2.Length);
            FileWriter.Close();
#endif
            var numArray3 = signal.ToDouble();
            var numberOfChannels1 = 0;
            for (int i = 0; i < (int)numArray2.Length; i++)
            {
                ref var numPointer = ref numArray3[numberOfChannels1];
                numPointer = numPointer + num4 * numArray2[i];
                numberOfChannels1 += signal.NumberOfChannels;
            }
            Console.WriteLine("- Modulating carrier to raw source.");
            WaveFileWriter waveFileWriter = new WaveFileWriter("output_watermarked.wav", new WaveFormat(signal.SampleRate, signal.NumberOfChannels));
            waveFileWriter.WriteSamples(numArray3.ToFloat(), 0, (int)numArray3.Length);
            waveFileWriter.Close();
            Console.WriteLine("- Watermarking completed! output_watermarked.wav");
            var signal1 = new Signal(numArray3, signal.NumberOfChannels, (int)numArray3.Length, signal.SampleRate, signal.SampleFormat);
            return null;
        }
    }
}
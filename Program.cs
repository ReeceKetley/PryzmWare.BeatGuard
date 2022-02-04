using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;
using Accord;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.Audio.Generators;
using Accord.Diagnostics;
using Accord.DirectSound;
using Accord.IO;
using Accord.Statistics.Distributions.Multivariate;
using Accord.Statistics.Kernels;
using BeatGuard_Test;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Debug = System.Diagnostics.Debug;

namespace BeatGuard_Engine
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var fileDecoder = new Accord.DirectSound.WaveFileAudioSource(@"carrier.wav");
            //var mainForm = new MainForm();
            //mainForm.ShowDialog();
            Console.WriteLine("Type in an option: 1 - Encode 2 - Decode");
            if(Console.ReadLine() == "1")
            {

                var mixer = new SignalMixer();
                var fingerPrintModulator = new FingerPrintModulator();
                var openFile = new OpenFileDialog();
                openFile.DefaultExt = ".wav";
                openFile.ShowDialog();
                //Pre_Process(openFile.FileName);
                var file = Signal.FromFile(@"pre_processed.wav");
                var encoder = new Encoder(mixer, fingerPrintModulator, file);
                var newGuid = Guid.NewGuid();
                Console.WriteLine("- Selected ID: " + newGuid.ToString());
                encoder.Dss_Encoding(file, newGuid.ToString());
                //            var fingerPrint = new FingerPrint(newGuid);
                //            var outFile = encoder.EncodeAsync(file.ToDouble(), fingerPrint, new CancellationToken()).Result;
                //            WaveFileWriter waveFileWriter = new WaveFileWriter("output_watermarked.wav", new WaveFormat(file.SampleRate, file.NumberOfChannels));
                //            waveFileWriter.WriteSamples(outFile.ToFloat(), 0, (int)outFile.Length);
                //            waveFileWriter.Close();

                var fromFile = Signal.FromFile("output_watermarked.wav");
                var lastString = "";
                var match = false;
                Console.WriteLine("- Checking watermarked file.\r\n");
                Console.WriteLine("#################################");
                for (int i = 16; i < 64; i++)
                {
                    var dssDecode = Decoder.Dss_Decode(fromFile, i);
                    if (dssDecode == newGuid.ToString())
                    {
                        lastString = dssDecode;
                        Console.WriteLine("#" + i + " " + dssDecode);
                        match = true;
                        break;
                    }

                    Console.WriteLine("#" + i + " " + dssDecode);
                }

                Console.WriteLine("#################################");
                Console.WriteLine();
                if (match)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("- Matching ID located: " + lastString);
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("- Comparing source file to watermarked file and calculating differances.");
                Process.Start("eaqual.exe", "-fref pre_processed.wav -ftest output_watermarked.wav");
                Console.ReadLine();
            } else 
            {
                Console.WriteLine("Enter expected watermark id: ");
                var match = Console.ReadLine();
                Decode(match);
            }

        }

        public static void Pre_Process(string inFile)
        {
            var outPath = @"pre_processed.wav";

            using (var reader = new AudioFileReader(inFile))
            {
                reader.Position = 0;
                reader.Volume = 0.99f;
                WaveFileWriter.CreateWaveFile16(outPath, reader);
            }
        }

        public static void Decode(string expectedString)
        {
            var openFile = new OpenFileDialog();
            openFile.DefaultExt = ".wav";
            openFile.ShowDialog();
            var fromFile = Signal.FromFile(openFile.FileName);
            var lastString = "";
            var match = false;
            Console.WriteLine("- Checking watermarked file.\r\n");
            Console.WriteLine("#################################");
            for (int i = 16; i < 64; i++)
            {
                var dssDecode = Decoder.Dss_Decode(fromFile, i);
                if (dssDecode == expectedString)
                {
                    lastString = dssDecode;
                    Console.WriteLine("#" + i + " " + dssDecode);
                    match = true;
                    break;
                }
                Console.WriteLine("#" + i + " " + dssDecode);
            }

            Console.ReadLine();
        }
    }
}

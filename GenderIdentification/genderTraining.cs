using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Numerics;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace GenderIdentification
{
    public class genderTraining
    {
        /// <summary>
        /// produce gamma and write maximum frequency to csv file
        /// </summary>
        public void machineLearning()
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine("                           Gender Training Program");
            Console.WriteLine("");
            Console.WriteLine("================================================================================");

            // Preparing wave file
            Console.WriteLine("preparing wave file...");
            Console.WriteLine("");
            getAudioData getWave = new getAudioData();
            Console.WriteLine("wave file is ready.");
            Console.WriteLine("");
            Console.WriteLine("================================================================================");
            long totalFreqFe = 0;
            int totalNFe = 0;
            string csvPath = Environment.CurrentDirectory + "/frequency.csv";
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("female,"));
            int nf = 700;
            Console.WriteLine("processing female frequency...");
            for (int i = 1; i <= 5; i++)
            {
                string filename = Environment.CurrentDirectory + @"/female/" + i + "_female.wav";
                double[] x = getWave.WaveReady(filename);
                FFT hitungFft = new FFT();
                Complex[] X = hitungFft.fft(x, nf, 300);
                getMaxFreq maximumFrequency = new getMaxFreq();
                int max = maximumFrequency.MaxFreq(X);
                Console.WriteLine("Maximum Frequency of number " + i + " is " + max + " Hz");
                sb.Append(string.Format("{0},", max.ToString()));
                totalFreqFe = totalFreqFe + max;
                totalNFe++;
            }
            sb.Append(string.Format("{0}", Environment.NewLine));
            double feAvg = totalFreqFe / totalNFe;
            long totalFreqMa = 0;
            int totalNMa = 0;
            sb.Append(string.Format("male,"));
            Console.WriteLine("");
            Console.WriteLine("processing male frequency...");
            for (int i = 1; i <= 5; i++)
            {
                string filename = Environment.CurrentDirectory + @"/male/" + i + "_male.wav";
                double[] x = getWave.WaveReady(filename);
                FFT hitungFft = new FFT();
                Complex[] X = hitungFft.fft(x, nf, 300);
                getMaxFreq maximumFrequency = new getMaxFreq();
                int max = maximumFrequency.MaxFreq(X);
                Console.WriteLine("Maximum Frequency of number " + i + " is " + max + " Hz");
                sb.Append(string.Format("{0},", max.ToString()));
                totalFreqMa = totalFreqMa + max;
                totalNMa++;
            }
            double maAvg = totalFreqMa / totalNMa;
            double gamma = (feAvg + maAvg) / 2;
            Console.WriteLine("");
            Console.WriteLine("Treshold value: " + gamma);
            string txtPath = Environment.CurrentDirectory + "/gamma.txt";
            File.WriteAllText(txtPath, gamma.ToString());
            File.WriteAllText(csvPath, sb.ToString());
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }
    }
}

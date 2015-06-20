using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace GenderIdentification
{
    class genderIdentification
    {
        /// <summary>
        /// Global variable
        /// </summary>
        IModel channelSendGender;
        string channelKeySendGender;
        double[] x;

        public genderIdentification(double[] input)
        {
            x = input;
            channelSendGender = Program.connect.channelSendGender;
            channelKeySendGender = Program.connect.channelKeySendGender;
        }

        /// <summary>
        /// decide if the voice is man's or woman's
        /// </summary>
        public void identification()
        {
            string filename = Environment.CurrentDirectory + "/gamma.txt";
            string gamma = File.ReadAllText(filename);
            double gamma1 = Convert.ToDouble(gamma);
            int nf = 700;
            Console.WriteLine("Start FFT...");
            FFT hitungFft = new FFT();
            Console.WriteLine("waiting for process...");
            Complex[] X = hitungFft.fft(x, nf, 300);
            getMaxFreq maximumFrequency = new getMaxFreq();
            int max = maximumFrequency.MaxFreq(X);
            Console.WriteLine("");
            Console.WriteLine("Maximum frequency is " + max + " Hz");
            if ((double)max < gamma1)
            {
                Console.Write("Result = ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("male voice");
                Console.ResetColor();
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("=====================================");
                Console.WriteLine("");
                genderResult hasil = new genderResult { name = "GenderIdentification", gender = "male" };
                string body = JsonConvert.SerializeObject(hasil); //serialisasi data menjadi string
                byte[] a = Encoding.UTF8.GetBytes(body);
                channelSendGender.BasicPublish("amq.topic", channelKeySendGender, null, a); //mengirim data ke RabbitMQ
            }
            else
            {
                Console.Write("Result = ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("female voice");
                Console.ResetColor();
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("=====================================");
                Console.WriteLine("");
                genderResult hasil = new genderResult { name = "GenderIdentification", gender = "female" };
                string body = JsonConvert.SerializeObject(hasil); //serialisasi data menjadi string
                byte[] a = Encoding.UTF8.GetBytes(body);
                channelSendGender.BasicPublish("amq.topic", channelKeySendGender, null, a); //mengirim data ke RabbitMQ
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio.Wave;
using NAudio;
using NAudio.WindowsMediaFormat;
using System.IO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace GenderIdentification
{
    public class getAudioData
    {
        genderIdentification identification;

        public double[] WaveReady(string filename)
        {
            WaveFileReader reader = new WaveFileReader(filename);
            int n = (int)reader.SampleCount;
            byte[] buffer = new byte[n * 2];
            int byteread = reader.Read(buffer, 0, 2 * (int)reader.SampleCount);
            for (int i = 2 * (int)reader.SampleCount; i < n * 2; i++)
            {
                buffer[i] = 0;
            }
            short[] shortdata = new short[n];
            double[] x = new double[n];
            int sample = 0;
            for (int index = 0; index < n; index++)
            {
                shortdata[index] = BitConverter.ToInt16(buffer, sample);
                x[index] = (double)shortdata[index];
                sample += 2;
            }
            return x;
        }

        public void startGetWave()
        {
            Thread wave = new Thread(fileStream);
            wave.Start();
        }

        public void fileStream()
        {
            BasicDeliverEventArgs ev;
            while (true)
            {
                if (Program.connect.subscriptionWav.Next(0, out ev))
                {
                    Console.WriteLine("File is ready.");
                    string body = Encoding.UTF8.GetString(ev.Body);
                    JsonSerializerSettings json = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
                    sound s = JsonConvert.DeserializeObject<sound>(body, json);
                    byte[] data = Convert.FromBase64String(s.content);
                    Stream audio = new MemoryStream(data);
                    WaveFileReader reader = new WaveFileReader(audio);
                    int n = (int)reader.SampleCount;
                    byte[] buffer = new byte[n * 2];
                    int byteread = reader.Read(buffer, 0, 2 * (int)reader.SampleCount);
                    for (int i = 2 * (int)reader.SampleCount; i < n * 2; i++)
                    {
                        buffer[i] = 0;
                    }
                    short[] shortdata = new short[n];
                    double[] x = new double[n];
                    int sample = 0;
                    for (int index = 0; index < n; index++)
                    {
                        shortdata[index] = BitConverter.ToInt16(buffer, sample);
                        x[index] = (double)shortdata[index];
                        sample += 2;
                    }
                    identification = new genderIdentification(x);
                    identification.identification();
                }
            }
        }
    }
}

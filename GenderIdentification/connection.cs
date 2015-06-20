using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace GenderIdentification
{
    public class connection
    {
        #region Field
        public IModel channelSendGender, channelGetWave;
        public string channelKeySendGender, channelKeyGetWave;
        public Subscription subscriptionWav;
        #endregion

        public connection()
        {
            channelKeySendGender = "lumen.audio.gender.identification"; // send gender data to RabbitMQ, used for gender identification
            channelKeyGetWave = "lumen.audio.get.wave";// get wave data from RabbitMQ, used for gender identification
        }

        public void connectTo()
        {
            Console.WriteLine("Start...");
            Console.WriteLine("");
            ConnectionFactory factory = new ConnectionFactory(); // establish connection
            factory.Uri = "amqp://lumen:lumen@169.254.18.223/%2F"; // ip syarif
            try
            {
                IConnection connection = factory.CreateConnection();
                
                channelGetWave = connection.CreateModel();
                QueueDeclareOk queueWav = channelGetWave.QueueDeclare("", true, false, true, null);
                channelGetWave.QueueBind(queueWav.QueueName, "amq.topic", channelKeyGetWave);
                subscriptionWav = new Subscription(channelGetWave, queueWav.QueueName);

                channelSendGender = connection.CreateModel();
                connection.AutoClose = true;

                Console.WriteLine("Connected to Server.");
                Console.WriteLine("");
                Console.WriteLine("=================================================");
                Console.WriteLine("");
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

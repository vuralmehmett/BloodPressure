using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using CommonQueueManager.Interface;
using CommonQueueManager.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace CommonQueueManager.QueueManager
{
    public class RabbitManager : IQueueManager
    {
        private static readonly string TopicName = ConfigurationManager.AppSettings["RabbitMqTopicName"];
        
        public RabbitManager()
        {
            var conn = QueueConnectionFactory.CreateConnection(Thread.CurrentThread.ManagedThreadId);
            QueueConnectionFactory.CreateChannel(Thread.CurrentThread.ManagedThreadId, conn);
        }

        public bool PutData(string data)
        {
            var channel = QueueConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            channel.QueueDeclare(queue: TopicName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(data);
            channel.BasicPublish(exchange: "",
                routingKey: TopicName,
                basicProperties: null,
                body: body);

            return true;
        }

        public BloodPressureModel PullData(int patientNo)
        {
            BloodPressureModel _patientModel = new BloodPressureModel();

            var channel = QueueConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);

            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(TopicName, true, consumer);

            Console.WriteLine(" [*] Processing existing messages.");

            for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(message);
                if (serializedJson.ClientNo == patientNo)
                {
                    _patientModel = serializedJson;
                }
                Console.WriteLine(" [x] Received {0}", message);
            }

            return _patientModel;
        }

        public List<string> GetAllMessage()
        {
            List<string> MessageList = new List<string>();

            var channel = QueueConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);
            var consumer = new QueueingBasicConsumer(channel);

            try
            {
                channel.BasicConsume(TopicName, true, consumer);

                Console.WriteLine(" [*] Processing existing messages.");

                for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
                {
                    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    MessageList.Add(message);
                    Console.WriteLine(" [x] Received {0}", message);
                }

            }
            catch (Exception)
            {
                var response = channel.BasicGet(TopicName, false);
                channel.BasicNack(response.DeliveryTag, true, true);
                throw;
            }

            return MessageList;
        }

        public List<string> GetSpecificMessage(ushort messageCount)
        {
            List<string> MessageList = new List<string>();

            var channel = QueueConnectionFactory.GetChannelPerThreadId(Thread.CurrentThread.ManagedThreadId);

            var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);

            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(TopicName, true, consumer);
            channel.BasicQos(0, messageCount, true);
            Console.WriteLine(" [*] Processing existing messages.");
                
            // TODO : aşağıda ki queueDeclareResponse.MessageCount degeri yerine istediğimiz mesaj sayısını koyduğumuzda çalışıyor ancak bu bence kötü yontem.
            for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                MessageList.Add(message);
                Console.WriteLine(" [x] Received {0}", message);
            }

            return MessageList;
        }
    }
}

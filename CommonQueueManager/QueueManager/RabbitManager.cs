using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using CommonQueueManager.Interface;
using CommonQueueManager.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommonQueueManager.QueueManager
{
    public class RabbitManager : IQueueManager
    {
        private static readonly Connector RabbitMqConnection = new Connector();
        private static readonly ConnectionFactory Factory = RabbitMqConnection.RabbitMqConnection();
        private static readonly string TopicName = ConfigurationManager.AppSettings["TopicName"];

        public bool PutData(string data)
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
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
            }

            return true;
        }

        public BloodPressureModel PullData(int patientNo)
        {
            BloodPressureModel _patientModel = new BloodPressureModel();

            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
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
        }

        public List<string> GetAllMessage()
        {
            List<string> MessageList = new List<string>();

            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
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
        }

        public List<string> GetSpecificMessage(ushort messageCount)
        {
            List<string> MessageList = new List<string>();

            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
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
}

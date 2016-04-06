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
        private static readonly List<string> MessageList = new List<string>();
        private static readonly Connector RabbitMqConnection = new Connector();
        private static readonly ConnectionFactory Factory = RabbitMqConnection.RabbitMqConnection();
        private static readonly string TopicName = ConfigurationManager.AppSettings["TopicName"];
        private BloodPressureModel _patientModel = new BloodPressureModel();

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
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, true, null);

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
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);
                var consumer = new QueueingBasicConsumer(channel);
                try //TODO : Gokhana sor !
                {
                    channel.TxSelect();
                    channel.BasicConsume(TopicName, false, consumer);

                    Console.WriteLine(" [*] Processing existing messages.");

                    for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        MessageList.Add(message);
                        Console.WriteLine(" [x] Received {0}", message);
                    }
                    channel.TxRollback();
                }
                catch (Exception)
                {
                   channel.TxRollback();
                }
                
                return MessageList;
            }
        }

        public List<string> GetSpecificMessage(ushort messageCount)
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);

                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(TopicName, false, consumer);
                channel.BasicQos(0, messageCount, true);
                Console.WriteLine(" [*] Processing existing messages.");

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

using System;
using System.Collections.Generic;
using System.Text;
using BloodPressureWebApi.Abstract;
using BloodPressureWebApi.Common;
using BloodPressureWebApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BloodPressureWebApi.Queues
{
    public class RabbitMq : AbstractQueue
    {
        private static readonly List<string> MessageList = new List<string>();
        public static Connector RabbitMqConnection = new Connector();
        public static ConnectionFactory Factory = RabbitMqConnection.RabbitMqConnection();
        public static string TopicName = System.Configuration.ConfigurationManager.AppSettings["TopicName"];

        public override bool SendMessage(BloodPressureModel model)
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: TopicName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                var serializedJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(serializedJson);

                channel.BasicPublish(exchange: "",
                    routingKey: TopicName,
                    basicProperties: null,
                    body: body);

            }

            return true;
        }

        public override List<string> GetMessage()
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);

                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(TopicName, false, consumer); // todo : mesajı aldıktan sonra kuyruktan silen parametre burada ki "false" olan. False olarak bırakıyorum çünkü mesajı kuyruktan alıp mongoya yazıyorum. Doktor tarafında silicem.

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
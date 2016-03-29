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

                for (int i = 0; i < 100; i++)
                {
                    var serializedJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var body = Encoding.UTF8.GetBytes(serializedJson.ToString());

                    channel.BasicPublish(exchange: "",
                        routingKey: TopicName,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent {0} : {1} , Thread{2}", body, i,
                        System.Threading.Thread.CurrentThread.ManagedThreadId);
                }

                //TODO : Test for performance
                // Parallel.For(2, 5000, i =>
                // {
                //     string message = i + "Test messages";
                //     var body = Encoding.UTF8.GetBytes(message);
                //
                //     channel.BasicPublish(exchange: "",
                //         routingKey: "test",
                //         basicProperties: null,
                //         body: body);
                //     Console.WriteLine(" [x] Sent {0} : {1} , Thread{2}", message, i, System.Threading.Thread.CurrentThread.ManagedThreadId);
                //
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
                return MessageList;
            }
        }
      
    }
}
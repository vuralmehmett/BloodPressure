using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using CommonQueueManager.QueueManager;
using MongoDB.Driver.Core.WireProtocol.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TestRabbitMq.RabbitMq
{
    public class Receive
    {
        //private static List<string> MessageList = new List<string>();
        //private static readonly string HostName = ConfigurationManager.AppSettings["RabbitMqHostName"];
        //private static readonly string UserName = ConfigurationManager.AppSettings["RabbitMqUserName"];
        //private static readonly string Password = ConfigurationManager.AppSettings["RabbitMqPassword"];
        //private static readonly string VirtualHost = ConfigurationManager.AppSettings["RabbitMqVirtualHost"];
        //private static readonly string Port = ConfigurationManager.AppSettings["RabbitMqPort"];
        //private static readonly string TopicName = ConfigurationManager.AppSettings["RabbitMqTopicName"];
        //private static readonly RabbitManager Manager = new RabbitManager();

        //public void ReceiveMessage()
        //{
        //    Mongo dbMongo = new Mongo();
        //    var factory = new ConnectionFactory
        //    {
        //        HostName = HostName,
        //        UserName = UserName,
        //        Password = Password,
        //        VirtualHost = VirtualHost,
        //        Port = Convert.ToInt32(Port)
        //    };

        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {

        //        channel.QueueDeclare(queue: TopicName,
        //                             durable: false,
        //                             exclusive: false,
        //                             autoDelete: false,
        //                             arguments: null);

        //        var consumer = new EventingBasicConsumer(channel);
        //        channel.BasicQos(0, 5, true); // count of messages
        //        consumer.Received += (model, ea) =>
        //        {
        //            var body = ea.Body;
        //            var message = Encoding.UTF8.GetString(body);
        //            // dbMongo.MongoInsert(message);
        //            Console.WriteLine(" [x] Received {0}", message);
        //        };

        //        channel.BasicConsume(queue: TopicName,
        //                             noAck: false,
        //                             consumer: consumer);

        //        Console.WriteLine(" Press [enter] to exit.");
        //        Console.ReadLine();
        //    }
        //}

        //private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //public string Get()
        //{
        //    var Factory = new ConnectionFactory
        //    {
        //        HostName = "bloodpressure-rabbitmq",
        //        UserName = "guest",
        //        Password = "guest",
        //        VirtualHost = "/",
        //        Port = 5672
        //    };


        //    using (var connection = Factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        ushort messageCount = 5;
        //        var queueDeclareResponse = channel.QueueDeclare(TopicName, false, false, false, null);
        //        var messageNumber = queueDeclareResponse.MessageCount - messageCount;
        //        //channel.BasicQos(0,10,true);
        //        // channel.BasicQos(0, (ushort)messageNumber, false);
        //        var consumer = new QueueingBasicConsumer(channel);
        //        channel.BasicConsume(TopicName, false, consumer);

        //        var queueDeclareResponse1 = channel.QueueDeclare(TopicName, false, false, false, null);

        //        Console.WriteLine(" [*] Processing existing messages.");

        //        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

        //        for (int i = 0; i < queueDeclareResponse.MessageCount; i++)
        //        {
        //            var body = ea.Body;
        //            var message = Encoding.UTF8.GetString(body);
        //            MessageList.Add(message);
        //            Console.WriteLine(i + "[x] Received {0}", message);
        //        }
        //    }

        //    var asd = MessageList;
        //    return "";
        //}

        public List<string> GetOneByOneMessage(RabbitManager manager, int messageCount, int partitionId)
        {
            var messageList = manager.GetSpecificMessage(messageCount);

            for (int i = 0; i < messageList.Count; i++)
            {
                Console.WriteLine(" [x] Sent {0} : task id: {1} ---- partition Id: {2}", messageList[i],
                        System.Threading.Thread.CurrentThread.ManagedThreadId, partitionId);
            }

            return messageList;
        }


    }
}

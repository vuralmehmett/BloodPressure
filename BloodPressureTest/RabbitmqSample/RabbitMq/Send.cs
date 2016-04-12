using System;
using System.Text;
using RabbitMQ.Client;

namespace TestRabbitMq.RabbitMq
{
    public class Send
    {
        public void SendMessage(int taskId)
        {
            var factory = new ConnectionFactory
            {
                HostName = "bloodpressure-rabbitmq",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "test",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                string message = "Test messages";
                var body = Encoding.UTF8.GetBytes(message);



                for (int i = 0; i < 10000; i++)
                {
                    channel.BasicPublish(exchange: "",
                   routingKey: "test",
                   basicProperties: null,
                   body: body);
                    Console.WriteLine(" [x] Sent {0} : {1} , Thread{2}, ---- task Id{3}", message,i,
                        System.Threading.Thread.CurrentThread.ManagedThreadId, taskId);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

    }
}

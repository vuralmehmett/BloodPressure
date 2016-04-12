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

            while (true)
            {
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
                    
                    channel.BasicPublish(exchange: "",
                            routingKey: "test",
                            basicProperties: null,
                            body: body);
                    Console.WriteLine(" [x] Sent {0} : {1} ---- task Id: {2}", message,
                        System.Threading.Thread.CurrentThread.ManagedThreadId, taskId);
                
                }
            }
        }

    }
}

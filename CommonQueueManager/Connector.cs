using System;
using System.Configuration;
using RabbitMQ.Client;

namespace CommonQueueManager
{
    public class Connector
    {
        public ConnectionFactory RabbitMqConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["RabbitMqHostName"],
                UserName = ConfigurationManager.AppSettings["RabbitMqUserName"],
                Password = ConfigurationManager.AppSettings["RabbitMqPassword"],
                VirtualHost = ConfigurationManager.AppSettings["RabbitMqVirtualHost"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMqPort"])
            };

            return factory;
        }
    }
}

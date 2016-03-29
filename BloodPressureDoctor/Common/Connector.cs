using System;
using System.Configuration;
using RabbitMQ.Client;

namespace BloodPressureDoctor.Common
{
    public class Connector
    {
        public ConnectionFactory RabbitMqConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["HostName"],
                UserName = ConfigurationManager.AppSettings["UserName"],
                Password = ConfigurationManager.AppSettings["Password"],
                VirtualHost = ConfigurationManager.AppSettings["VirtualHost"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"])
            };

            return factory;
        }
    }
}

using System;
using RabbitMQ.Client;

namespace BloodPressureWebApi.Common
{
    public class Connector
    {
        public ConnectionFactory RabbitMqConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = System.Configuration.ConfigurationManager.AppSettings["HostName"],
                UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"],
                Password = System.Configuration.ConfigurationManager.AppSettings["Password"],
                VirtualHost = System.Configuration.ConfigurationManager.AppSettings["VirtualHost"],
                Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"])
            };

            return factory;
        }
    }
}
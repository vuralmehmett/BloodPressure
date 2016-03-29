using System;
using System.Configuration;
using RabbitMQ.Client;

namespace BloodPressureDoctor.Common
{
    public class Connector
    {
        public ConnectionFactory RabbitMqConnection()
        {
            string aaa = ConfigurationManager.AppSettings["HostName"];
            string aaa1 = ConfigurationManager.AppSettings["UserName"];
            string aaa2 = ConfigurationManager.AppSettings["Password"];
            string aaa3 = ConfigurationManager.AppSettings["VirtualHost"];
            string aaa4 = ConfigurationManager.AppSettings["Port"];

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

using System;
using System.Configuration;
using System.Text;
using BloodPressureDoctor.Abstract;
using BloodPressureDoctor.Common;
using BloodPressureDoctor.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BloodPressureDoctor.Queues
{
    public class RabbitMq : AbstractQueue
    {
        public static Connector RabbitMqConnection = new Connector();
        public static ConnectionFactory Factory = RabbitMqConnection.RabbitMqConnection();
        public static string TopicName = ConfigurationManager.AppSettings["TopicName"];
        BloodPressureModel _patientModel = new BloodPressureModel();


        public override BloodPressureModel GetMessage(int patientNo)
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

    }
}

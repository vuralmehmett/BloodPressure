using System.Text;
using BloodPressureWebApi.Kafka;
using BloodPressureWebApi.Models;

namespace BloodPressureWebApi.Bussiness.Kafka
{
    public class SendMessageToTopic
    {
        public bool SendMessage(string topicName , BloodPressureModel model)
        {
            const string serverAddress = "192.168.86.128";
            const int serverPort = 9092;
            const int correlationId = 0;
            const int partitionId = 0;


            var connector = new Connector(serverAddress, serverPort);
            var serializedJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);

           var kafkaMessage1 = Encoding.ASCII.GetBytes(serializedJson);
           connector.Produce(correlationId, "c#", 5000000, topicName, partitionId, kafkaMessage1, model.ClientNo);



            for (int i = 0; i < 2000; i++)
            {
                var kafkaMessage = Encoding.ASCII.GetBytes(serializedJson);

                connector.Produce(correlationId, "c#", 5000000, topicName, partitionId, kafkaMessage, model.ClientNo = i);
            }

            //Stopwatch time = new Stopwatch();
            //time.Start();

            //Parallel.For(2, 1000, i =>
            //{
            //    var kafkaMessage = Encoding.ASCII.GetBytes(serializedJson);

            //    connector.Produce(correlationId, "c#", 5000000, topicName, partitionId, kafkaMessage, model.ClientNo = i);
            //});
            //time.Stop();

            //return time.ElapsedMilliseconds.ToString();

             return true;
        }

    }
}
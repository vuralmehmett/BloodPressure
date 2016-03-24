using System;
using BloodPressureWebApi.Kafka;

namespace BloodPressureWebApi.Bussiness.Kafka
{
    public class CreateKafkaTopic
    {
        public bool CreateTopic(string topicName)
        {
            const int correlationId = 0;
            const string serverAddress = "192.168.86.128";
            const int serverPort = 9092;

            var connector = new Connector(serverAddress, serverPort);
            var metadataResponse = connector.Metadata(correlationId, "C# KafkaMetadata util", topicName);
            foreach (var broker in metadataResponse.Brokers)
            {
                Console.WriteLine("\t" + broker);
            }

            return true;

        }
    }
}
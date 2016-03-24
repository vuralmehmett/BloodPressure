using System;
using System.Collections.Generic;
using System.Linq;
using BloodPressureWebApi.Kafka;
using BloodPressureWebApi.KafkaRequest;
using BloodPressureWebApi.KafkaResponse;

namespace BloodPressureWebApi.Bussiness.Kafka
{
    public class GetMessageFromKafka
    {
        private static readonly List<string> MessageList = new List<string>();

        public List<string> GetMessage(string topicName)
        {

            const string serverAddress = "192.168.86.128";
            const int serverPort = 9092;
            const int partitionId = 0;
            const int correlationId = 0;
            const int max = 2;

            Connector connector = new Connector(serverAddress, serverPort);

            OffsetResponse offsetResponse = connector.GetOffsetResponse(topicName, OffsetRequest.LatestTime, max,
                correlationId, "C#", partitionId);

            foreach(var offsetTopicName in offsetResponse.Topics())
            {
                foreach (var partition in offsetResponse.Partitions(offsetTopicName))
                {
                    var dataLength = offsetResponse.Offsets(offsetTopicName, partition)[0] * 45.45;
                    FetchResponse fetchResponse = connector.Fetch(offsetTopicName, partition, correlationId, "C#", offsetResponse.Offsets(offsetTopicName, partition)[1], 50000);

                    foreach (var fetchTopic in fetchResponse.Topics)
                    {
                        foreach (FetchPartitionResponse fetchPartition in fetchTopic.Partitions)
                        {
                            foreach (var messageSet in fetchPartition.MessageSets)
                            {
                                if (messageSet != null)
                                {
                                    MessageList.Add(messageSet.Message.ToString());
                                }
                            }
                        }
                    }
                }
            }

            return MessageList;

        }
    }
}
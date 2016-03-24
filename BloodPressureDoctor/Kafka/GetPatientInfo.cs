using BloodPressureDoctor.Model;

namespace BloodPressureDoctor.Kafka
{
    public class GetPatientInfo
    {
        public BloodPressureModel GetInfo(string clientNo)
        {
            var model = new BloodPressureModel();
            const string serverAddress = "192.168.86.128";
            const int serverPort = 9092;
            const int partitionId = 0;
            const int correlationId = 0;
            const int max = 100;
            const string topicName = "testforNet";

            Connector connector = new Connector(serverAddress, serverPort);

            OffsetResponse offsetResponse = connector.GetOffsetResponse(topicName, OffsetRequest.LatestTime, max,
               correlationId, "C#", partitionId);

            foreach (var offsetTopicName in offsetResponse.Topics())
            {
                foreach (var partition in offsetResponse.Partitions(offsetTopicName))
                {
                    FetchResponse fetchResponse = connector.Fetch(offsetTopicName, partition, correlationId, "C#", offsetResponse.Offsets(offsetTopicName, partition)[1], 50000);

                    foreach (var fetchTopic in fetchResponse.Topics)
                    {
                        foreach (FetchPartitionResponse fetchPartition in fetchTopic.Partitions)
                        {
                            foreach (var messageSet in fetchPartition.MessageSets)
                            {
                                if (messageSet.Message.Key.ToString() == clientNo)
                                {
                                    var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(messageSet.Message.ToString());
                                    model = serializedJson;
                                }
                            }
                        }
                    }
                }
            }

            return model;
        }
    }
}

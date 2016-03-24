using System.Collections.Generic;

namespace BloodPressureDoctor.Model
{
    public class ProduceTopic : IRequestBuffer
    {
        private readonly Topic topic;


        public ProduceTopic(string topicName)
        {
            topic = new Topic(topicName);
        }

        public ProducePartitionRequest AddPartition(int partitionId)
        {
            var partition = new ProducePartitionRequest(partitionId);
            topic.Partitions.Add(partition);
            return partition;
        }

        public List<byte> GetRequestBytes()
        {
            var request = new List<byte>();
            request.AddRange(topic.GetRequestBytes());
            return request;
        }
    }
}

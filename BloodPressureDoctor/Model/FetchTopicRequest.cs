using System;
using System.Collections.Generic;

namespace BloodPressureDoctor.Model
{
    public class FetchTopicRequest : IRequestBuffer
    {
        private readonly Topic topic;

        public FetchTopicRequest(String topicName)
        {
            topic = new Topic(topicName);
        }

        public FetchPartitionRequest AddPartition(int partitionId, long fetchOffset, int maxBytes)
        {
            var partition = new FetchPartitionRequest(partitionId, fetchOffset, maxBytes);
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

using System;
using System.Collections.Generic;
using BloodPressureWebApi.KafkaRequest.Interface;

namespace BloodPressureWebApi.KafkaRequest
{
    public class OffsetTopic : IRequestBuffer
    {
        private readonly Topic topic;

        public OffsetTopic(String topicName)
        {
            topic = new Topic(topicName);
        }

        public OffsetPartitionRequest AddPartition(int partitionId, long time, int maxNumberOfOffsets)
        {
            var partition = new OffsetPartitionRequest(partitionId, time, maxNumberOfOffsets);
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
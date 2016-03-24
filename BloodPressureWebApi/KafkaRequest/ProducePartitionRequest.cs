using System.Collections.Generic;
using BloodPressureWebApi.Common;
using BloodPressureWebApi.KafkaRequest.Interface;

namespace BloodPressureWebApi.KafkaRequest
{
    public class ProducePartitionRequest : IRequestBuffer
    {
        private int partitionId;
        private int messageSetSize;
        private MessageSet messageSet;


        public ProducePartitionRequest(int partitionId, MessageSet messageSet)
        {
            this.partitionId = partitionId;
            this.messageSet = messageSet;
        }

        public ProducePartitionRequest(int partitionId)
        {
            this.partitionId = partitionId;
        }

        public void AddMessageSet(MessageSet set)
        {
            messageSet = set;
        }

        public List<byte> GetRequestBytes()
        {
            var requestBytes = new List<byte>();
            requestBytes.AddRange(BitWorks.GetBytesReversed(partitionId));

            var messageSetBuffer = new List<byte>();
            messageSetBuffer.AddRange(messageSet.GetRequestBytes());
            requestBytes.AddRange(BitWorks.GetBytesReversed(messageSetBuffer.Count));
            requestBytes.AddRange(messageSetBuffer);
            return requestBytes;
        }
    }
}
using System.Collections.Generic;
using BloodPressureWebApi.Common;
using BloodPressureWebApi.KafkaRequest.Interface;

namespace BloodPressureWebApi.KafkaRequest
{
    public class FetchPartitionRequest : IRequestBuffer
    {
        private readonly Partition partition;

        public long FetchOffset { get; set; }
        public int MaxBytes { get; set; }

        public FetchPartitionRequest(int partitionId, long fetchOffset, int maxBytes)
        {
            partition = new Partition(partitionId);
            FetchOffset = fetchOffset;
            MaxBytes = maxBytes;
        }

        public List<byte> GetRequestBytes()
        {
            var request = new List<byte>();
            request.AddRange(partition.GetRequestBytes());
            request.AddRange(BitWorks.GetBytesReversed(FetchOffset));
            request.AddRange(BitWorks.GetBytesReversed(MaxBytes));
            return request;
        }
    }
}
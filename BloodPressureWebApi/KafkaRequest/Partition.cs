using System.Collections.Generic;
using BloodPressureWebApi.Common;
using BloodPressureWebApi.KafkaRequest.Interface;

namespace BloodPressureWebApi.KafkaRequest
{
    public class Partition :IRequestBuffer
    {
        private readonly int partitionId;

        public Partition(int partitionId)
        {
            this.partitionId = partitionId;
        }

        public List<byte> GetRequestBytes()
        {
            var request = new List<byte>();
            request.AddRange(BitWorks.GetBytesReversed(partitionId));
            return request;
        }
    }
}
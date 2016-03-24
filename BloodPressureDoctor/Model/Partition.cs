using System.Collections.Generic;

namespace BloodPressureDoctor.Model
{
    public class Partition
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

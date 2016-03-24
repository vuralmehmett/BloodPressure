using System.Collections.Generic;

namespace BloodPressureDoctor.Model
{
    public class OffsetPartitionRequest : IRequestBuffer
    {
        public int MaxNuberOfOffsets { get; set; }
        public long Time { get; set; }

        private readonly Partition partition;

        public OffsetPartitionRequest(int partitionId, long time, int maxNuberOfOffsets)
        {
            partition = new Partition(partitionId);
            Time = time;
            MaxNuberOfOffsets = maxNuberOfOffsets;
        }

        public List<byte> GetRequestBytes()
        {
            var request = new List<byte>();
            request.AddRange(partition.GetRequestBytes());
            request.AddRange(BitWorks.GetBytesReversed(Time));
            request.AddRange(BitWorks.GetBytesReversed(MaxNuberOfOffsets));
            return request;
        }
    }
}

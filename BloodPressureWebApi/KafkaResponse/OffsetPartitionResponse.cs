using System.Collections.Generic;

namespace BloodPressureWebApi.KafkaResponse
{
    public class OffsetPartitionResponse
    {
        private readonly short errorCode;
        private readonly int partitionId;

        public OffsetPartitionResponse(short errorCode, int partitionId)
        {
            this.errorCode = errorCode;
            this.partitionId = partitionId;
            Offsets = new List<long>();
        }

        public List<long> Offsets { get; private set; }

        public short ErrorCode
        {
            get { return errorCode; }
        }

        public int PartitionId
        {
            get { return partitionId; }
        }


        public void Add(long offset)
        {
            Offsets.Add(offset);
        }
    }
}
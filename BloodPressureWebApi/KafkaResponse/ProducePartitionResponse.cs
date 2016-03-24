namespace BloodPressureWebApi.KafkaResponse
{
    public class ProducePartitionResponse
    {
        public int PartitionId { get; private set; }
        public short ErrorCode { get; private set; }
        public long Offset { get; private set; }

        public ProducePartitionResponse(int partitionId, short errorCode, long offset)
        {
            PartitionId = partitionId;
            ErrorCode = errorCode;
            Offset = offset;
        }
    }
}
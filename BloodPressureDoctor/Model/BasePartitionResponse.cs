namespace BloodPressureDoctor.Model
{
    public class BasePartitionResponse
    {
        private short errorCode;
        private int partitionId;

        public short ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        public int PartitionId
        {
            get { return partitionId; }
            set { partitionId = value; }
        }

        protected BasePartitionResponse(int partitionId, short errorCode)
        {
            this.partitionId = partitionId;
            this.errorCode = errorCode;
        }

        public int ParseHeaderData(byte[] data, int offset)
        {
            var dataOffset = offset;
            dataOffset = BufferReader.Read(data, dataOffset, out partitionId);
            dataOffset = BufferReader.Read(data, dataOffset, out errorCode);
            return dataOffset;
        }
    }
}

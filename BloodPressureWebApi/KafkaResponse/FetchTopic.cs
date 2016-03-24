using System.Collections.Generic;
using BloodPressureWebApi.Common;

namespace BloodPressureWebApi.KafkaResponse
{
    public class FetchTopic : BaseTopic
    {
        public List<FetchPartitionResponse> Partitions { get; set; }
        public FetchTopic(string topicName)
            : base(topicName)
        {
        }

        public int Parse(byte[] data, int dataIndex)
        {
            int bufferOffset = ParseHeaderData(data, dataIndex);
            Partitions = new List<FetchPartitionResponse>(PartitionCount);

            for (var i = 0; i < PartitionCount; i++)
            {
                int partitionId;
                short errorCode;
                bufferOffset = BufferReader.Read(data, bufferOffset, out partitionId);
                bufferOffset = BufferReader.Read(data, bufferOffset, out errorCode);
                var fetchPartition = new FetchPartitionResponse(partitionId, errorCode);
                Partitions.Add(fetchPartition);
                bufferOffset = fetchPartition.Parse(data, bufferOffset);
                if (bufferOffset >= data.Length)
                    break;
            }
            return bufferOffset;
        }
    }
}
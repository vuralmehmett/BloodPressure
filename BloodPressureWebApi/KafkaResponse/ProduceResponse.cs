﻿using System;
using System.Collections.Generic;
using BloodPressureWebApi.Common;
using BloodPressureWebApi.KafkaRequest;

namespace BloodPressureWebApi.KafkaResponse
{
    public class ProduceResponse
    {
        private readonly Dictionary<String, List<ProducePartitionResponse>> responsePartitions = new Dictionary<string, List<ProducePartitionResponse>>();
        private int correlationId;

        public int CorrelationId
        {
            get { return correlationId; }
        }

        public void Parse(byte[] data)
        {
            var dataOffset = BufferReader.Read(data, 0, out correlationId);

            int numTopics;
            dataOffset = BufferReader.Read(data, dataOffset, out numTopics);
            for (var i = 0; i < numTopics; i++)
            {
                String topicName;
                dataOffset = BufferReader.Read(data, dataOffset, out topicName);
                int partitionCount;
                dataOffset = BufferReader.Read(data, dataOffset, out partitionCount);

                var partitions = new List<ProducePartitionResponse>();

                for (var partitionIndex = 0; partitionIndex < partitionCount; partitionIndex++)
                {

                    int partitionId;
                    dataOffset = BufferReader.Read(data, dataOffset, out partitionId);

                    short errorCode;
                    dataOffset = BufferReader.Read(data, dataOffset, out errorCode);

                    long offset;
                    dataOffset = BufferReader.Read(data, dataOffset, out offset);

                    var producePartition = new ProducePartitionResponse(partitionId, errorCode, offset);
                    partitions.Add(producePartition);
                }
                responsePartitions.Add(topicName, partitions);
            }
        }

        public short ErrorCode(string topicName, int partitionId)
        {
            return responsePartitions[topicName][partitionId].ErrorCode;
        }

        public long Offset(string topicName, int partitionId)
        {
            return responsePartitions[topicName][partitionId].Offset;
        }
    }
}
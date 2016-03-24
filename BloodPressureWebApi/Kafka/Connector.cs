using System;
using BloodPressureWebApi.Common;
using BloodPressureWebApi.KafkaRequest;
using BloodPressureWebApi.KafkaResponse;

namespace BloodPressureWebApi.Kafka
{
    public class Connector
    {
        public string Server { get; }
        public int Port { get; }
        private const int DefaultCorrelationId = 0;

        /// <summary>
        /// Initializes a new instance of the Consumer class.
        /// </summary>
        /// <param name="server">The server to connect to.</param>
        /// <param name="port">The port to connect to.</param>

        public Connector(string server, int port)
        {
            this.Server = server;
            this.Port = port;
        }

        /// <summary>
        /// Get meta data for a topic
        /// </summary>
        /// <param name="correlationId"></param>Id used by the client to identify this transaction. Returned in the response
        /// <param name="clientId"></param>Name to identify the client. Used in server logs
        /// <param name="topicName"></param> Name of the requested topic. If topic name is null metadata for all topics will be returned
        /// <returns></returns>
        public MetadataResponse Metadata(int correlationId, string clientId, String topicName)
        {
            MetadataRequest request = new MetadataRequest(correlationId, clientId, topicName);

            using (var connection = new KafkaConnection(Server, Port))
            {
                connection.Write(request.GetRequestBytes().ToArray());

                int dataLength = BitConverter.ToInt32(BitWorks.ReverseBytes(connection.Read(4)), 0);

                if (dataLength == 0)
                    return null;
                byte[] data = connection.Read(dataLength);
                MetadataResponse metadataResponse = new MetadataResponse();
                metadataResponse.Parse(data, 0);
                return metadataResponse;
            }
        }

        public ProduceResponse Produce(int correlationId, string clientId, int timeOut, string topicName, int partitionId, byte[] payLoad, int clientNo)
        {
            var request = new ProduceRequest(timeOut, correlationId, clientId);
            request.AddMessage(topicName, partitionId, payLoad, clientNo);
            using (var connection = new KafkaConnection(Server, Port))
            {
                connection.Write(request.GetRequestBytes().ToArray());

                int dataLength = BitConverter.ToInt32(BitWorks.ReverseBytes(connection.Read(4)), 0);

                var response = new ProduceResponse();
                if (dataLength != 0)
                {
                    byte[] data = connection.Read(dataLength);
                    response.Parse(data);
                }
                return response;
            }
        }

        public FetchResponse Fetch(string topic, int partition, int correlationId, string clientId, long offset, int maxBytes, bool fromStart)
        {
            if (fromStart)
                offset = OffsetRequest.LatestTime;
            FetchRequest fetchRequest = new FetchRequest(correlationId, clientId);
            fetchRequest.AddTopic(topic, partition, offset, maxBytes);
            return Fetch(fetchRequest);
        }

        public FetchResponse Fetch(string topic, int partition, int correlationId, string clientId, long offset, int maxBytes)
        {
            FetchRequest fetchRequest = new FetchRequest(correlationId, clientId);
            fetchRequest.AddTopic(topic, partition, offset, maxBytes);
            return Fetch(fetchRequest);
        }

        public FetchResponse Fetch(FetchRequest request)
        {
            using (KafkaConnection connection = new KafkaConnection(Server, Port))
            {
                connection.Write(request.GetRequestBytes().ToArray());
                int dataLength = BitConverter.ToInt32(BitWorks.ReverseBytes(connection.Read(4)), 0);
                if (dataLength > 0)
                {
                    byte[] data = connection.Read(dataLength);
                    var fetchResponse = new FetchResponse(data);
                    return fetchResponse;
                }
                return null;
            }
        }

        /// <summary>
        /// Get a list of valid offsets (up to maxSize) before the given time.
        /// </summary>
        /// <param name="topic">The topic to check.</param>
        /// <param name="clientId"></param>Name to identify the client. Used in server logs
        /// <param name="partitionId">The partition on the topic.</param>
        /// <returns>OffseRersponse containing the first offser for the topic</returns>
        public OffsetResponse GetStartOffset(string topic, string clientId, int partitionId)
        {
            var request = new OffsetRequest(DefaultCorrelationId, clientId);
            request.AddTopic(topic, partitionId, OffsetRequest.EarliestTime, 1);
            return GetOffsetResponseBefore(request);
        }

        /// <summary>
        /// Get a list of valid offsets (up to maxSize) before the given time.
        /// </summary>
        /// <param name="request">The offset request.</param>
        /// <returns>List of offsets, in descending order.</returns>
        public OffsetResponse GetOffsetResponseBefore(OffsetRequest request)
        {
            using (var connection = new KafkaConnection(Server, Port))
            {
                connection.Write(request.GetRequestBytes().ToArray());

                int dataLength = BitConverter.ToInt32(BitWorks.ReverseBytes(connection.Read(4)), 0);

                if (dataLength == 0)
                    return null;
                byte[] data = connection.Read(dataLength);
                var offsetResponse = new OffsetResponse(data);
                return offsetResponse;
            }
        }


        /// <summary>
        /// Get a list of valid offsets (up to maxSize) before the given time.
        /// </summary>
        /// <param name="topic">The topic to check.</param>
        /// <param name="clientId"></param>Name to identify the client. Used in server logs
        /// <param name="partitionId">The partition on the topic.</param>
        /// <returns>OffseRersponse containing the next offser for the topic</returns>
        public OffsetResponse GetCurrentOffset(string topic, string clientId, int partitionId)
        {
            var request = new OffsetRequest(DefaultCorrelationId, clientId);
            request.AddTopic(topic, partitionId, OffsetRequest.LatestTime, 1);
            return GetOffsetResponseBefore(request);
        }

        /// <summary>
        /// Get a list of valid offsets (up to maxSize) before the given time.
        /// </summary>
        /// <param name="topic">The topic to check.</param>
        /// <param name="time">time in millisecs (if -1, just get from the latest available)</param>
        /// <param name="maxNumOffsets">That maximum number of offsets to return.</param>
        /// <param name="correlationId"></param>Id used by the client to identify this transaction. Returned in the response
        /// <param name="clientId"></param>Name to identify the client. Used in server logs
        /// <param name="partitionId">The partition on the topic.</param>
        /// <returns>OffseRersponse</returns>
        public OffsetResponse GetOffsetResponse(string topic, long time, int maxNumOffsets, int correlationId, string clientId, int partitionId)
        {
            var request = new OffsetRequest(correlationId, clientId);
            request.AddTopic(topic, partitionId, time, maxNumOffsets);
            return GetOffsetResponseBefore(request);
        }

    }
}
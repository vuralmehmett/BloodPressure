using System;
using BloodPressureDoctor.Model;

namespace BloodPressureDoctor.Kafka
{
    public class Connector
    {
        public string Server { get; }
        public int Port { get; }
        private const int DefaultCorrelationId = 0;

        public Connector(string server, int port)
        {
            this.Server = server;
            this.Port = port;
        }

        public OffsetResponse GetOffsetResponse(string topic, long time, int maxNumOffsets, int correlationId, string clientId, int partitionId)
        {
            var request = new OffsetRequest(correlationId, clientId);
            request.AddTopic(topic, partitionId, time, maxNumOffsets);
            return GetOffsetResponseBefore(request);
        }

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
    }
}

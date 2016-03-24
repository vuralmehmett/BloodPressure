using BloodPressureWebApi.Common;

namespace BloodPressureWebApi.KafkaClient
{
    public delegate void ConsumeDelegate(KafkaMessage message);
    public interface IKafkaMessageConsumer
    {
        void Start(KafkaBusConnector busConnector, string topicName, int partitionId, long startOffset, ConsumeDelegate consumeDelegate);
        void Pause();
        void Resume();
        void Stop();
    }
}
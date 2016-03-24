namespace BloodPressureWebApi.KafkaException
{
    public class NoTopicException : KafkaException
    {
        public NoTopicException(string topicName)
           : base("Topic " + topicName + "don't exist!")
        {
        }

    }
}
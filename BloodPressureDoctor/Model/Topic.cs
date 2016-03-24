using System.Collections.Generic;
using System.Text;

namespace BloodPressureDoctor.Model
{
    public class Topic : IRequestBuffer
    {
        private readonly string _topicName;

        public List<IRequestBuffer> Partitions { get; set; } = new List<IRequestBuffer>();

        public Topic(string name)
        {
            _topicName = name;
        }

        /// <summary>
        /// Serialize the topic and all contained partitions to a byte buffer
        /// </summary>
        /// <returns>Byte buffer containing the topic and partition data</returns>
        public List<byte> GetRequestBytes()
        {
            var request = new List<byte>();
            if (_topicName == null)
                return request;
            request.AddRange(BitWorks.GetBytesReversed((short)_topicName.Length));
            request.AddRange(Encoding.ASCII.GetBytes(_topicName));
            if (Partitions.Count > 0)
            {
                request.AddRange(BitWorks.GetBytesReversed(Partitions.Count));
                foreach (IRequestBuffer partition in Partitions)
                {
                    request.AddRange(partition.GetRequestBytes());
                }
            }
            return request;
        }
    }
}

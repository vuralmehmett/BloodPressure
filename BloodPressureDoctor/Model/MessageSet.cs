using System.Collections.Generic;
using System.Linq;

namespace BloodPressureDoctor.Model
{
    public class MessageSet
    {
        private long _messageOffset;
        private int _messageSize;
        private Message _message;

        public long MessageOffset
        {
            get { return _messageOffset; }
            set { _messageOffset = value; }
        }

        public int MessageSize
        {
            get { return _messageSize; }
            set { _messageSize = value; }
        }

        public Message Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public MessageSet()
        {
        }

        public MessageSet(byte[] payLoad)
        {
            _message = new Message(payLoad);
        }

        public Message SetMessage(byte[] payLoad)
        {
            _message = new Message(payLoad);
            return _message;
        }

        public int Parse(byte[] data, int offset)
        {
            var dataOffset = offset;
            dataOffset = BufferReader.Read(data, dataOffset, out _messageOffset);
            dataOffset = BufferReader.Read(data, dataOffset, out _messageSize);
            _message = Message.ParseFrom(data.Skip(dataOffset).ToArray());
            // Return used byte count
            return _messageSize + dataOffset - offset;
        }

        public List<byte> GetRequestBytes()
        {
            var requestBytes = new List<byte>();
            requestBytes.AddRange(BitWorks.GetBytesReversed(_messageOffset)); // Don't care

            var messageBuffer = new List<byte>();
            messageBuffer.AddRange(_message.GetBytes());
            requestBytes.AddRange(BitWorks.GetBytesReversed(messageBuffer.Count));
            requestBytes.AddRange(messageBuffer);
            return requestBytes;
        }
    }
}

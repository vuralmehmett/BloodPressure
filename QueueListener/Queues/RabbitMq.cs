using System.Collections.Generic;
using CommonQueueManager.Interface;
using CommonQueueManager.QueueManager;
using Ninject;

namespace QueueListener.Queues
{
    public class RabbitMq
    {
        public readonly StandardKernel Kernel = new StandardKernel();

        public RabbitMq()
        {
            Kernel.Load(new QueueModule());
        }

        public List<string> GetQueue()
        {
            var ninjectConnect = Kernel.Get<IQueueManager>();
            var model = ninjectConnect.GetAllMessage();
            return model;
        }

        public List<string> GetSpesificMessageWithCount(ushort messageCount)
        {
            var ninjectConnect = Kernel.Get<IQueueManager>();
            var model = ninjectConnect.GetSpecificMessage(messageCount);
            return model;
        }
    }
}

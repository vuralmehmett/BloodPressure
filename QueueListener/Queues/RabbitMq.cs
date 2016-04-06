using System.Collections.Generic;
using CommonQueueManager.Interface;
using CommonQueueManager.QueueManager;
using Ninject;

namespace QueueListener.Queues
{
    public class RabbitMq
    {
        public readonly StandardKernel Kernel = new StandardKernel();


        //TODO : StartRead, ConfirmRead RollbackRead kuyrukta put olması lazım ortak projede webapi ve console uygulamalarının hepsi kullansın
        //TODO : thread loopunda verileri kuyruktan cekilecek ve belli sayıda gelecek.

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

using CommonQueueManager.Interface;
using Ninject.Modules;

namespace CommonQueueManager.QueueManager
{
    public class QueueModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IQueueManager>().To<RabbitManager>();
        }
    }
}

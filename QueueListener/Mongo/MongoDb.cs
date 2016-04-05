using System.Collections.Generic;
using CommonDbManager.DbManager;
using CommonDbManager.Interface;
using Ninject;

namespace QueueListener.Mongo
{
    public class MongoDb
    {
        public readonly StandardKernel Kernel = new StandardKernel();

        public MongoDb()
        {
            Kernel.Load(new DbModule());
        }

        public void InsertDataMongoDb(List<string> model)
        {
            var ninjectConnect = Kernel.Get<IDbManager>();
            ninjectConnect.InsertListOfMessage(model);
        }
    }
}

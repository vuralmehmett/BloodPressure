using CommonDbManager.Interface;
using Ninject.Modules;

namespace CommonDbManager.DbManager
{
   public class DbModule : NinjectModule
    {
       public override void Load()
       {
           Bind<IDbManager>().To<MongoManager>();
       }
    }
}

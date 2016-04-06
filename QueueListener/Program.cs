using QueueListener.Mongo;
using QueueListener.Queues;

namespace QueueListener
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMq rabbit = new RabbitMq();
            var result = rabbit.GetQueue();
            var resultWithCount = rabbit.GetSpesificMessageWithCount(5);

            MongoDb mongo = new MongoDb();
            mongo.InsertDataMongoDb(result);

        }
    }
}

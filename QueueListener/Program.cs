using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using QueueListener.Mongo;
using QueueListener.Queues;

namespace QueueListener
{
    class QueueController
    {
        public static void Start(int taskId)
        {
            while (true)
            {
                RabbitMq rabbit = new RabbitMq();
                //var result = rabbit.GetQueue();
                var result = rabbit.GetSpesificMessageWithCount(5);

                Console.WriteLine("Task: {0}, received from queue {1} messages", taskId, result.Count);

                MongoDb mongo = new MongoDb();
                mongo.InsertDataMongoDb(result);

                Console.WriteLine("Task: {0}, processed from queue {1} messages", taskId, result.Count);

                Thread.Sleep(10);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int NO_OF_TASKS = 1;
            List<Task> listOfTask = new List<Task>();

            for (int i = 0; i < NO_OF_TASKS; ++i)
            {
                int taskId = i;

                Task tsk = new Task(() =>
                {
                    QueueController.Start(taskId);
                }, TaskCreationOptions.LongRunning);

                listOfTask.Add(tsk);
            }

            listOfTask.ForEach(t => t.Start());

            Task.WaitAll(listOfTask.ToArray());
        }
    }
}

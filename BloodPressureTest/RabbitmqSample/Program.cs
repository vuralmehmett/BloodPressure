using CommonQueueManager.QueueManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestRabbitMq.RabbitMq;

namespace TestRabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            TestOperations.CreateTasks();
        }

        public class GetPseudoData
        {
            public static void Start(int partitionId)
            {
                RabbitManager manager = new RabbitManager();

                Receive receive = new Receive();

                receive.GetOneByOneMessage(manager, 10, 5);
            }
        }

        public class PseudoDataGenerator
        {
            public static void Start(int partitionId)
            {
                RabbitManager manager = new RabbitManager();

                while (true)
                {
                    Send send = new Send();

                    string message = string.Format("Test messages for partition id: {0}", partitionId);

                    send.SendMessage(manager, partitionId, message);
                }
            }
        }

        public class TestOperations
        {
            public static void CreateTasks()
            {
                const int NO_OF_TASKS = 1;

                List<Task> listOfTask = new List<Task>();

                for (int i = 0; i < NO_OF_TASKS; ++i)
                {
                    int partitionId = i;

                    Task tsk = new Task(() =>
                    {
                        GetPseudoData.Start(partitionId);
                        //PseudoDataGenerator.Start(partitionId);

                    }, TaskCreationOptions.LongRunning);

                    listOfTask.Add(tsk);
                }

                listOfTask.ForEach(t => t.Start());

                Task.WaitAll(listOfTask.ToArray());
            }
        }


    }
}

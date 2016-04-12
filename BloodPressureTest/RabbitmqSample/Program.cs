using System.Collections.Generic;
using System.Threading.Tasks;
using TestRabbitMq.RabbitMq;

namespace TestRabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSendMessage.SendMessage();
        }

        public class PseudoDataGenerator
        {
            public static void Start(int taskId)
            {
                Send send = new Send();
                send.SendMessage(taskId);
            }
        }

        public class TestSendMessage
        {
            public static void SendMessage()
            {
                const int NO_OF_TASKS = 5;

                List<Task> listOfTask = new List<Task>();

                for (int i = 0; i < NO_OF_TASKS; ++i)
                {
                    int taskId = i;

                    Task tsk = new Task(() =>
                    {
                        PseudoDataGenerator.Start(taskId);

                    }, TaskCreationOptions.LongRunning);

                    listOfTask.Add(tsk);
                }

                listOfTask.ForEach(t => t.Start());

                Task.WaitAll(listOfTask.ToArray());
            }
        }

    }

}

using System;
using System.Text;
using RabbitMQ.Client;
using CommonQueueManager.QueueManager;

namespace TestRabbitMq.RabbitMq
{
    public class Send
    {
        public void SendMessage(RabbitManager manager, int partitionId, string data)
        {
            var result = manager.PutData(data);

            Console.WriteLine(" [x] Sent {0} : task id: {1} ---- partition Id: {2}", data,
                        System.Threading.Thread.CurrentThread.ManagedThreadId, partitionId);   
        }
    }
}

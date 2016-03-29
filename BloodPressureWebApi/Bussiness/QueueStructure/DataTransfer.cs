using System.Collections.Generic;
using BloodPressureWebApi.Models;

namespace BloodPressureWebApi.Bussiness.QueueStructure
{
    public class DataTransfer
    {
        public static Queues.RabbitMq RabbitMq = new Queues.RabbitMq();

        public bool SendMessage(BloodPressureModel model)
        {
            var result = RabbitMq.SendMessage(model);
            return result;
        }

        public List<string> GetMessage()
        {
            var result = RabbitMq.GetMessage();
            return result;
        }
    }
}
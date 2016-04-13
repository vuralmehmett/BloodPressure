using System.Collections.Generic;
using CommonQueueManager.Model;

namespace CommonQueueManager.Interface
{
    public interface IQueueManager
    {
        bool PutData(string model);
        BloodPressureModel PullData(int patientNo);
        List<string> GetAllMessage();
        List<string> GetSpecificMessage(int messageCount);


    }
}

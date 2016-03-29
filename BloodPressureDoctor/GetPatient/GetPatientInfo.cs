using System;
using BloodPressureDoctor.Model;

namespace BloodPressureDoctor.GetPatient
{
    public class GetPatientInfo
    {
        public BloodPressureModel GetInfo(string clientNo)
        {
            var data = new Queues.RabbitMq();
            var result = data.GetMessage(Int32.Parse(clientNo));

            return result;
        }
    }
}

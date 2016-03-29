using System.Collections.Generic;
using BloodPressureWebApi.Models;

namespace BloodPressureWebApi.Abstract
{
    public abstract class AbstractQueue
    {
        public abstract bool SendMessage(BloodPressureModel model);
        public abstract List<string> GetMessage();
    }
}
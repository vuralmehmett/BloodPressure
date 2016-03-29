using BloodPressureDoctor.Model;

namespace BloodPressureDoctor.Abstract
{
    public abstract class AbstractQueue
    {
        public abstract BloodPressureModel GetMessage(int patientNo);
    }
}

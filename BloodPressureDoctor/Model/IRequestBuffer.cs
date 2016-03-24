using System.Collections.Generic;

namespace BloodPressureDoctor.Model
{
    public interface IRequestBuffer
    {
        List<byte> GetRequestBytes();
    }
}

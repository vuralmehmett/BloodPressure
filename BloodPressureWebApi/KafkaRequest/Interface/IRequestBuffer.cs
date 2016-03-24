using System.Collections.Generic;

namespace BloodPressureWebApi.KafkaRequest.Interface
{
    public interface IRequestBuffer
    {
        List<byte> GetRequestBytes();
    }
}
using System;

namespace BloodPressureConsole.Request
{
    public class BloodPressureRequest
    {
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public string TimeStamp { get; set; }
    }
}

using System;

namespace BloodPressureWebApi.Models
{
    public class BloodPressureModel
    {
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public string TimeStamp { get; set; }
    }
}
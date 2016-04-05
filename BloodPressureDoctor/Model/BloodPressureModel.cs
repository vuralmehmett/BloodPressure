using System;
using MongoDB.Bson;

namespace BloodPressureDoctor.Model
{
    public class BloodPressureModel
    {
        public ObjectId _id { get; set; }
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

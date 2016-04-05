using MongoDB.Bson;

namespace QueueListener.Model
{
    public class BloodPressureModel
    {
        public ObjectId _id { get; set; }
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public string TimeStamp { get; set; }
    }
}

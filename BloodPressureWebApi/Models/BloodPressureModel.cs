using MongoDB.Bson;

namespace BloodPressureWebApi.Models
{
    public class BloodPressureModel
    {
        public ObjectId _id { get; set; }
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public string TimeStamp { get; set; }
    }
}
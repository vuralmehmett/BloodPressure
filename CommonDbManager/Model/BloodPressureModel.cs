using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommonDbManager.Model
{
    [BsonIgnoreExtraElements]
    public class BloodPressureModel
    {
        
        //public ObjectId _id { get; set; }
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

using System.Collections.Generic;
using BloodPressureWebApi.Models;
using BloodPressureWebApi.Bussiness.QueueStructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BloodPressureWebApi.Bussiness.Mongo
{
    public class InsertMongo
    {
        private const string ConnectionString = "mongodb://192.168.86.128:27017";
        public void InsertMongoDb(BloodPressureModel model)
        {

            DataTransfer asd = new DataTransfer();
            
            var messageList = asd.GetMessage();

            var modelList = new List<BloodPressureModel>();

            for (var i = 0; i < messageList.Count; i++)
            {
                var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(messageList[i]);
                modelList.Add(serializedJson);
            }

            var client = new MongoClient(ConnectionString);

            var database = client.GetDatabase("TestVural");

            var collection = database.GetCollection<BsonDocument>("TestKafka");

            for (var i = 0; i < modelList.Count; i++)
            {
                var document = new BloodPressureModel
                {
                    ClientNo = modelList[i].ClientNo,
                    TimeStamp = modelList[i].TimeStamp,
                    Pressure = modelList[i].Pressure
                };
                collection.InsertOne(document.ToBsonDocument());
            }

            
        }

        

    }
}
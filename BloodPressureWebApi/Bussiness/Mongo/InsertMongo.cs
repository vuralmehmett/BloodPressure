using System.Collections.Generic;
using BloodPressureWebApi.Bussiness.Kafka;
using BloodPressureWebApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BloodPressureWebApi.Bussiness.Mongo
{
    public class InsertMongo
    {
        public void InsertMongoDb(BloodPressureModel model)
        {

            var kafka = new GetMessageFromKafka();
            var messageList = kafka.GetMessage("atiba");

            var modelList = new List<BloodPressureModel>();

            for (var i = 0; i < messageList.Count; i++)
            {
                var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(messageList[i]);
                modelList.Add(serializedJson);
            }

            

            const string connectionString = "mongodb://192.168.86.128:27017";

            var client = new MongoClient(connectionString);

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
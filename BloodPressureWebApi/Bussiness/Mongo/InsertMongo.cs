using System;
using System.Collections.Generic;
using BloodPressureWebApi.Models;
using BloodPressureWebApi.Bussiness.QueueStructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BloodPressureWebApi.Bussiness.Mongo
{
    public class InsertMongo
    {
        public static string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["MongoConnection"];
        public static string MongoDbName = System.Configuration.ConfigurationManager.AppSettings["MongoDbName"];
        public static string MongoCollectionName = System.Configuration.ConfigurationManager.AppSettings["MongoCollectionName"];

        public void InsertMongoDb(BloodPressureModel model)
        {

            DataTransfer dataTransfer = new DataTransfer();
            var messageList = dataTransfer.GetMessage();
            var modelList = new List<BloodPressureModel>();

            for (var i = 0; i < messageList.Count; i++)
            {
                var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(messageList[i]);
                modelList.Add(serializedJson);
            }

            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BsonDocument>(MongoCollectionName);

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
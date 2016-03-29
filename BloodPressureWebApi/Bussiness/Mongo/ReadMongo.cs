using System.Collections.Generic;
using BloodPressureWebApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BloodPressureWebApi.Bussiness.Mongo
{
    public class ReadMongo
    {
        public static string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["MongoConnection"];
        public static string MongoDbName = System.Configuration.ConfigurationManager.AppSettings["MongoDbName"];
        public static string MongoCollectionName = System.Configuration.ConfigurationManager.AppSettings["MongoCollectionName"];

       public static List<BloodPressureModel> BloodPressureModels = new List<BloodPressureModel>(); 
        

        public static List<BloodPressureModel> GetData()
        {

            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BsonDocument>(MongoCollectionName);

            var result = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

            for (var i = 0; i < result.Count; i++)
            {
                var myObj = BsonSerializer.Deserialize<BloodPressureModel>(result[i]);
                BloodPressureModels.Add(myObj);
            }

            return BloodPressureModels;

        }

        
    }
}
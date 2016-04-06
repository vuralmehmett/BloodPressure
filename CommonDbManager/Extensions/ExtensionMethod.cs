using System;
using System.Collections.Generic;
using System.Configuration;
using CommonDbManager.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CommonDbManager.Extensions
{
    public class ExtensionMethod
    {
        public static string ConnectionString = ConfigurationManager.AppSettings["MongoConnection"];
        public static string MongoDbName = ConfigurationManager.AppSettings["MongoDbName"];
        public static string MongoCollectionName = ConfigurationManager.AppSettings["MongoCollectionName"];

        public bool Rollback(List<string> model)
        {
            try
            {
                var client = new MongoClient(ConnectionString);
                var database = client.GetDatabase(MongoDbName);
                var collection = database.GetCollection<BsonDocument>(MongoCollectionName);

                for (int i = 0; i < model.Count; i++)
                {
                    var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(model[i]);
                    var filter = Builders<BloodPressureModel>.Filter.Eq("ClientNo", serializedJson.ClientNo);
                    var delete = database.GetCollection<BloodPressureModel>(MongoCollectionName).DeleteOne(filter);
                }
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }
    }
}

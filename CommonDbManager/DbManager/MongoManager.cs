using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CommonDbManager.Interface;
using CommonDbManager.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace CommonDbManager.DbManager
{
    public class MongoManager : IDbManager
    {
        public static string ConnectionString = ConfigurationManager.AppSettings["MongoConnection"];
        public static string MongoDbName = ConfigurationManager.AppSettings["MongoDbName"];
        public static string MongoCollectionName = ConfigurationManager.AppSettings["MongoCollectionName"];
        public static List<BloodPressureModel> BloodPressureModels = new List<BloodPressureModel>();

        public List<BloodPressureModel> GetData()
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

        public void InsertMongo(string model)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BsonDocument>(MongoCollectionName);

            var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(model);

            var document = new BloodPressureModel
            {
                ClientNo = serializedJson.ClientNo,
                Pressure = serializedJson.Pressure,
                TimeStamp = serializedJson.TimeStamp,
            };

            collection.InsertOne(document.ToBsonDocument());
        }

        public List<BloodPressureModel> GetWithDateRange(string clientNo, string startDate, string finishDate)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BloodPressureModel>(MongoCollectionName);

            var start = Convert.ToDateTime(startDate + "-01-01");
            var finish = Convert.ToDateTime(finishDate + "-01-01");
            var result =
                collection.AsQueryable()
                    .Where(q => q.ClientNo == int.Parse(clientNo) && q.TimeStamp >= start && q.TimeStamp <= finish)
                    .ToList();

            return result;
        }

        public List<BloodPressureModel> GetWithDate(string clientNo, string finishDate)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BloodPressureModel>(MongoCollectionName);

            var finish = Convert.ToDateTime(finishDate + "-01-01");
            var result =
                collection.AsQueryable()
                    .Where(q => q.ClientNo == int.Parse(clientNo) && q.TimeStamp <= finish)
                    .ToList();

            return result;
        }

        public List<BloodPressureModel> GetWithClientNo(string clientNo)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BloodPressureModel>(MongoCollectionName);

            if (string.IsNullOrEmpty(clientNo))
            {
                return new List<BloodPressureModel>();
            }
            var result =
                collection.AsQueryable()
                    .Where(q => q.ClientNo == int.Parse(clientNo))
                    .ToList();

            return result;
        }

        /// <summary>
        /// mongo için basit rollback yazıldı
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertListOfMessage(List<string> model)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(MongoDbName);
            var collection = database.GetCollection<BsonDocument>(MongoCollectionName);


            for (int i = 0; i < model.Count; i++)
            {
                var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(model[i]);

                var document = new BloodPressureModel
                {
                    ClientNo = serializedJson.ClientNo,
                    Pressure = serializedJson.Pressure,
                    TimeStamp = serializedJson.TimeStamp,
                };

                collection.InsertOne(document.ToBsonDocument());
            }

            return true;


        }
    }
}

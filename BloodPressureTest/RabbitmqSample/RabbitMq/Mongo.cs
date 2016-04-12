using System;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TestRabbitMq.RabbitMq
{
    public class Mongo
    {
        public void MongoInsert(string x)
        {
            string connectionString = ConfigurationManager.AppSettings["MongoConnection"];
            string mongoDbName = ConfigurationManager.AppSettings["MongoDbName"];
            string mongoCollectionName = ConfigurationManager.AppSettings["MongoCollectionName"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(mongoDbName);
            var collection = database.GetCollection<BsonDocument>(mongoCollectionName);

            //  var filter = Builders<BsonDocument>.Filter.Eq("TimeStamp",asd.Year == 2016 );

            var col = database.GetCollection<BloodPressureModel>(mongoCollectionName);
            var tarih = Convert.ToDateTime("2013-04-04");
            var popups =
                col.AsQueryable()
                    .Where(q => q.TimeStamp <= tarih)
                    .ToList();

            
           // var secim = col.AsQueryable<BloodPressureModel>().Where(a => a.TimeStamp == "").Select(a=>a.Pressure).ToList();
            var result = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
            var serializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<BloodPressureModel>(x);

            var document = new BloodPressureModel
            {
                TimeStamp = Convert.ToDateTime(DateTime.Today.ToShortDateString())
            };

            collection.InsertOne(document.ToBsonDocument());
        }
    }

    public class BloodPressureModel
    {
        public ObjectId _id { get; set; }
        public int ClientNo { get; set; }
        public string Pressure { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

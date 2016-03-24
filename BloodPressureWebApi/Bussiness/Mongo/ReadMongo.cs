using MongoDB.Bson;
using MongoDB.Driver;

namespace BloodPressureWebApi.Bussiness.Mongo
{
    public class ReadMongo
    {
        public static void GetData()
        {
            const string connectionString = "mongodb://192.168.86.128:27017";

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("TestVural");

            var collection = database.GetCollection<BsonDocument>("TestKafka");

            var result = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
        }

        
    }
}
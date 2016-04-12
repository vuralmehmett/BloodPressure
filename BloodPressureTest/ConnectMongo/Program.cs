using System;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ConnectMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["MongoConnection"];
            string mongoDbName = ConfigurationManager.AppSettings["MongoDbName"];
            string mongoCollectionName = ConfigurationManager.AppSettings["MongoCollectionName"];
            Random asd = new Random();

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(mongoDbName);

            var collection = database.GetCollection<BsonDocument>(mongoCollectionName);

            var count = collection.Count(Builders<BsonDocument>.Filter.Empty);

            var randomResult =
                collection.Find(Builders<BsonDocument>.Filter.Empty).Limit(-1).Skip(0);

            var result = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

            for (var i = 0; i < result.Count; i++)
            {
                var myObj = BsonSerializer.Deserialize<BsonDocument>(result[i]);
                Console.WriteLine(myObj);
            }

            Console.ReadLine();
        }

    }

}

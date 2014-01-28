using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);

            var server = client.GetServer();
            var database = server.GetDatabase("course");

            Insert(database);

            Console.Read();
        }

        static void Insert(MongoDatabase db)
        {
            var collection = db.GetCollection("insertTest");

            collection.Drop();


            //insert one doc
            var doc = new BsonDocument().Add("x", 1);
            Console.WriteLine(doc);

            collection.Insert(doc);

            Console.WriteLine(doc);

            //batch insert
            List<BsonDocument> documents = new List<BsonDocument>();

            for (var x = 2; x < 10; x++)
            {
                documents.Add(new BsonDocument("x", x));
            }

            collection.InsertBatch(documents);

            foreach (var d in collection.Find(null))
            {
                Console.WriteLine(d);
            }

        }
    }
}

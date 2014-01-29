using System;
using System.Collections.Generic;
using System.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;


namespace DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.AppSettings["mongodb"];
            var client = new MongoClient(connectionString);

            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings["database"]);

            InsertByDocument(database);

            InsertByModel(database);

            Console.Read();
        }

        static void InsertByDocument(MongoDatabase db)
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

        public class Coordinate
        {
            public ObjectId Id { get; set; }
            public int x {get; set;}

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");

                if (Id != ObjectId.Empty)
                    sb.AppendFormat("\"_id\" : ObjectId(\"{0}\"), ", Id);

                sb.AppendFormat("\"x\" : {0}", x);

                sb.Append(" }");

                return sb.ToString();
            }
        }

        static void InsertByModel(MongoDatabase db)
        {
            var collection = db.GetCollection<Coordinate>("insertTest");

            collection.Drop();

            Coordinate c = new Coordinate { x = 1 };

            Console.WriteLine(c);

            collection.Insert(c);

            Console.WriteLine(c);

            List<Coordinate> cs = new List<Coordinate>();

            for (var x = 2; x < 10; x++)
            {
                cs.Add(new Coordinate { x = x });
            }

            collection.InsertBatch(cs);

            foreach(var coord in collection.Find(null))
            {
                Console.WriteLine(coord);
            }
        }
    }
}

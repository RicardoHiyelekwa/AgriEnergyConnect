using MongoDB.Driver;
using System;

namespace AgriEnergyConnect.Data
{
    public static class MongoDbContext
    {
        public static IMongoDatabase Create(string connectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Mongo connection string is required");
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}

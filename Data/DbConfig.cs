using System;

namespace AgriEnergyConnect.Data
{
    public enum PersistenceMode
    {
        Sql,
        Mongo,
        Both
    }

    public class DbConfig
    {
        public string SqlConnectionString { get; }
        public string MongoConnectionString { get; }
        public string MongoDatabase { get; }
        public PersistenceMode Mode { get; }

        // Simple constructor used by DI or manual creation
        public DbConfig(string sqlCs, string mongoCs, string mongoDb, PersistenceMode mode = PersistenceMode.Sql)
        {
            SqlConnectionString = sqlCs ?? string.Empty;
            MongoConnectionString = mongoCs ?? string.Empty;
            MongoDatabase = mongoDb ?? "AgriEnergyConnect";
            Mode = mode;
        }

        // Backward compatible constructor that accepts a single connection string (keeps old usage)
        public DbConfig(string cs) : this(cs, "mongodb://localhost:27017", "AgriEnergyConnect", PersistenceMode.Sql) { }
    }
}

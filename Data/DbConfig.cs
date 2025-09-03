namespace AgriEnergyConnect.Data
{
    public class DbConfig
    {
        public string ConnectionString { get; }
        public DbConfig(string cs) => ConnectionString = cs;
    }
}

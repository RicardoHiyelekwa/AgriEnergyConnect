using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Data
{
    public class MongoRepository
    {
        private readonly IMongoDatabase _db;

        public MongoRepository(IMongoDatabase database)
        {
            _db = database ?? throw new ArgumentNullException(nameof(database));
        }

        // Users
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var col = _db.GetCollection<User>("Users");
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<int> CreateUserAsync(User u)
        {
            var col = _db.GetCollection<User>("Users");
            // Mongo will generate an ObjectId; to keep compatibility we use a generated integer id if absent.
            await col.InsertOneAsync(u);
            // If UserId not set, try to return 0 (caller shouldn't rely on this for Mongo)
            return u.UserId;
        }

        public async Task<bool> IsInRoleAsync(int userId, string roleName)
        {
            var col = _db.GetCollection<User>("Users");
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.UserId, userId),
                Builders<User>.Filter.Eq(u => u.Role, roleName)
            );
            var count = await col.CountDocumentsAsync(filter);
            return count > 0;
        }

        // Farmers
        public async Task<int> AddFarmerAsync(Farmer f)
        {
            var col = _db.GetCollection<Farmer>("Farmers");
            await col.InsertOneAsync(f);
            return f.FarmerId;
        }

        public async Task<Farmer?> GetFarmerByIdAsync(int farmerId)
        {
            var col = _db.GetCollection<Farmer>("Farmers");
            var filter = Builders<Farmer>.Filter.Eq(x => x.FarmerId, farmerId);
            return await col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Farmer?> GetFarmerByUserIdAsync(int userId)
        {
            var col = _db.GetCollection<Farmer>("Farmers");
            var filter = Builders<Farmer>.Filter.Eq(x => x.UserId, userId);
            return await col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            var col = _db.GetCollection<Farmer>("Farmers");
            return await col.Find(Builders<Farmer>.Filter.Empty).ToListAsync();
        }

        // Products
        public async Task<int> AddProductAsync(Product p)
        {
            var col = _db.GetCollection<Product>("Products");
            await col.InsertOneAsync(p);
            return p.ProductId;
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId, DateTime? from, DateTime? to, string? category)
        {
            var col = _db.GetCollection<Product>("Products");
            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.FarmerId, farmerId);

            if (from.HasValue) filter = builder.And(filter, builder.Gte(x => x.ProductionDate, from.Value));
            if (to.HasValue) filter = builder.And(filter, builder.Lte(x => x.ProductionDate, to.Value));
            if (!string.IsNullOrWhiteSpace(category)) filter = builder.And(filter, builder.Eq(x => x.Category, category));

            return await col.Find(filter).SortByDescending(x => x.ProductionDate).ToListAsync();
        }
    }
}

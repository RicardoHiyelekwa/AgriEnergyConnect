using Microsoft.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgriEnergyConnect.Models;
using MongoDB.Driver;

namespace AgriEnergyConnect.Data
{
    public class Repository
    {
        private readonly string _sqlCs;
        private readonly MongoRepository? _mongoRepo;
        private readonly PersistenceMode _mode;

        public Repository(DbConfig cfg)
        {
            _sqlCs = cfg.SqlConnectionString;
            _mode = cfg.Mode;

            if (!string.IsNullOrWhiteSpace(cfg.MongoConnectionString))
            {
                try
                {
                    var client = new MongoClient(cfg.MongoConnectionString);
                    var db = client.GetDatabase(cfg.MongoDatabase);
                    _mongoRepo = new MongoRepository(db);
                }
                catch
                {
                    // ignore mongo init errors; _mongoRepo stays null
                    _mongoRepo = null;
                }
            }
        }

        private SqlConnection Conn() => new SqlConnection(_sqlCs);

        #region Users
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (_mode == PersistenceMode.Mongo && _mongoRepo != null)
                return await _mongoRepo.GetUserByEmailAsync(email);

            const string sql = "SELECT TOP 1 * FROM Users WHERE Email = @Email";
            using var db = Conn();
            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> CreateUserAsync(User u)
        {
            if ((_mode == PersistenceMode.Mongo || _mode == PersistenceMode.Both) && _mongoRepo != null)
            {
                await _mongoRepo.CreateUserAsync(u);
                if (_mode == PersistenceMode.Mongo) return u.UserId;
            }

            if (_mode == PersistenceMode.Mongo) return u.UserId;

            const string insertUser = @"
                INSERT INTO Users (FullName, Email, PasswordHash, Role, CreatedAt)
                OUTPUT INSERTED.UserId
                VALUES (@FullName, @Email, @PasswordHash, @Role, SYSUTCDATETIME());";

            using var db = Conn();
            return await db.ExecuteScalarAsync<int>(insertUser, u);
        }

        public async Task<bool> IsInRoleAsync(int userId, string roleName)
        {
            if (_mode == PersistenceMode.Mongo && _mongoRepo != null)
                return await _mongoRepo.IsInRoleAsync(userId, roleName);

            const string sql = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId AND Role = @Role;";
            using var db = Conn();
            var count = await db.ExecuteScalarAsync<int>(sql, new { UserId = userId, Role = roleName });
            return count > 0;
        }
        #endregion
        // Users
        public async Task<int> CreateUserAsync(User u, string role)
        {
            u.Role = role; // atribui antes de salvar
            return await CreateUserAsync(u);
        }

        // Farmers
        public async Task<int> AddFarmerAsync(Farmer f, int userId)
        {
            f.UserId = userId; // atribui antes de salvar
            return await AddFarmerAsync(f);
        }


        #region Farmers
        public async Task<int> AddFarmerAsync(Farmer f)
        {
            if ((_mode == PersistenceMode.Mongo || _mode == PersistenceMode.Both) && _mongoRepo != null)
            {
                await _mongoRepo.AddFarmerAsync(f);
                if (_mode == PersistenceMode.Mongo) return f.FarmerId;
            }

            if (_mode == PersistenceMode.Mongo) return f.FarmerId;

            const string sql = @"
                INSERT INTO Farmers (Name, Email, Phone, Location, UserId)
                OUTPUT INSERTED.FarmerId
                VALUES (@Name, @Email, @Phone, @Location, @UserId);";

            using var db = Conn();
            return await db.ExecuteScalarAsync<int>(sql, f);
        }

        public async Task<Farmer?> GetFarmerByIdAsync(int farmerId)
        {
            if (_mode == PersistenceMode.Mongo && _mongoRepo != null)
                return await _mongoRepo.GetFarmerByIdAsync(farmerId);

            const string sql = "SELECT TOP 1 * FROM Farmers WHERE FarmerId = @FarmerId;";
            using var db = Conn();
            return await db.QueryFirstOrDefaultAsync<Farmer>(sql, new { FarmerId = farmerId });
        }

        public async Task<Farmer?> GetFarmerByUserIdAsync(int userId)
        {
            if (_mode == PersistenceMode.Mongo && _mongoRepo != null)
                return await _mongoRepo.GetFarmerByUserIdAsync(userId);

            const string sql = "SELECT TOP 1 * FROM Farmers WHERE UserId = @UserId;";
            using var db = Conn();
            return await db.QueryFirstOrDefaultAsync<Farmer>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            if (_mode == PersistenceMode.Mongo && _mongoRepo != null)
                return await _mongoRepo.GetAllFarmersAsync();

            const string sql = "SELECT * FROM Farmers ORDER BY Name;";
            using var db = Conn();
            return await db.QueryAsync<Farmer>(sql);
        }
        #endregion

        #region Products
        public async Task<int> AddProductAsync(Product p)
        {
            if ((_mode == PersistenceMode.Mongo || _mode == PersistenceMode.Both) && _mongoRepo != null)
            {
                await _mongoRepo.AddProductAsync(p);
                if (_mode == PersistenceMode.Mongo) return p.ProductId;
            }

            if (_mode == PersistenceMode.Mongo) return p.ProductId;

            const string sql = @"
                INSERT INTO Products (FarmerId, Name, Category, ProductionDate)
                OUTPUT INSERTED.ProductId
                VALUES (@FarmerId, @Name, @Category, @ProductionDate);";

            using var db = Conn();
            return await db.ExecuteScalarAsync<int>(sql, p);
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerAsync(
            int farmerId, DateTime? from, DateTime? to, string? category)
        {
            if (_mode == PersistenceMode.Mongo && _mongoRepo != null)
                return await _mongoRepo.GetProductsByFarmerAsync(farmerId, from, to, category);

            var sql = "SELECT * FROM Products WHERE FarmerId = @FarmerId";

            if (from.HasValue) sql += " AND ProductionDate >= @From";
            if (to.HasValue) sql += " AND ProductionDate <= @To";
            if (!string.IsNullOrWhiteSpace(category)) sql += " AND Category = @Category";

            sql += " ORDER BY ProductionDate DESC";

            using var db = Conn();
            return await db.QueryAsync<Product>(sql,
                new { FarmerId = farmerId, From = from, To = to, Category = category });
        }
        #endregion
    }
}

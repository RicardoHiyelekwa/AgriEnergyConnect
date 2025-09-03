using Microsoft.Data.SqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Data
{
    public class Repository
    {
        private readonly string _cs;
        public Repository(DbConfig cfg) => _cs = cfg.ConnectionString;

        private SqlConnection Conn() => new SqlConnection(_cs);

        #region Users
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            const string sql = "SELECT TOP 1 * FROM Users WHERE Email = @Email";
            using var db = Conn();
            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> CreateUserAsync(User u, string roleName)
        {
            const string insertUser = @"
                INSERT INTO Users (FullName, Email, PasswordHash, CreatedAt)
                OUTPUT INSERTED.UserId
                VALUES (@FullName, @Email, @PasswordHash, SYSUTCDATETIME());";

            const string getRole = "SELECT RoleId FROM Roles WHERE Name = @Name;";
            const string insertUserRole = "INSERT INTO UserRoles(UserId, RoleId) VALUES(@UserId, @RoleId);";

            using var db = Conn();
            var userId = await db.ExecuteScalarAsync<int>(insertUser, u);
            var roleId = await db.ExecuteScalarAsync<int>(getRole, new { Name = roleName });
            await db.ExecuteAsync(insertUserRole, new { UserId = userId, RoleId = roleId });
            return userId;
        }

        public async Task<bool> IsInRoleAsync(int userId, string roleName)
        {
            const string sql = @"
                SELECT COUNT(*) FROM UserRoles ur
                JOIN Roles r ON r.RoleId = ur.RoleId
                WHERE ur.UserId = @UserId AND r.Name = @RoleName;";

            using var db = Conn();
            var count = await db.ExecuteScalarAsync<int>(sql, new { UserId = userId, RoleName = roleName });
            return count > 0;
        }
        #endregion

        #region Farmers
        public async Task<int> AddFarmerAsync(Farmer f, int? userId = null)
        {
            const string sql = @"
                INSERT INTO Farmers (Name, Email, Phone, Location, UserId)
                OUTPUT INSERTED.FarmerId
                VALUES (@Name, @Email, @Phone, @Location, @UserId);";

            using var db = Conn();
            return await db.ExecuteScalarAsync<int>(sql, new
            {
                f.Name,
                f.Email,
                f.Phone,
                f.Location,
                UserId = userId
            });
        }

        public async Task<Farmer?> GetFarmerByIdAsync(int farmerId)
        {
            const string sql = "SELECT TOP 1 * FROM Farmers WHERE FarmerId = @FarmerId;";
            using var db = Conn();
            return await db.QueryFirstOrDefaultAsync<Farmer>(sql, new { FarmerId = farmerId });
        }

        public async Task<Farmer?> GetFarmerByUserIdAsync(int userId)
        {
            const string sql = "SELECT TOP 1 * FROM Farmers WHERE UserId = @UserId;";
            using var db = Conn();
            return await db.QueryFirstOrDefaultAsync<Farmer>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<Farmer>> GetAllFarmersAsync()
        {
            const string sql = "SELECT * FROM Farmers ORDER BY Name;";
            using var db = Conn();
            return await db.QueryAsync<Farmer>(sql);
        }
        #endregion

        #region Products
        public async Task<int> AddProductAsync(Product p)
        {
            const string sql = @"
                INSERT INTO Products (FarmerId, Name, Category, ProductionDate)
                OUTPUT INSERTED.ProductId
                VALUES (@FarmerId, @Name, @Category, @ProductionDate);";

            using var db = Conn();
            return await db.ExecuteScalarAsync<int>(sql, p);
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerAsync(int farmerId, DateTime? from, DateTime? to, string? category)
        {
            var sql = "SELECT * FROM Products WHERE FarmerId = @FarmerId";

            if (from.HasValue) sql += " AND ProductionDate >= @From";
            if (to.HasValue) sql += " AND ProductionDate <= @To";
            if (!string.IsNullOrWhiteSpace(category)) sql += " AND Category = @Category";

            sql += " ORDER BY ProductionDate DESC";

            using var db = Conn();
            return await db.QueryAsync<Product>(sql, new { FarmerId = farmerId, From = from, To = to, Category = category });
        }
        #endregion
    }
}
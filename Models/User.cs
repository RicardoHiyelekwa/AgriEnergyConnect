using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AgriEnergyConnect.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Usado no Mongo

        public int UserId { get; set; } // Usado no SQL
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = "";
    }
}

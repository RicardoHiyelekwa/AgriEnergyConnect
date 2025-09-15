using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AgriEnergyConnect.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Usado no Mongo

        public int ProductId { get; set; } // Usado no SQL
        public int FarmerId { get; set; } // Relacionamento com Farmer
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime ProductionDate { get; set; }
    }
}

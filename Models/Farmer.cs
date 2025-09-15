using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        [BsonId] // Mongo usa este campo como _id
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // Usado no Mongo, ignorado no SQL

        public int FarmerId { get; set; } // Usado no SQL (PK Identity)
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Location { get; set; } = "";

        public int UserId { get; set; } // Relacionamento com User
    }
}

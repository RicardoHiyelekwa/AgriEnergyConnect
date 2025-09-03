using System;

namespace AgriEnergyConnect.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int FarmerId { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime ProductionDate { get; set; }
    }
}

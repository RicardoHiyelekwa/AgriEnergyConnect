using System;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [Required, StringLength(50)]
        public string Category { get; set; } = "";

        [Required]
        public DateTime ProductionDate { get; set; }

        // Foreign Key to Farmer
        public int FarmerId { get; set; }
        public Farmer? Farmer { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = "";

        [Required, Phone, StringLength(20)]
        public string Phone { get; set; } = "";

        [Required, StringLength(200)]
        public string Location { get; set; } = "";

        // Foreign Key to User
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

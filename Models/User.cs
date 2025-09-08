using System;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required, StringLength(50)]
        public string Role { get; set; } = "Farmer"; // Default role is Farmer

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

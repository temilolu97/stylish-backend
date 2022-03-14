using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcommerceCRUD.Models
{

    [Index(nameof(Email), IsUnique =true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        public int Role { get; set; }

        [Required, JsonIgnore]
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}

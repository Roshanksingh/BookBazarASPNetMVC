using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookBazar.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.MinValue;
        public DateTime? DateUpdated { get; set; } = DateTime.MinValue;
        public string? City { get; set; }
        public String? State { get; set; }

        public string? PostalCode { get; set; }
    }
}

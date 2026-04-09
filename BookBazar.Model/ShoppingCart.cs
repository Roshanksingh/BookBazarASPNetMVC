using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBazar.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        [ValidateNever]
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
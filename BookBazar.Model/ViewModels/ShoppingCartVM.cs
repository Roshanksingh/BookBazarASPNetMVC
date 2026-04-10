using System.ComponentModel.DataAnnotations;

namespace BookBazar.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public int TotalQuantity { get; set; }
        public OrderHeader OrderHeader { get; set; } = new();

        // ADD THIS — now the view can use asp-for="OrderInput.Name" 
        // because OrderInput lives on the model the view already knows about
        public OrderHeaderInputVM OrderInput { get; set; } = new();
    }

    public class OrderHeaderInputVM
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;
    }
}
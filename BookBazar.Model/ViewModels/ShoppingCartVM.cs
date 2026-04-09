namespace BookBazar.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public int TotalQuantity { get; set; }
        public OrderHeader OrderHeader { get; set; } = new();
    }
}
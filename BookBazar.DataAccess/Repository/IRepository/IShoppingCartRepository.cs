using BookBazar.Models;

namespace BookBazar.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
        void RemoveRange(IEnumerable<ShoppingCart> shoppingCarts);

    }
}

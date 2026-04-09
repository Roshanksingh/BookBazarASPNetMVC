using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDBContext _db;

        public ShoppingCartRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
        }

        public void RemoveRange(IEnumerable<ShoppingCart> shoppingCarts)
        {
            _db.ShoppingCarts.RemoveRange(shoppingCarts);
        }
    }
}
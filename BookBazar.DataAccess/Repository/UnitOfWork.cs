using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;

namespace BookBazar.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _db;

        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }

        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
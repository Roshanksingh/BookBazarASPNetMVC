using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDBContext _db;

        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void update(Product p)
        {
            Product? productFromDb = _db.Product.FirstOrDefault(u => u.Id == p.Id);

            if (productFromDb != null)
            {
                productFromDb.Title = p.Title;
                productFromDb.Description = p.Description;
                productFromDb.ISBN = p.ISBN;
                productFromDb.Author = p.Author;
                productFromDb.Price = p.Price;
                productFromDb.ListPrice = p.ListPrice;
                productFromDb.Price50 = p.Price50;
                productFromDb.Price100 = p.Price100;
                productFromDb.CategoryId = p.CategoryId;

                if (!string.IsNullOrEmpty(p.ImageUrl))
                {
                    productFromDb.ImageUrl = p.ImageUrl;
                }
            }
        }
    }
}
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void update(Product p);
    }
}
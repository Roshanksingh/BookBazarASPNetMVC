using BookBazar.Models;

namespace BookBazar.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void update(Category c);
    }
}
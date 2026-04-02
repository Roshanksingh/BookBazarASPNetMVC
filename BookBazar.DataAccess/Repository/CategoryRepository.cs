using System.Linq.Expressions;
using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDBContext _db;

        public CategoryRepository(ApplicationDBContext db): base(db)
        {
            _db = db;
        }

        public void update(Category c)
        {
            _db.Categories.Update(c);
        }
    }
}
using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDBContext _db;

        public ApplicationUserRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
    }
}

using BookBazar.DataAccess.Repository.IRepository;

namespace BookBazar.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category {get;}

        void Save();
    }
}
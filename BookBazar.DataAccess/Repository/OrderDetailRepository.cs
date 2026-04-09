using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDBContext _db;

        public OrderDetailRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
        }
    }
}
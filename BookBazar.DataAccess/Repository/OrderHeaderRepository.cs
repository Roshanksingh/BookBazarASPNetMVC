using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.Repository.IRepository;
using BookBazar.Models;

namespace BookBazar.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDBContext _db;

        public OrderHeaderRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;

                if (!string.IsNullOrWhiteSpace(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string? paymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (orderFromDb != null)
            {
                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    orderFromDb.SessionId = sessionId;
                }

                if (!string.IsNullOrWhiteSpace(paymentIntentId))
                {
                    orderFromDb.PaymentIntentId = paymentIntentId;
                }
            }
        }
    }
}
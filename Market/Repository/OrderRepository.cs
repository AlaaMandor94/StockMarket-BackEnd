using Market.Interfaces;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StockMarketContext _dbContext;
        public OrderRepository(StockMarketContext db)
        {
            _dbContext = db;
        }
        public async Task<List<Order>> GetOrders()
        {
            var Orders = await _dbContext.Orders.Include(o => o.Stock).ToListAsync();
            return Orders;
        }
        public async Task<Order> GetOrder(int Id)
        {
            var order = await _dbContext.Orders.Include(o => o.Stock).FirstOrDefaultAsync(Or => Or.id == Id);
            return order;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            var existingOrder = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.PersonName == order.PersonName && o.StockId == order.StockId);
            return existingOrder;
        }


    }
}

using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task<Order> CreateOrder(Order order);
        Task<Order> GetOrder(int Id);
    }
}

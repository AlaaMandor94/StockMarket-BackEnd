using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Market.Models;
using Market.DTOs;
using Market.Repository;
using Market.Interfaces;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _dbOrder;
        private readonly IStockRepository _dbStock;
        private readonly StockMarketContext _dbContext;

        public OrdersController(
            IOrderRepository dbOrder,
            IStockRepository dbStock,
            StockMarketContext dbContext
        )
        {
            _dbOrder = dbOrder;
            _dbStock = dbStock;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            List<OrderDto> orderDtos = new List<OrderDto>();
            var Orders = await _dbOrder.GetOrders();
            if (Orders != null)
            {
                foreach (var order in Orders)
                {
                    OrderDto orderDto = new OrderDto
                    {
                        Id = order.id,
                        PersonName = order.PersonName,
                        Quantity = order.Quantity,
                        TotalPrice = order.Quantity * order.Stock.Price,
                        StockId = order.StockId,
                        StockName = order.Stock.Name
                    };

                    orderDtos.Add(orderDto);
                }
            }
            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var existingOrder = await _dbOrder.CreateOrder(order);

            var stock = await _dbStock.GetStockById(order.StockId);

            if (existingOrder != null)
            {
                existingOrder.Quantity += order.Quantity;
                existingOrder.TotalPrice = existingOrder.Quantity * stock.Price;
                await _dbContext.SaveChangesAsync();
                OrderDto existOrderDto = new OrderDto()
                {
                    Id = order.id,
                    PersonName = existingOrder.PersonName,
                    Quantity = existingOrder.Quantity,
                    TotalPrice = existingOrder.Quantity * existingOrder.Stock.Price,
                    StockId = existingOrder.StockId,
                    StockName = existingOrder.Stock.Name
                };
                // Configure JsonSerializerOptions with ReferenceHandler.Preserve
                return Created("existingOrder", existOrderDto);
            }

            order.TotalPrice = order.Quantity * stock.Price;
            _dbContext.Orders.Add(order);
            OrderDto orderDto = new OrderDto()
            {
                Id = order.id,
                PersonName = order.PersonName,
                Quantity = order.Quantity,
                TotalPrice = order.Quantity * order.Stock.Price,
                StockId = order.StockId,
                StockName = order.Stock.Name
            };
            await _dbContext.SaveChangesAsync();
            return Created("new order",orderDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int Id)
        {
            var order = await _dbOrder.GetOrder(Id);
            if (order == null)
            {
                return NotFound();
            }
            var orderDto = new OrderDto
            {
                Id = order.id,
                PersonName = order.PersonName,
                Quantity = order.Quantity,
                TotalPrice = order.Quantity * order.Stock.Price,
                StockId = order.StockId,
                StockName = order.Stock.Name
            };
            return Ok(orderDto);
        }

    }
}

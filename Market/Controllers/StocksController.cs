using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Market.DTOs;
using Market.Models;
using Market.Interfaces;
namespace StockMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository _SR;
        public StocksController(IStockRepository dbContext)
        {
            _SR = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Stock>>> GetStocks()
        {
            List<Stock> stocks = await _SR.GetAllStocks();
            List<StockDto> stockDtos = new List<StockDto>();

            foreach (var stock in stocks)
            {
                StockDto stockDto = new StockDto()
                {
                    Id = stock.id,
                    Name = stock.Name,
                    Price = stock.Price
                };
                stockDtos.Add(stockDto);
            }
            return Ok(stockDtos);
        }

        
    }
}

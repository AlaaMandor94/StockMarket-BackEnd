using Microsoft.AspNetCore.SignalR;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Hubs
{
    public class StockHub : Hub
    {
        
        private readonly StockMarketContext _dbContext;
        private readonly Random _random;
        public StockHub(StockMarketContext dbContext)
        {
            _dbContext = dbContext;
            _random = new Random();
        }
        [HubMethodName("UpdateStockPrices")]
        public async Task UpdateStockPrices()
        {
            var stocks = await _dbContext.Stocks.ToListAsync();
            foreach (var stock in stocks)
            {
                double randomPrice = _random.Next(1, 10001) / 100.0; // Convert to double
                stock.Price = Math.Round((decimal)randomPrice, 2);
            }
            await _dbContext.SaveChangesAsync();
            await Clients.All.SendAsync("StockPricesUpdated", stocks);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Market.Models;

namespace Market.utilities
{
   
    public class StockPriceGenerator : BackgroundService
    {
        private readonly StockMarketContext _dbContext;
        private readonly Random _random;
        public StockPriceGenerator(StockMarketContext dbContext)
        {
            _dbContext = dbContext;
            _random = new Random();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateStockPrices();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        private async Task UpdateStockPrices()
        {
            var stocks = await _dbContext.Stocks.ToListAsync();
            foreach (var stock in stocks)
            {
                double randomPrice = _random.Next(1, 10001) / 100.0; // Convert to double
                stock.Price = Math.Round((decimal)randomPrice, 2);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}

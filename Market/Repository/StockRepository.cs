using Market.Models;
using Market.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Market.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly StockMarketContext _dbContext;
        public StockRepository(StockMarketContext db)
        {
            _dbContext = db;
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            var stocks = await _dbContext.Stocks.ToListAsync();
            return stocks;
        }
        public async Task<Stock> GetStockById(int Id)
        {
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s=>s.id == Id);

            return stock;
        }
    }
}

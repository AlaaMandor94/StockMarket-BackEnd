using Market.Models;

namespace Market.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocks();
        Task<Stock> GetStockById(int id);
    }
}

using System.Threading.Tasks;

namespace Stocks.Services
{
    public interface IStockService
    {
        Task<string> GetByStockCode(string stock_code);
    }
}

using Application.Common.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StockService : IStockService
    {
        Task<string> IStockService.GetByStockCode(string stock_code)
        {
            throw new System.NotImplementedException();
        }
    }
}

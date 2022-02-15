using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IStockService
    {
        Task<string> GetByStockCode(string stock_code);
    }
}

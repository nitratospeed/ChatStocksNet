using Microsoft.AspNetCore.Mvc;
using Stocks.Services;
using System.Threading.Tasks;

namespace MBProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("stock_code")]
        public async Task<IActionResult> Post(string stock_code)
        {
            if (stock_code != null)
            {
                return Ok(await _stockService.GetByStockCode(stock_code));
            }
            return BadRequest();
        }
    }
}

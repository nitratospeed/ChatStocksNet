using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IRabbitMQProducerService _rabbitMQProducerService;
        private readonly IStockService _stockService;

        public StocksController(IRabbitMQProducerService rabbitMQProducerService, IStockService stockService)
        {
            _rabbitMQProducerService = rabbitMQProducerService;
            _stockService = stockService;
        }

        [HttpGet("{stock_code}")]
        public async Task<IActionResult> Get(string stock_code)
        {
            var stockResult = await _stockService.GetByStockCode(stock_code);
            _rabbitMQProducerService.Push(stockResult);
            return Ok();
        }
    }
}

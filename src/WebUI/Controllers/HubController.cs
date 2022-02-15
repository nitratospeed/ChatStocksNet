using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : ControllerBase
    {
        private readonly IRabbitMQProducerService _rabbitMQProducerService;

        public HubController(IRabbitMQProducerService rabbitMQProducerService)
        {
            _rabbitMQProducerService = rabbitMQProducerService;
        }

        [HttpGet("{stock_code}")]
        public IActionResult GetStocksByStockValue(string stock_code)
        {
            var stockMessage = "45.67";
            _rabbitMQProducerService.Push(stockMessage);
            return Ok();
        }
    }
}

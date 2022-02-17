using Application.Common.Interfaces;
using Application.Core.Stocks.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers.v1
{
    public class StocksController : BaseApiController
    {
        [HttpGet("{stock_code}")]
        public async Task<IActionResult> Get(string stock_code)
        {
            return Ok(await Mediator.Send(new NotifyStockByStockIdQuery { StockCode = stock_code }));
        }
    }
}

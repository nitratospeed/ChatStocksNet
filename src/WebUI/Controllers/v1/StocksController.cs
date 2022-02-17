using Application.Common.Interfaces;
using Application.Core.Stocks.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers.v1
{
    public class StocksController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get(string room, string stock_code)
        {
            return Ok(await Mediator.Send(new NotifyStockByStockIdQuery { Room = room, StockCode = stock_code }));
        }
    }
}

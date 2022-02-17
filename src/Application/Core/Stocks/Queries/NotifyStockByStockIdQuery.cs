using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Stocks.Queries
{
    public class NotifyStockByStockIdQuery : IRequest<BaseResult<bool>>
    {
        public string StockCode { get; set; }
    }
    public class NotifyStockByStockIdQueryHandler : IRequestHandler<NotifyStockByStockIdQuery, BaseResult<bool>>
    {
        private readonly IRabbitMQProducerService _rabbitMQProducerService;
        private readonly IStockService _stockService;

        public NotifyStockByStockIdQueryHandler(IRabbitMQProducerService rabbitMQProducerService, IStockService stockService)
        {
            _rabbitMQProducerService = rabbitMQProducerService;
            _stockService = stockService;
        }

        public async Task<BaseResult<bool>> Handle(NotifyStockByStockIdQuery request, CancellationToken cancellationToken)
        {
            var stockResult = await _stockService.GetByStockCode(request.StockCode);
            _rabbitMQProducerService.Push(stockResult);

            return BaseResult<bool>.Success(true, "");
        }
    }
}

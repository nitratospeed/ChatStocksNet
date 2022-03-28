using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SharedContracts;
using Stocks.Services;
using System;
using System.Threading.Tasks;

namespace Stocks.Consumers
{
    public class GetStockValueConsumer : IConsumer<StockCodeContract>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetStockValueConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<StockCodeContract> context)
        {
            using var scope = _serviceProvider.CreateScope();

            var stockService = scope.ServiceProvider.GetRequiredService<IStockService>();

            var stockValue = await stockService.GetByStockCode(context.Message.StockCode);

            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var getStockValueContract = new StockValueContract { StockValue = stockValue, Room = context.Message.Room };

            await publishEndpoint.Publish(getStockValueContract);
        }
    }
}

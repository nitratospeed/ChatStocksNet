using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SharedContracts;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationService : IConsumer<StockValueContract>
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<StockValueContract> context)
        {
            var chatHub = (IHubContext<SignalRService>)_serviceProvider.GetService(typeof(IHubContext<SignalRService>));

            await chatHub.Clients.Group(context.Message.Room).SendAsync("ReceiveMessage", "StocksBot", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} StocksBot says: {context.Message.StockValue}");
        }
    }
}

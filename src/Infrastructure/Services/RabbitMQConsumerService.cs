using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RabbitMQConsumerService : BackgroundService
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public RabbitMQConsumerService(IServiceProvider serviceProvider, ILogger<RabbitMQConsumerService> logger)
        {
            try
            {
                _logger = logger;
                _factory = new ConnectionFactory() { HostName = "localhost" };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                _serviceProvider = serviceProvider;
                _channel.QueueDeclare(queue: "TestQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Constructor: {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"RabbitMQConsumerService is starting.");

                stoppingToken.Register(() =>
                    _logger.LogInformation($"RabbitMQConsumer background task is stopping."));

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_channel != null)
                    {
                        var consumer = new EventingBasicConsumer(_channel);

                        consumer.Received += async (model, ea) =>
                        {
                            var body = ea.Body.ToArray();

                            var message = Encoding.UTF8.GetString(body);

                            var chatHub = (IHubContext<SignalRService>)_serviceProvider.GetService(typeof(IHubContext<SignalRService>));

                            await chatHub.Clients.Group(message.Split('|')[0]).SendAsync("ReceiveMessage", "StocksBot", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} StocksBot says: {message.Split('|')[1]}");
                        };

                        _channel.BasicConsume(queue: "TestQueue",
                                             autoAck: true,
                                             consumer: consumer);

                        _logger.LogInformation($"RabbitMQConsumer task doing background work.");

                    }

                    return Task.CompletedTask;
                }

                _logger.LogInformation($"RabbitMQConsumer background task is stopping.");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ExecuteAsync: {ex.Message}");
                return Task.CompletedTask;
            }
        }
    }
}

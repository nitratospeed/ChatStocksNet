using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RabbitMQConsumerService : IRabbitMQConsumerService
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumerService(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _serviceProvider = serviceProvider;
        }

        public void Receive()
        {

                _channel.QueueDeclare(queue: "TestQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    var chatHub = (IHubContext<SignalRService>)_serviceProvider.GetService(typeof(IHubContext<SignalRService>));

                    await chatHub.Clients.Group("net devs").SendAsync("ReceiveMessage", "StocksBot", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} StocksBot says: {message}");

                };

                _channel.BasicConsume(queue: "TestQueue",
                                     autoAck: true,
                                     consumer: consumer);          
        }
    }
}

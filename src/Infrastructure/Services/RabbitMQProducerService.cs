using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Infrastructure.Services
{
    public class RabbitMQProducerService : IRabbitMQProducerService
    {
        private readonly ILogger _logger;

        public RabbitMQProducerService(ILogger<RabbitMQProducerService> logger)
        {
            _logger = logger;
        }

        public bool Send(string room, string message)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "TestQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes($"{room}|{message}");

                channel.BasicPublish(exchange: "",
                                     routingKey: "TestQueue",
                                     basicProperties: null,
                                     body: body);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Send: {ex.Message}");
                return false;
            }
        }
    }
}

using Application.Common.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace Infrastructure.Services
{
    public class RabbitMQProducerService : IRabbitMQProducerService
    {
        public void Push(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "TestQueue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "TestQueue",
                                 basicProperties: null,
                                 body: body);
        }
    }
}

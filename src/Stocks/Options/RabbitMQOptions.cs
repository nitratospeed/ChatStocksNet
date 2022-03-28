namespace MBProducer.Options
{
    public class RabbitMQOptions
    {
        public const string RabbitMQ = "RabbitMQ";

        public string Hostname { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string Queue { get; set; }
    }
}

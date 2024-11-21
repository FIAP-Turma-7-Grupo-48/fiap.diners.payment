using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WebApi
{
    public class RabbitMqWorker : BackgroundService
    {
        private readonly ILogger<RabbitMqWorker> _logger;
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMqWorker(ILogger<RabbitMqWorker> logger)
        {
            _logger = logger;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "SendToPayment", durable: false, exclusive: false, autoDelete: false,
                    arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                    return Task.CompletedTask;
                };

                await channel.BasicConsumeAsync("SendToPayment", autoAck: true, consumer: consumer);
            }
        }
    }
}

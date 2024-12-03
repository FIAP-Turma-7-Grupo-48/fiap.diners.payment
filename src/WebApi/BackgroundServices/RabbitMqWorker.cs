using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UseCase.Dtos;
using UseCase.Service.Interfaces;

namespace WebApi.BackgroundServices;

public class RabbitMqWorker : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RabbitMqWorker(IServiceScopeFactory serviceScopeFactory, IConnectionFactory factory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _connectionFactory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "SendToPayment", durable: true, exclusive: false, autoDelete: false,
                arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(body);

                    return ProcessMessage(messageJson, stoppingToken);
                }
                catch (Exception ex)
                {

                    throw;
                }

            };

            await channel.BasicConsumeAsync("SendToPayment", autoAck: true, consumer: consumer);
        }
    }

    private async Task ProcessMessage(string json, CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Deserialize<CreatePaymentDto>(json, _options);

        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IPaymentService orderApplication = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            await orderApplication.CreatePaymentAsync(message, cancellationToken);
        }
    }
}


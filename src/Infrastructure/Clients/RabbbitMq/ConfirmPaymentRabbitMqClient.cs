using Domain.Clients;
using Domain.Entities.PaymentAggregate;
using Infrastructure.Clients.Dtos;
using RabbitMQ.Client;

namespace Infrastructure.Clients.RabbbitMq;

public class ConfirmPaymentRabbitMqClient : RabbitMQPublisher<ConfirmOrderPaymentDto>, IConfirmPaymentClient
{
    public const string QueueName = "OrderPaymentConfirmed";
    public ConfirmPaymentRabbitMqClient(IConnectionFactory factory) : base(factory, QueueName)
    {

    }

    public async Task SendAsync(Payment payment, CancellationToken cancellationToken)
    {

     
        var dto = new ConfirmOrderPaymentDto
        {
            OrderId = payment.ExternalId,
        };

        await PublishMessageAsync(dto, cancellationToken);
    }
}

using Domain.Entities.PaymentAggregate;

namespace Domain.Clients;

public interface IConfirmPaymentClient
{
    Task SendAsync(Payment payment, CancellationToken cancellationToken);
}

using Domain.Entities.PaymentAggregate;

namespace Domain.Gateways;

public interface IPaymentGateway
{
    Task<Payment> CreatePayment(Payment order, CancellationToken cancellationToken);
}

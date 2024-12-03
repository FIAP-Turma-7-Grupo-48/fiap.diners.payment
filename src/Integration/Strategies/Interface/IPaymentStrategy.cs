using Domain.ValueObjects;
using Domain.Entities.PaymentAggregate;

namespace Integration.Strategies.Interface;

public interface IPaymentStrategy
{

    PaymentMethod PaymentMethod { get; }

    Task<Payment> CreatePayment(Payment order, CancellationToken cancellationToken);
}

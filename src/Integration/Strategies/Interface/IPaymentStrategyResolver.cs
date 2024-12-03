using Domain.ValueObjects;

namespace Integration.Strategies.Interface;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(PaymentMethod paymentProvider);
}

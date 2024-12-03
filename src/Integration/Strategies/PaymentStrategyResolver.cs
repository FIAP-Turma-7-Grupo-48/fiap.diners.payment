using Domain.ValueObjects;
using Helpers;
using Integration.Strategies.Interface;

namespace Integration.Strategies;

public class PaymentStrategyResolver : IPaymentStrategyResolver
{
    private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;
    public PaymentStrategyResolver(IEnumerable<IPaymentStrategy> pixPaymentStrategies)
    {
        _paymentStrategies = pixPaymentStrategies;
    }

    public IPaymentStrategy Resolve(PaymentMethod payment)
    {
        var response = _paymentStrategies.FirstOrDefault(x => x.PaymentMethod.Equals(payment)); ;

        if (response == null)
        {
            throw new InvalidOperationException($"Pix Strategy not implemented for kind {payment.Kind.GetEnumDescription()} in Provider {payment.Provider.GetEnumDescription()}");
        }

        return response;
    }
}

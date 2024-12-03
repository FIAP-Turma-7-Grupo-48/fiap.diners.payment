using Domain.Entities.Enums;
using Domain.Entities.PaymentAggregate;
using Domain.Gateways;
using Integration.Strategies.Interface;

namespace Integration.Gateway;
public class PaymentGateway : IPaymentGateway
{
    private readonly IPaymentStrategyResolver _pixStrategyResolver;
    public PaymentGateway(IPaymentStrategyResolver pixStrategyResolver)
    {
        _pixStrategyResolver = pixStrategyResolver;
    }
    public Task<Payment> CreatePayment(Payment payment, CancellationToken cancellationToken)
    {
        if (payment.PaymentMethod.Kind == PaymentMethodKind.None || payment.PaymentMethod.Provider == PaymentProvider.None)
        {
            throw new ArgumentException("Payment Method Can't be null");
        }

        return
            _pixStrategyResolver.Resolve(payment.PaymentMethod).CreatePayment(payment, cancellationToken);
    }
}

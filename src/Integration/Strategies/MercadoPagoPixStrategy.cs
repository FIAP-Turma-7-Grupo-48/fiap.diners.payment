using Domain.Entities.Enums;
using Domain.ValueObjects;
using Integration.Strategies.Interface;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPagoPayment = MercadoPago.Resource.Payment.Payment;
using Payment = Domain.Entities.PaymentAggregate.Payment;

namespace Integration.Strategies;

public class MercadoPagoPixStrategy : IPaymentStrategy
{
    public Domain.ValueObjects.PaymentMethod PaymentMethod => new(PaymentProvider.MercadoPago, PaymentMethodKind.Pix);
    static string AcessToken { get; } = Environment.GetEnvironmentVariable("MercadoPagoAcessToken") ?? throw new Exception("Enviroment Variable MercadoPagoAcessToken not Found");



    public MercadoPagoPixStrategy()
    {
        MercadoPagoConfig.AccessToken = AcessToken;
    }
    public async Task<Payment> CreatePayment(Payment payment, CancellationToken cancellationToken)
    {
        var paymentRequest = new PaymentCreateRequest
        {
            TransactionAmount = payment.Amount,
            Description = $"OrderId: {payment.Id}",
            PaymentMethodId = "pix", // Para gerar QR Code PIX
            Installments = 1,
            StatementDescriptor = "Diners Tech challenge",
            Payer = new PaymentPayerRequest
            {
                Email = "meu@email2.com"
            }
        };

        var client = new PaymentClient();
        MercadoPagoPayment mercadoPagoPayment = await client.CreateAsync(paymentRequest, default, cancellationToken);

        if (mercadoPagoPayment.Status != MercadoPago.Resource.Payment.PaymentStatus.Pending)
        {
            throw new Exception("Falha ao criar o pagamento: " + mercadoPagoPayment.Status);
        }

        string qrCodeBase64 = mercadoPagoPayment.PointOfInteraction.TransactionData.QrCodeBase64;
        var qrCodeByte = Convert.FromBase64String(qrCodeBase64);
        var qrCodeImage = new Photo("qrCode.png", "image/png", qrCodeByte);

        payment.ProviderPaymentId = payment.Id.ToString();
        payment.Image = qrCodeImage;

        return payment;
    }

}

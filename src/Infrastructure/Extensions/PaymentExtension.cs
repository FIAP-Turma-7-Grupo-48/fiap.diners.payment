using Domain.Entities.PaymentAggregate;
using Infrastructure.MongoModels;

namespace Infrastructure.Extensions;

internal static class PaymentExtension
{
    public static PaymentMongoModel ToPaymentMongoModel(this Payment payment)
    {
        PaymentMongoModel response = new()
        {
            Id = payment.Id,
            Status = payment.Status,
            ExternalId = payment.ExternalId,
            Provider = payment.PaymentMethod.Provider,
            Kind = payment.PaymentMethod.Kind,
            ProviderPaymentId = payment.ProviderPaymentId,
            Amount = payment.Amount,
            ImageContentType = payment.Image?.ContentType,
            ImageFileName = payment.Image?.FileName,
            ImageData = payment.Image?.Data,
            CreatedAt = payment.CreatedAt
        };

        return response;
    }
}

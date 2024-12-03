using Domain.Entities.PaymentAggregate;
using Domain.ValueObjects;

namespace Infrastructure.MongoModels.Extensions;

public static class PaymentMongoModelExtension
{
    public static Payment ToPayment(this PaymentMongoModel payment)
    {
        Photo? image = null;
        if (payment.ImageFileName != null && payment.ImageContentType != null && payment.ImageData != null)
        {
            image = new Photo(payment.ImageFileName, payment.ImageContentType, payment.ImageData);
        }

        Payment response = new(payment.ProviderPaymentId, payment.Status, payment.CreatedAt)
        {
            Id = payment.Id,
            ExternalId = payment.ExternalId,
            Amount = payment.Amount,
            PaymentMethod = new PaymentMethod(payment.Provider, payment.Kind),
            Image = image,
        };

        return response;
    }
}

using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.Entities.PaymentAggregate;
using Domain.ValueObjects;

namespace UnitTest.Entities
{
    public class PaymentUnitTest
    {
        [Fact]
        public void SetPaymentToPaidOk()
        {

            Payment payment = new(Guid.NewGuid().ToString(), PaymentStatus.Pending, DateTime.Now)
            {
                Amount = 1,
                PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
                ExternalId = 1
            };

            payment.SetStatusPaid();

            Assert.Equal(PaymentStatus.Paid, payment.Status);

        }

        [Fact]
        public void SetPaymentToPaidInvalid()
        {

            Payment payment = new()
            {
                Amount = 1,
                PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
                ExternalId = 1
            };

            Assert.Throws<UnableToChangePaymentStatusToPaidException>(() => payment.SetStatusPaid());
        }

        [Fact]
        public void SetProviderPaymentIdOk()
        {

            Payment payment = new()
            {
                Amount = 1,
                PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
                ExternalId = 1
            };

            string providerId= Guid.NewGuid().ToString();   

            payment.ProviderPaymentId = providerId;

            Assert.Equal(providerId, payment.ProviderPaymentId);
            Assert.Equal(PaymentStatus.Pending, payment.Status);
        }

        [Fact]
        public void SetProviderPaymentInvalid()
        {

            Payment payment = new(Guid.NewGuid().ToString(), PaymentStatus.Pending, DateTime.Now)
            {
                Amount = 1,
                PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
                ExternalId = 1
            };

            string providerId = Guid.NewGuid().ToString();

            Assert.Throws<UnableToChangePaymentProiderIdException>(() => payment.ProviderPaymentId = providerId);

        }
    }
}

using Domain.Entities.PaymentAggregate;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.MongoModels;
using Infrastructure.MongoModels.Extensions;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Adapters;

public class PaymentRepositoryAdapter : IPaymentRepository
{
    private readonly IPaymentMongoDbRepository _paymentMongoDbRepository;
    public PaymentRepositoryAdapter(IPaymentMongoDbRepository paymentMongoDbRepository)
    {
        _paymentMongoDbRepository = paymentMongoDbRepository;
    }
    public async Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken)
    {
        var paymentMongo = payment.ToPaymentMongoModel();
        paymentMongo = await _paymentMongoDbRepository.CreateAsync(paymentMongo, cancellationToken);
        return paymentMongo.ToPayment();
    }

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken)
    {
        var paymentMongo = payment.ToPaymentMongoModel();
        await _paymentMongoDbRepository.ReplaceOneAsync(paymentMongo, cancellationToken);
    }

    public async Task<Payment?> GetByExternalIdAsync(int id, CancellationToken cancellationToken)
    {
        var paymentMongo = await _paymentMongoDbRepository.GetByExternalIdAsync(id, cancellationToken);
        return paymentMongo?.ToPayment();
    }

    public async Task<Payment?> GetByproviderPaymentIdAsync(string providerPaymentId, CancellationToken cancellationToken)
    {
        var paymentMongo = await _paymentMongoDbRepository.GetByProviderPaymentIdAsync(providerPaymentId, cancellationToken);
        return paymentMongo?.ToPayment();
    }
}

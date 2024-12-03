using Infrastructure.MongoModels;

namespace Infrastructure.Repositories.Interfaces;

public interface IPaymentMongoDbRepository
{
    Task<PaymentMongoModel> CreateAsync(PaymentMongoModel orderMongoModel, CancellationToken cancellationToken);
    Task<PaymentMongoModel?> GetAsync(string id, CancellationToken cancellationToken);
    Task<PaymentMongoModel?> GetByExternalIdAsync(int id, CancellationToken cancellationToken);
    Task<PaymentMongoModel> ReplaceOneAsync(PaymentMongoModel orderMongoModel, CancellationToken cancellationToken);
    Task<PaymentMongoModel?> GetByProviderPaymentIdAsync(string id, CancellationToken cancellationToken);
}

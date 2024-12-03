using Infrastructure.Context.Interfaces;
using Infrastructure.MongoModels;
using Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class PaymentMongoDbRepository : IPaymentMongoDbRepository
{
    private readonly IMongoCollection<PaymentMongoModel> _collection;

    public PaymentMongoDbRepository(IMongoContext mongoContext)
    {
        _collection = mongoContext.GetCollection<PaymentMongoModel>();
    }

    public async Task<PaymentMongoModel> CreateAsync(PaymentMongoModel orderMongoModel, CancellationToken cancellationToken)
    {

        await _collection.InsertOneAsync(orderMongoModel, default, cancellationToken);

        return orderMongoModel;
    }

    public async Task<PaymentMongoModel?> GetAsync(string id, CancellationToken cancellationToken)
    {

        var filters = Builders<PaymentMongoModel>
            .Filter
            .Eq(x => x.Id, id);

        var response = await _collection
            .Find(filters)
            .FirstOrDefaultAsync(cancellationToken);

        return
            response;
    }

    public async Task<PaymentMongoModel?> GetByExternalIdAsync(int id, CancellationToken cancellationToken)
    {

        var filters = Builders<PaymentMongoModel>
            .Filter
            .Eq(x => x.ExternalId, id);

        var response = await _collection
            .Find(filters)
            .FirstOrDefaultAsync(cancellationToken);

        return
            response;
    }

    public async Task<PaymentMongoModel?> GetByProviderPaymentIdAsync(string id, CancellationToken cancellationToken)
    {

        var filters = Builders<PaymentMongoModel>
            .Filter
            .Eq(x => x.ProviderPaymentId, id);

        var response = await _collection
            .Find(filters)
            .FirstOrDefaultAsync(cancellationToken);

        return
            response;
    }

    public async Task<PaymentMongoModel> ReplaceOneAsync(PaymentMongoModel orderMongoModel, CancellationToken cancellationToken)
    {
        var filters = Builders<PaymentMongoModel>
            .Filter
            .Eq(x => x.Id, orderMongoModel.Id);

        var options = new FindOneAndReplaceOptions<PaymentMongoModel>()
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false
        };

        var replacedOrder = await _collection.FindOneAndReplaceAsync(filters, orderMongoModel, options, cancellationToken);

        return replacedOrder;
    }
}

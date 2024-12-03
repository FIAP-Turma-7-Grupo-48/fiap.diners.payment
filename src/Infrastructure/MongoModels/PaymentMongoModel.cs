using Domain.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.MongoModels;

[ExcludeFromCodeCoverage]
[BsonIgnoreExtraElements]
[BsonDiscriminator("payment")]
public class PaymentMongoModel
{

    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public PaymentStatus Status { get; set; }
    [BsonRepresentation(BsonType.String)]
    public required int ExternalId { get; set; }
    public PaymentProvider Provider { get; set; }
    public PaymentMethodKind Kind { get; set; }
    public string? ProviderPaymentId { get; set; }
    public decimal Amount { get; set; }
    public string? ImageFileName { get; set; }
    public string? ImageContentType { get; set; }
    public byte[]? ImageData { get; set; }
    public DateTime CreatedAt { get; set; }
}

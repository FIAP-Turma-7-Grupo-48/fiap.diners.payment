using Domain.Entities.Base.Interfaces;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities.PaymentAggregate;

public class Payment : IAggregateRoot
{
    public Payment(string providerPaymentId, PaymentStatus status, DateTime createdAt)
    {
        _providerPaymentId = providerPaymentId;
        Status = status;
        CreatedAt = createdAt;
    }
    public Payment()
    {
        Status = PaymentStatus.Creating;
        CreatedAt = DateTime.Now;
    }
    public string Id { get; init; } = string.Empty;
    private PaymentStatus _status;
    public PaymentStatus Status
    {
        get => _status;
        private set
        {
            _status = value;
        }
    }
    public required PaymentMethod PaymentMethod { get; init; }
    public required int ExternalId { get; init; }
    private string? _providerPaymentId;
    public string? ProviderPaymentId
    {
        get => _providerPaymentId;
        set
        {
            UnableToChangePaymentProiderIdException.ThrowIfUnableToChangeStatus(Status, PaymentStatus.Creating);
            _providerPaymentId = value;
            Status = PaymentStatus.Pending;
        }
    }
    public required decimal Amount { get; init; }
    public Photo? Image { get; set; }
    public DateTime CreatedAt { get; init; }

    public void SetStatusPaid()
    {
        UnableToChangePaymentStatusToPaidException.ThrowIfUnableToChangeStatus(Status);
        Status = PaymentStatus.Paid;
    }
}

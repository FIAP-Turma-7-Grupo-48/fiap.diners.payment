using Domain.Entities.Base.Interfaces;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities.PaymentAggregate;

public class Payment : IAggregateRoot
{
    public Payment(PaymentStatus status, DateTime createdAt)
    {
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
            UnableToChangePaymentStatusException.ThrowIfUnableToChangeStatus(_status);
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
        UnableToChangePaymentProiderIdException.ThrowIfUnableToChangeStatus(Status, PaymentStatus.Pending);
        Status = PaymentStatus.Paid;
    }
}

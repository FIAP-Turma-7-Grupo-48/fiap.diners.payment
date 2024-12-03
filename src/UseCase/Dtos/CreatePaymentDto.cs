using Domain.ValueObjects;

namespace UseCase.Dtos;

public record CreatePaymentDto
{
    public int ExternalId { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public decimal Price { get; init; }
}

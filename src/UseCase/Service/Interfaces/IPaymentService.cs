using Domain.ValueObjects;
using UseCase.Dtos;

namespace UseCase.Service.Interfaces;

public interface IPaymentService
{
    Task CreatePaymentAsync(CreatePaymentDto createPaymentDto, CancellationToken cancellationToken);
    Task<Photo?> GetImage(int ExternalId, CancellationToken cancellationToken);
}

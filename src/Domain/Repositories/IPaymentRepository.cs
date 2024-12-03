using Domain.Entities.PaymentAggregate;

namespace Domain.Repositories;

public interface IPaymentRepository
{
    Task<Payment> CreateAsync(Payment order, CancellationToken cancellationToken);
    Task UpdateAsync(Payment order, CancellationToken cancellationToken);
    Task<Payment?> GetByExternalIdAsync(int id, CancellationToken cancellationToken);
    Task<Payment?> GetByproviderPaymentIdAsync(string providerPaymentId, CancellationToken cancellationToken);
}

using Domain.Clients;
using Domain.Entities.PaymentAggregate;
using Domain.Gateways;
using Domain.Repositories;
using Domain.ValueObjects;
using UseCase.Dtos;
using UseCase.Service.Exception;
using UseCase.Service.Interfaces;

namespace UseCase.Service;

public class PaymentService : IPaymentService
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IConfirmPaymentClient _confirmPaymentClient;
    public PaymentService(IPaymentGateway paymentGateway,
        IPaymentRepository paymentRepository,
        IConfirmPaymentClient confirmPaymentClient)
    {
        _paymentGateway = paymentGateway;
        _paymentRepository = paymentRepository;
        _confirmPaymentClient = confirmPaymentClient;
    }
    public async Task<string> CreatePaymentAsync(CreatePaymentDto createPaymentDto, CancellationToken cancellationToken)
    {
        Payment payment = new Payment()
        {
            ExternalId = createPaymentDto.ExternalId,
            PaymentMethod = createPaymentDto.PaymentMethod,
            Amount = createPaymentDto.Price
        };

        payment = await _paymentRepository.CreateAsync(payment, cancellationToken);

        await _paymentGateway.CreatePayment(payment, cancellationToken);

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        return payment.Id;
    }

    public async Task<Photo?> GetImage(int externalId, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByExternalIdAsync(externalId, cancellationToken);
        return payment?.Image;
    }


    public async Task ConfirmPaymentAsync(string providerPaymentId, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByproviderPaymentIdAsync(providerPaymentId, cancellationToken);

        ConfirmPaymentNotFoundException.ThrowIfNull(payment, providerPaymentId);

        payment!.SetStatusPaid();

        await Task.WhenAll(
            _confirmPaymentClient.SendAsync(payment, cancellationToken),
            _paymentRepository.UpdateAsync(payment, cancellationToken)
        );
    }
}

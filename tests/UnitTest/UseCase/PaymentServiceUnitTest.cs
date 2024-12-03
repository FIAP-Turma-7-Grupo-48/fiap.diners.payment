using Domain.Clients;
using Domain.Gateways;
using Domain.Repositories;
using Moq;
using UseCase.Service;

namespace UnitTest.UseCase;

internal class PaymentServiceUnitTest
{
    private readonly PaymentService _paymentService;

    private readonly Mock<IPaymentGateway> _paymentGateway;
    private readonly Mock<IPaymentRepository> _paymentRepository;
    private readonly Mock<IConfirmPaymentClient> _confirmPaymentClient;

    public PaymentServiceUnitTest()
    {
        _paymentGateway = new Mock<IPaymentGateway>();
        _paymentRepository = new Mock<IPaymentRepository>();
        _confirmPaymentClient = new Mock<IConfirmPaymentClient>();

        _paymentService = new PaymentService(
            _paymentGateway.Object,
            _paymentRepository.Object,
            _confirmPaymentClient.Object
            );
    }


}

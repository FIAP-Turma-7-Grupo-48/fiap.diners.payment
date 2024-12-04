using Castle.Core.Resource;
using Domain.Clients;
using Domain.Entities.Enums;
using Domain.Entities.PaymentAggregate;
using Domain.Gateways;
using Domain.Repositories;
using Domain.ValueObjects;
using Moq;
using UseCase.Dtos;
using UseCase.Service;
using UseCase.Service.Exception;

namespace UnitTest.UseCase;

public class PaymentServiceUnitTest
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

    [Fact]
    public async void ShouldCreatePayment()
    {
        CreatePaymentDto payment = new()
        {
            Price = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1
        };

        Payment paymentResponse = new(Guid.NewGuid().ToString(), PaymentStatus.Pending, DateTime.Now)
        {
            Amount = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1
        };

        _paymentRepository.Setup(x => x.CreateAsync(It.IsAny<Payment>(), default)).ReturnsAsync(paymentResponse);

        var result = await _paymentService.CreatePaymentAsync(payment, CancellationToken.None);

        Assert.Equal(paymentResponse.Id, result);


    }
    [Fact]
    public async void ShouldConfirmPayment()
    {
        string providerPaymentId = Guid.NewGuid().ToString();
        Payment paymentResponse = new(providerPaymentId, PaymentStatus.Pending, DateTime.Now)
        {
            Amount = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1
        };

        _paymentRepository.Setup(x => x.GetByproviderPaymentIdAsync(providerPaymentId, default)).ReturnsAsync(paymentResponse);

        await _paymentService.ConfirmPaymentAsync(providerPaymentId, default);
        Assert.True(true);

    }
    [Fact]
    public async void ConfirmPaymentNotFound()
    {
        string providerPaymentId = Guid.NewGuid().ToString();
        Payment paymentResponse = new(providerPaymentId, PaymentStatus.Pending, DateTime.Now)
        {
            Amount = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1
        };

        _paymentRepository.Setup(x => x.GetByproviderPaymentIdAsync("12", default)).ReturnsAsync(paymentResponse);


        await Assert.ThrowsAsync<ConfirmPaymentNotFoundException>(() =>  _paymentService.ConfirmPaymentAsync(providerPaymentId, default));

    }


    [Fact]
    public async void GetImageFoundWithImageNull()
    {
        string providerPaymentId = Guid.NewGuid().ToString();
        Payment paymentResponse = new(providerPaymentId, PaymentStatus.Pending, DateTime.Now)
        {
            Amount = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1
        };

        _paymentRepository.Setup(x => x.GetByExternalIdAsync(1, default)).ReturnsAsync(paymentResponse);


        var image = await _paymentService.GetImage(paymentResponse.ExternalId, default);

        Assert.Null(image);

    }

    [Fact]
    public async void GetImagePaymentNotFound()
    {
        string providerPaymentId = Guid.NewGuid().ToString();
        Payment paymentResponse = new(providerPaymentId, PaymentStatus.Pending, DateTime.Now)
        {
            Amount = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1
        };

        _paymentRepository.Setup(x => x.GetByExternalIdAsync(2, default)).ReturnsAsync(paymentResponse);


        var image = await _paymentService.GetImage(paymentResponse.ExternalId, default);

        Assert.Null(image);

    }

    [Fact]
    public async void GetImageFound()
    {
        string providerPaymentId = Guid.NewGuid().ToString();
        Payment paymentResponse = new(providerPaymentId, PaymentStatus.Pending, DateTime.Now)
        {
            Amount = 1,
            PaymentMethod = new PaymentMethod(PaymentProvider.MercadoPago, PaymentMethodKind.Pix),
            ExternalId = 1,
            Image = new Photo("img.jpg", "texto", [])
        };

        _paymentRepository.Setup(x => x.GetByExternalIdAsync(1, default)).ReturnsAsync(paymentResponse);


        var image = await _paymentService.GetImage(paymentResponse.ExternalId, default);

        Assert.NotNull(image);

    }
}

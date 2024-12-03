using UseCase.Service;
using UseCase.Service.Interfaces;

namespace WebApi.Extensions;

public static class UseCaseExtension
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        return
            services
                .AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPaymentService, PaymentService>();
    }


}

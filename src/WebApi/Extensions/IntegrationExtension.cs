using Domain.Gateways;
using Integration.Gateway;
using Integration.Strategies;
using Integration.Strategies.Interface;

namespace WebApi.Extensions;

public static class IntegrationExtension
{
    public static IServiceCollection AddIntegration(this IServiceCollection services)
    {
        return
            services.AddStrategies()
                .AddResolver()
                .AddGateway();
    }

    private static IServiceCollection AddGateway(this IServiceCollection services)
    {
        return
            services.AddSingleton<IPaymentGateway, PaymentGateway>();
    }

    private static IServiceCollection AddResolver(this IServiceCollection services)
    {
        return
            services.AddSingleton<IPaymentStrategyResolver, PaymentStrategyResolver>();
    }

    private static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        return
            services.AddSingleton<IPaymentStrategy, MercadoPagoPixStrategy>();
    }
}

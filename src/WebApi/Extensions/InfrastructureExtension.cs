using Domain.Clients;
using Domain.Repositories;
using Infrastructure.Adapters;
using Infrastructure.Clients.RabbbitMq;
using Infrastructure.Context;
using Infrastructure.Context.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using RabbitMQ.Client;

namespace WebApi.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return
            services
                .AddContext()
                .AddRepository()
                .AddAdapters()
                .AddClients()
                .AddRabbitMqConnectionFactory();
    }

    private static IServiceCollection AddContext(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("MongoConnectionString");
        var dbName = Environment.GetEnvironmentVariable("dinersPayment");
        return
            services.AddSingleton<IMongoContext>(new MongoContext(connectionString, dbName));
    }

    private static IServiceCollection AddRepository(this IServiceCollection services)
    {
        return
            services.AddSingleton<IPaymentMongoDbRepository, PaymentMongoDbRepository>();
    }

    private static IServiceCollection AddAdapters(this IServiceCollection services)
    {
        return
            services.AddSingleton<IPaymentRepository, PaymentRepositoryAdapter>();
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        return
        services
                .AddSingleton<IConfirmPaymentClient, ConfirmPaymentRabbitMqClient>();
    }

    private static IServiceCollection AddRabbitMqConnectionFactory(this IServiceCollection services)
    {

        var hostName = Environment.GetEnvironmentVariable("RabbitMqHostName");
        var port = int.Parse(Environment.GetEnvironmentVariable("RabbitMqPort"));
        var user = Environment.GetEnvironmentVariable("RabbitMqUserName");
        var password = Environment.GetEnvironmentVariable("RabbitMqPassword");

        return
            services
                .AddSingleton<IConnectionFactory>(
                    new ConnectionFactory()
                    {
                        HostName = hostName,
                        Port = port,
                        UserName = user,
                        Password = password
                    }
                );
    }
}

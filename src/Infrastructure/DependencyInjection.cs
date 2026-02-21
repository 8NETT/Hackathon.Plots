using Application.Messaging;
using Application.Persistence;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;


namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddPersistence(configuration)
            .AddMessaging(configuration);

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("ConnectionString não localizada na configuração.");

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

        services.AddScoped<IPropriedadeRepository, PropriedadeRepository>();
        services.AddScoped<ITalhaoRepository, TalhaoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var eventHubConnectionString = configuration.GetConnectionString("EventHub");

        services.AddSingleton(new EventHubProducerClient(eventHubConnectionString));
        services.AddScoped<IEventPublisher, AzureEventHubPublisher>();

        return services;
    }
}

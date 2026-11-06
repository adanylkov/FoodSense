namespace FoodSense.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddOpenFoodFactsClient(this IServiceCollection services)
    {
        services.AddScoped<IOpenFoodFactsClient, LocalOpenFoodFactsClient>();

        return services;
    }
}

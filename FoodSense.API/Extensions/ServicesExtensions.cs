using OpenFoodFactsCSharp.Services;
using OpenFoodFactsCSharp.Services.Interfaces;

namespace FoodSense.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddOpenFoodFactsClient(this IServiceCollection services)
    {
        services.AddSingleton<IOpenFoodFactsWrapper, OpenFoodFactsWrapperImpl>(implementationFactory =>
        {
            var client = new HttpClient();
            return new OpenFoodFactsWrapperImpl(new OpenFoodFactsCSharp.Clients.OpenFoodFactsApiLowLevelClient(client));
        });

        return services;
    }
}

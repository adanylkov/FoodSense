namespace FoodSense.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddOpenFoodFactsClient(this IServiceCollection services)
    {
        services.AddHttpClient<IOpenFoodFactsClient, OpenFoodFactsClient>(client =>
        {
            client.BaseAddress = new Uri("https://world.openfoodfacts.org/");
            client.DefaultRequestHeaders.Add("User-Agent", "MyFoodApp/1.0 (danilkovxp@gmail.com)");
        });

        return services;
    }
}

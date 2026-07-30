var builder = DistributedApplication.CreateBuilder(args);

var localDb = builder.AddConnectionString("db");

var api = builder.AddProject<Projects.FoodSense_API>("foodsense-api")
    .WithReference(localDb);

builder.AddProject<Projects.FoodSense_BlazorFrontend>("blazor-frontend")
    .WithReference(api)
    .WithEnvironment("ApiBaseUrl", api.GetEndpoint("http"))
    .WithExternalHttpEndpoints();

builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("sql")
    .AddDatabase("foodsense");

var api = builder.AddProject<Projects.FoodSense_API>("foodsense-api")
    .WithReference(db);

builder.AddProject<Projects.FoodSense_BlazorFrontend>("blazor-frontend")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();

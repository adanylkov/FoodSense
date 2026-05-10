var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("sql")
    .AddDatabase("foodsense");

builder.AddProject<Projects.FoodSense_API>("foodsense-api")
    .WithReference(db);

builder.Build().Run();

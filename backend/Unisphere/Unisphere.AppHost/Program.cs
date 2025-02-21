var builder = DistributedApplication.CreateBuilder(args);

var explorer = builder.AddProject<Projects.Unisphere_Explorer_Api>("explorer");

builder.AddProject<Projects.Unisphere_Gateway_Api>("gateway")
    .WithReference(explorer);

var app = builder.Build();

await app.RunAsync();

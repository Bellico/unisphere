var builder = DistributedApplication.CreateBuilder(args);

var explorer = builder.AddProject<Projects.Unisphere_Explorer_Api>("explorer");

// dotnet user-secrets set Parameters:sql-password 2chats1Tigre343434
var sqlPassword = builder.AddParameter("sql-password", "2chats1Tigre343434");

var sqlServerGateway = builder.AddSqlServer("sql-gateway", sqlPassword)
    .WithDataVolume();

var sqlDatabaseOpenIddict = sqlServerGateway.AddDatabase("sql-openiddict");

builder.AddProject<Projects.Unisphere_Gateway_Api>("gateway")
    .WithReference(explorer)
    .WithReference(sqlDatabaseOpenIddict)
    .WaitFor(sqlServerGateway);

var app = builder.Build();

await app.RunAsync();

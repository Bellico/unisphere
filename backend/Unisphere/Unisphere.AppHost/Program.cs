var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithContainerName("postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var sqlServer = builder.AddSqlServer("sqlServer")
    .WithDataVolume()
    .WithContainerName("sql-server")
    .WithLifetime(ContainerLifetime.Persistent);

// Explorer Service
var dbExplorer = postgres.AddDatabase("db-explorer");
var explorer = builder.AddProject<Projects.Unisphere_Explorer_Api>("explorer")
    .WithReference(dbExplorer)
    .WaitFor(dbExplorer);

// Gateway
var dbGateway = sqlServer.AddDatabase("db-gateway");
var gateway = builder.AddProject<Projects.Unisphere_Gateway_Api>("gateway")
    .WithReference(explorer)
    .WithReference(dbGateway)
    .WaitFor(dbGateway);

var app = builder.Build();

await app.RunAsync();

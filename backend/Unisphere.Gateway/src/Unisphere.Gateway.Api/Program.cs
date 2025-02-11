WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

//builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddEndpointsApiExplorer();
    //.AddSwaggerGen();

//builder.Services
//    .AddApplication()
//    .AddPresentation()
//    .AddInfrastructure(builder.Configuration);

//builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

//app.MapEndpoints();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwaggerWithUi();

//    app.ApplyMigrations();
//}

//app.MapHealthChecks("health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();

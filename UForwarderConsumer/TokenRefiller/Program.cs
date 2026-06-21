using StackExchange.Redis;
using TokenRefiller;
using TokenRefiller.Services.TokenRefillService;
using TokenRefiller.Services.TokenRefillService.Implementations;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    return ConnectionMultiplexer.Connect(
        configuration["Redis:ConnectionString"]!);
});
builder.Services.AddSingleton<ITokenRefillService, TokenRefillService>();

var host = builder.Build();
host.Run();

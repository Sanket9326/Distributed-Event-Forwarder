using StackExchange.Redis;
using UForwarderConsumer.Services.MessageProcessingService;
using UForwarderConsumer.Services.MessageProcessingService.Implementations;
using UForwarderConsumer.Services.RetryService;
using UForwarderConsumer.Services.RetryService.Implementations;
using UForwarderConsumer.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ConsumerWorker>();
builder.Services.AddSingleton<IMessageProcessor, MessageProcessor>();
builder.Services.AddSingleton<IRoutineEngine, RoutineEngine>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    return ConnectionMultiplexer.Connect(
        configuration["Redis:ConnectionString"]!);
});
builder.Services.AddSingleton<IRetryService, RetryService>();

var host = builder.Build();
host.Run();

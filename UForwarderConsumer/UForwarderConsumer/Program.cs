using Confluent.Kafka;
using StackExchange.Redis;
using UForwarderConsumer.Services.DlqService;
using UForwarderConsumer.Services.DlqService.Implementations;
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
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var config = new ProducerConfig
    {
        BootstrapServers = configuration["Kafka:BootstrapServers"]
    };

    return new ProducerBuilder<string, string>(config).Build();
});
builder.Services.AddSingleton<IRetryService, RetryService>();
builder.Services.AddSingleton<IDlqService, DlqService>();

var host = builder.Build();
host.Run();

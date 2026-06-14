using Confluent.Kafka;
using RetryWorker;
using RetryWorker.Services.KafkaProducerService;
using RetryWorker.Services.KafkaProducerService.Implementations;
using RetryWorker.Services.RetryService;
using RetryWorker.Services.RetryService.Implementations;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var config = new ProducerConfig
    {
        BootstrapServers = configuration["Kafka:BootstrapServers"]
    };

    return new ProducerBuilder<string, string>(config).Build();
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    return ConnectionMultiplexer.Connect(
        configuration["Redis:ConnectionString"]!);
});
builder.Services.AddSingleton<IRetryService, RetryService>();
builder.Services.AddSingleton<IProducerService, ProducerService>();

var host = builder.Build();
host.Run();

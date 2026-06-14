using UForwarderConsumer;
using UForwarderConsumer.Services;
using UForwarderConsumer.Services.Implementations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IMessageProcessor, MessageProcessor>();
builder.Services.AddSingleton<IRoutineEngine, RoutineEngine>();

var host = builder.Build();
host.Run();

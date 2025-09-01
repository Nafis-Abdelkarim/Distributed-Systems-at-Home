using GrpcServiceA.Interfaces;
using GrpcServiceA.Messaging;
using GrpcServiceA.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(50051, listenOptions =>
    {
        // Allow both HTTP/1.1 (for browsers) and HTTP/2 (for gRPC clients)
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<IConnection>(sp =>
{
    var config = builder.Configuration.GetSection("RabbitMq");
    var factory = new ConnectionFactory()
    {
        HostName = config["HostName"],
        Port = int.Parse(config["Port"]), //TODO
        UserName = config["UserName"],
        Password = config["Password"],
        DispatchConsumersAsync = true
    };
    return factory.CreateConnection();
});

builder.Services.AddTransient<IMessageQueue, RabbitMqService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<SumService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

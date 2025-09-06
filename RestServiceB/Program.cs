using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestServiceB.NewFolder;
using RestServiceB.ServicesB;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnection>(sp =>
{
    try
    {
        var config = builder.Configuration.GetSection("RabbitMq");
        var factory = new ConnectionFactory()
        {
            HostName = config["HostName"],
            Port = int.Parse(config["Port"]),
            UserName = config["UserName"],
            Password = config["Password"],
            DispatchConsumersAsync = true
        };
        var connection = factory.CreateConnection();
        connection.ConnectionShutdown += (sender, e) =>
        {
            sp.GetRequiredService<ILogger<Program>>()
              .LogWarning("RabbitMQ connection shut down: {ReplyText}", e.ReplyText);
        };
        return connection;
    }
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to create RabbitMQ connection");
        throw;
    }
});

builder.Services.AddSingleton<IMessageQueue, RabbitMQueueService>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add this temporary endpoint to test the connection
app.MapGet("/test-rabbitmq", (IConnection connection) =>
{
    return $"RabbitMQ connected: {connection.IsOpen}";
});

app.MapGet("/getSumResult", async (IMessageQueue messageQueueServices) =>
{
    //get result from the queue 
    string? message = await messageQueueServices.GetLatestMessageFromQueueAsync();

    if (message == null || !int.TryParse(message, out var newValue))
    {
        return Results.NotFound(new { message = "No valid message in queue" });
    }

    return Results.Ok(message);


});


app.Run();

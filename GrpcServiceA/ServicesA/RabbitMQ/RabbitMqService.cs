using GrpcServiceA.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace GrpcServiceA.Messaging;

public class RabbitMqService(IConnection messageQueueConnection, ILogger<RabbitMqService> logger) : IMessageQueue
{
    private readonly IConnection _messageQueueConnection = messageQueueConnection;
    private readonly ILogger<RabbitMqService> _logger = logger;

    public void PublishResult(string message)
    {
        using var channel = _messageQueueConnection.CreateModel();
        channel.QueueDeclare(queue: "calculation_results",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: "calculation_results",
                             basicProperties: null,
                             body: body);

        _logger.LogInformation($"Published result to RabbitMQ: {message}");
    }
}

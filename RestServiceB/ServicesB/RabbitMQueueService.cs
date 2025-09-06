using RabbitMQ.Client;
using RestServiceB.NewFolder;
using System.Text;

namespace RestServiceB.ServicesB
{
    public class RabbitMQueueService : IMessageQueue, IDisposable
    {
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMQueueService> _logger;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQueueService(IConnection connection, ILogger<RabbitMQueueService> logger)
        {
            _connection = connection;
            _logger = logger;
            _channel = _connection.CreateModel();
            _queueName = "calculation_results";
            _channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public async Task<string?> GetLatestMessageFromQueueAsync()
        {
            var result = _channel.BasicGet(_queueName, autoAck: true);
            if(result == null)
            {
                return null;
            }
            var body = result.Body.ToArray();
            return Encoding.UTF8.GetString(body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}

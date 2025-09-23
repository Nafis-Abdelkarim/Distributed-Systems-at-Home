namespace GrpcServiceA.ServicesA.Outbox
{
    public class OutboxBackgroundService(ILogger<OutboxBackgroundService> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<OutboxBackgroundService> _logger = logger;
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;
        private const int OutboxProcessorFrequency = 7;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Starting Outbox Background Service...");

                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var outboxProcessor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();

                    await outboxProcessor.Execute(stoppingToken);

                    //Simulate running Outbox processing every N seconds
                    await Task.Delay(TimeSpan.FromSeconds(OutboxProcessorFrequency), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Log the exception (you can use any logging framework you prefer)
                _logger.LogInformation("Outbox Background Service is stopping due to cancellation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Outbox Background Service.");
            }
            finally
            {
                _logger.LogInformation("Outbox Background Service finished...");
            }
        }
    }
}

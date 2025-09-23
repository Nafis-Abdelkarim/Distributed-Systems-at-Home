using GrpcServiceA.Database.Migration;
using GrpcServiceA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceA.ServicesA.Outbox
{
    public class OutboxProcessor(AppDbContext context, IMessageQueue messageQueue)
    {
        private readonly AppDbContext _context = context;
        private readonly IMessageQueue _messageQueue = messageQueue;

        public async Task<int> Execute(CancellationToken cancellationToken = default)
        {
            //Select the batch from the outbox table
            var messges = _context.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(5) //set it into the confiaguration
                .ToList();

            foreach (var message in messges)
            {
                try
                {
                    //Publish each message to RabbitMQ
                    _messageQueue.PublishResult(message.Content);

                    //Mark each message as processed
                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.Error = ex.Message;
                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            return _context.OutboxMessages.Count(m => m.ProcessedOnUtc != null);
        }
    }
}

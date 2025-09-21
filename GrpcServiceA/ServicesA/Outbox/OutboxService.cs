using GrpcServiceA.Database;
using GrpcServiceA.Database.Entities;
using GrpcServiceA.Database.Migration;
using GrpcServiceA.InterfacesA;
using Npgsql;
using System.Text.Json;

namespace GrpcServiceA.ServicesA.Outbox
{
    public class OutboxService : IOutboxService
    {
        private readonly AppDbContext _context;
        public async Task<OutboxMessage> CreateOutboxMessageAsync(OutboxMessage outboxMessage)
        {
            _context.OutboxMessages.Add(outboxMessage);
            await _context.SaveChangesAsync();
            return outboxMessage;
        }

        public Task<OutboxMessage?> GetOutboxMessageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOutboxMessageAsync(OutboxMessage outboxMessage)
        {
            throw new NotImplementedException();
        }
    }
}

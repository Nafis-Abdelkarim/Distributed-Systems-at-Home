using GrpcServiceA.Database.Entities;

namespace GrpcServiceA.InterfacesA
{
    public interface IOutboxService 
    {
        Task<OutboxMessage?> GetOutboxMessageByIdAsync(Guid id);
        Task<OutboxMessage> CreateOutboxMessageAsync(OutboxMessage outboxMessage);
        Task UpdateOutboxMessageAsync(OutboxMessage outboxMessage);
    }
}

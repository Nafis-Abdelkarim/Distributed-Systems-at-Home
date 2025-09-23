namespace GrpcServiceA.Database.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public byte Type { get; set; }

        // JSONB column → use string or JsonDocument
        public string Content { get; set; } = string.Empty;

        public DateTime OccurredOnUtc { get; set; }

        public DateTime? ProcessedOnUtc { get; set; }

        public string? Error { get; set; }
    }
}

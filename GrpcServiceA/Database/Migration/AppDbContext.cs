using GrpcServiceA.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceA.Database.Migration
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        // Define your DbSets here
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define your entity configurations here
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .HasColumnType("jsonb")
                    .IsRequired();

                entity.Property(e => e.OccurredOnUtc)
                    .HasColumnType("timestamptz")
                    .IsRequired();

                entity.Property(e => e.ProcessedOnUtc)
                    .HasColumnType("timestamptz");
            });
        }
    }
}

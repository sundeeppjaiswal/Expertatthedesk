using ExpertService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ExpertService.Data;

public class ExpertDbContext : DbContext
{
    public ExpertDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Expert> Experts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}

using ExpertService.Data;
using ExpertService.Entities;
using Contracts;
using MassTransit;

namespace ExpertService;

public class ExpertFinishedConsumer : IConsumer<ExpertFinished>
{
    private readonly ExpertDbContext _dbContext;

    public ExpertFinishedConsumer(ExpertDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ExpertFinished> context)
    {
        Console.WriteLine("--> Consuming Expert finished");

        var Expert = await _dbContext.Experts.FindAsync(Guid.Parse(context.Message.ExpertId));

        if (context.Message.ItemSold)
        {
            Expert.ExpertName = context.Message.Winner;
            Expert.Topics.TopicName = context.Message.ExpertId;
        }

        Expert.Status = Expert.Available > 0
            ? Status.Online : Status.Offline;

        await _dbContext.SaveChangesAsync();
    }
}

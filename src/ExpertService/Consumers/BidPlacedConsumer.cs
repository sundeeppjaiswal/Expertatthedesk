using ExpertService.Data;
using Contracts;
using MassTransit;

namespace ExpertService;

public class BidPlacedConsumer : IConsumer<SelectingPlaced>
{
    private readonly ExpertDbContext _dbContext;

    public BidPlacedConsumer(ExpertDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<SelectingPlaced> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        var Expert = await _dbContext.Experts.FindAsync(Guid.Parse(context.Message.ExpertId));

        if (Expert.CurrentlyConsulting == null 
            || context.Message.Selectingtatus.Contains("Accepted") 
            && context.Message.User == Expert.CurrentlyConsulting)
        {
            Expert.CurrentlyConsulting = context.Message.User;
            await _dbContext.SaveChangesAsync();
        }
    }
}

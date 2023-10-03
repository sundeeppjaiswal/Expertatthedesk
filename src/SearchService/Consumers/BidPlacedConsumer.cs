using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class BidPlacedConsumer : IConsumer<SelectingPlaced>
{
    public async Task Consume(ConsumeContext<SelectingPlaced> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        var Expert = await DB.Find<Item>().OneAsync(context.Message.ExpertId);

        if (context.Message.Selectingtatus.Contains("Accepted") 
            && context.Message.Amount > Expert.CurrentHighBid)
        {
            Expert.CurrentHighBid = context.Message.Amount;
            await Expert.SaveAsync();
        }
    }
}

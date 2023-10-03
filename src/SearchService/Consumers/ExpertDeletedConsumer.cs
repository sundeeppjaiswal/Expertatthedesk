using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class ExpertDeletedConsumer : IConsumer<ExpertDeleted>
{
    public async Task Consume(ConsumeContext<ExpertDeleted> context)
    {
        Console.WriteLine("--> Consuming ExpertDeleted: " + context.Message.Id);

        var result = await DB.DeleteAsync<Item>(context.Message.Id);

        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(ExpertDeleted), "Problem deleting Expert");
    }
}

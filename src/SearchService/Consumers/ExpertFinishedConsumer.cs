using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class ExpertFinishedConsumer : IConsumer<ExpertFinished>
{
    public async Task Consume(ConsumeContext<ExpertFinished> context)
    {
        var Expert = await DB.Find<Item>().OneAsync(context.Message.ExpertId);

        if (context.Message.Available == 1)
        {
            Expert.ID = context.Message.ExpertId;
            Expert.ExpertName = context.Message.ExpertName;
        }

        Expert.Status = "Finished";

        await Expert.SaveAsync();
    }
}

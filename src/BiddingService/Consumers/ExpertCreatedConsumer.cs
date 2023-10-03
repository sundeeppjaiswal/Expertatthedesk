using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SelectingService;

public class ExpertCreatedConsumer : IConsumer<ExpertCreated>
{
    public async Task Consume(ConsumeContext<ExpertCreated> context)
    {
        var Expert = new Expert
        {
            ID = context.Message.Id.ToString(),
            ExpertName = context.Message.ExpertName,
            Available = context.Message.Available,
            Accepted = context.Message.Status
        };

        await Expert.SaveAsync();
    }
}

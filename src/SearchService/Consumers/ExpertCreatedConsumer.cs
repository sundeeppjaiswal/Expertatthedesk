using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class ExpertCreatedConsumer : IConsumer<ExpertCreated>
{
    private readonly IMapper _mapper;

    public ExpertCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ExpertCreated> context)
    {
        Console.WriteLine("--> Consuming Expert created: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");

        await item.SaveAsync();
    }
}

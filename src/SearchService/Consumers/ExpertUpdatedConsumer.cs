﻿using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class ExpertUpdatedConsumer : IConsumer<ExpertUpdated>
{
    private readonly IMapper _mapper;

    public ExpertUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<ExpertUpdated> context)
    {
        Console.WriteLine("--> Consuming Expert updated: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
            .Match(a => a.ID == context.Message.Id)
            .ModifyOnly(x => new
            {
                x.Color,
                x.Make,
                x.Model,
                x.Year,
                x.Mileage
            }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(ExpertUpdated), "Problem updating mongodb");
    }
}

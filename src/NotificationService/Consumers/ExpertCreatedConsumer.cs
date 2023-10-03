using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class ExpertCreatedConsumer : IConsumer<ExpertCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public ExpertCreatedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<ExpertCreated> context)
    {
        Console.WriteLine("--> Expert created message received");

        await _hubContext.Clients.All.SendAsync("ExpertCreated", context.Message);
    }
}

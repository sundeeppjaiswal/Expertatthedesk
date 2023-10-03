using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class ExpertFinishedConsumer : IConsumer<ExpertFinished>
{ 
    private readonly IHubContext<NotificationHub> _hubContext;

    public ExpertFinishedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<ExpertFinished> context)
    {
        Console.WriteLine("--> Expert finished message received");

        await _hubContext.Clients.All.SendAsync("ExpertFinished", context.Message);
    }
}

using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class BidPlacedConsumer : IConsumer<SelectingPlaced>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<SelectingPlaced> context)
    {
        Console.WriteLine("--> bid placed message received");

        await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}

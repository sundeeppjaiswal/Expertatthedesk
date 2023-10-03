using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SelectingService;

public class CheckExpertFinished : BackgroundService
{
    private readonly ILogger<CheckExpertFinished> _logger;
    private readonly IServiceProvider _services;

    public CheckExpertFinished(ILogger<CheckExpertFinished> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting check for finished Experts");

        stoppingToken.Register(() => _logger.LogInformation("==> Expert check is stopping"));

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckExperts(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task CheckExperts(CancellationToken stoppingToken)
    {
        var finishedExperts = await DB.Find<Expert>()
            .Match(x => x.ExpertEnd <= DateTime.UtcNow)
            .Match(x => !x.Finished)
            .ExecuteAsync(stoppingToken);
        
        if (finishedExperts.Count == 0) return;

        _logger.LogInformation("==> Found {count} Experts that have completed", finishedExperts.Count);

        using var scope = _services.CreateScope();
        var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        foreach (var Expert in finishedExperts)
        {
            Expert.Finished = true;
            await Expert.SaveAsync(null, stoppingToken);

            var winningBid = await DB.Find<Bid>()
                .Match(a => a.ExpertId == Expert.ID)
                .Match(b => b.Selectingtatus == Selectingtatus.Accepted)
                .Sort(x => x.Descending(s => s.Amount))
                .ExecuteFirstAsync(stoppingToken);

            await endpoint.Publish(new ExpertFinished
            {
                ItemSold = winningBid != null,
                ExpertId = Expert.ID,
                Winner = winningBid?.User,
                Amount = winningBid?.Amount,
                Seller = Expert.Seller
            }, stoppingToken);
        }
    }
}

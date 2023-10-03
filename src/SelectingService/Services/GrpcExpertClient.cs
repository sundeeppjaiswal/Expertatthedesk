using ExpertService;
using Grpc.Net.Client;

namespace SelectingService;

public class GrpcExpertClient
{
    private readonly ILogger<GrpcExpertClient> _logger;
    private readonly IConfiguration _config;

    public GrpcExpertClient(ILogger<GrpcExpertClient> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public Expert GetExpert(string id)
    {
        _logger.LogInformation("Calling GRPC Service");
        var channel = GrpcChannel.ForAddress(_config["GrpcExpert"]);
        var client = new GrpcExpert.GrpcExpertClient(channel);
        var request = new GetExpertRequest{Id = id};

        try
        {
            var reply = client.GetExpert(request);
            var Expert = new Expert
            {
                ID = reply.Expert.Id,
                ExpertName =  reply.Expert.ExpertName, //DateTime.Parse(reply.Expert.ExpertEnd),
                Available = reply.Expert.Available,
                Accepted = reply.Expert.Accepted
            };

            return Expert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not call GRPC Server");
            return null;
        }
    }
}

using ExpertService.Data;
using Grpc.Core;

namespace ExpertService;

public class GrpcExpertService : GrpcExpert.GrpcExpertBase
{
    private readonly ExpertDbContext _dbContext;

    public GrpcExpertService(ExpertDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<GrpcExpertResponse> GetExpert(GetExpertRequest request, 
        ServerCallContext context) 
    {
        Console.WriteLine("==> Received Grpc request for Expert");

        var Expert = await _dbContext.Experts.FindAsync(Guid.Parse(request.Id)) 
            ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
            
        var response = new GrpcExpertResponse
        {
            Expert = new GrpcExpertModel
            {
                ExpertEnd = Expert.ExpertEnd.ToString(),
                Id = Expert.Id.ToString(),
                ReservePrice = Expert.ReservePrice,
                Seller = Expert.Seller
            }
        };

        return response;
    }
}

using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SelectingService;

[ApiController]
[Route("api/[controller]")]
public class SelectingController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GrpcExpertClient _grpcClient;

    public SelectingController(IMapper mapper, IPublishEndpoint publishEndpoint,
        GrpcExpertClient grpcClient)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _grpcClient = grpcClient;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SelectingDto>> PlaceSelection(string ExpertId, string userSelectingTopics)
    {
        var Expert = await DB.Find<Expert>().OneAsync(ExpertId);

        if (Expert == null)
        {
            Expert = _grpcClient.GetExpert(ExpertId);

            if (Expert == null) return BadRequest("Cannot accept Selection on this Expert at this moment!");
        }

        if (Expert.ExpertName == User.Identity.Name)
        {
            return BadRequest("You cannot select on your own Expert");
        }

        var select = new Selecting
        {
            UserSelectingTopics = userSelectingTopics,
            ExpertId = ExpertId,
            User = User.Identity.Name
        };

        if (Expert.LastAdviseGivenAt < DateTime.Now)
        {
            select.SelectingStatus = SelectingStatus.Accepted;
        }
        else
        {
            var highSelect = await DB.Find<Selecting>()
                        .Match(a => a.ExpertId == ExpertId)
                        .Sort(b => b.Descending(x => x.UserSelectingTopics))
                        .ExecuteFirstAsync();

            if (highSelect != null && userSelectingTopics == highSelect.UserSelectingTopics || highSelect == null)
            {
                select.SelectingStatus = userSelectingTopics == Expert.Accepted
                    ? SelectingStatus.Accepted
                    : SelectingStatus.Decline;
            }

            if (highSelect != null && select.UserSelectTime < Expert.LastAdviseGivenAt)
            {
                select.SelectingStatus = SelectingStatus.Waiting;
            }
        }

        await DB.SaveAsync(select);

        await _publishEndpoint.Publish(_mapper.Map<SelectingPlaced>(select));

        return Ok(_mapper.Map<SelectingDto>(select));
    }

    [HttpGet("{ExpertId}")]
    public async Task<ActionResult<List<SelectingDto>>> GetSelectingForExpert(string ExpertId)
    {
        var Selecting = await DB.Find<Selecting>()
            .Match(a => a.ExpertId == ExpertId)
            .Sort(b => b.Descending(a => a.UserSelectTime))
            .ExecuteAsync();

        return Selecting.Select(_mapper.Map<SelectingDto>).ToList();
    }
}

using ExpertService.Data;
using ExpertService.DTOs;
using ExpertService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpertService.Controllers;

[ApiController]
[Route("api/Experts")]
public class ExpertsController : ControllerBase
{
    private readonly IExpertRepository _repo;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public ExpertsController(IExpertRepository repo, IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _repo = repo;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExpertDto>>> GetAllExperts(string date)
    {
        return await _repo.GetExpertsAsync(date);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpertDto>> GetExpertById(Guid id)
    {
        var Expert = await _repo.GetExpertByIdAsync(id);

        if (Expert == null) return NotFound();

        return Expert;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ExpertDto>> CreateExpert(CreateExpertDto ExpertDto)
    {
        var Expert = _mapper.Map<Expert>(ExpertDto);

        Expert.ExpertName = User.Identity.Name;

        _repo.AddExpert(Expert);

        var newExpert = _mapper.Map<ExpertDto>(Expert);

        await _publishEndpoint.Publish(_mapper.Map<ExpertCreated>(newExpert));

        var result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetExpertById),
            new { Expert.Id }, newExpert);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateExpert(Guid id, UpdateExpertDto updateExpertDto)
    {
        var Expert = await _repo.GetExpertEntityById(id);

        if (Expert == null) return NotFound();

        if (Expert.ExpertName != User.Identity.Name) return Forbid();

        Expert.Topics.TopicName = updateExpertDto.TopicName ?? Expert.Topics.TopicName;
        Expert.Topics.Description = updateExpertDto.Description ?? Expert.Topics.Description;
        Expert.Topics.TopicExp = updateExpertDto.TopicExp ?? Expert.Topics.TopicExp;
        Expert.Topics.ImageUrl = updateExpertDto.ImageUrl ?? Expert.Topics.ImageUrl;
       

        await _publishEndpoint.Publish(_mapper.Map<ExpertUpdated>(Expert));

        var result = await _repo.SaveChangesAsync();

        if (result) return Ok();

        return BadRequest("Problem saving changes");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExpert(Guid id)
    {
        var Expert = await _repo.GetExpertEntityById(id);

        if (Expert == null) return NotFound();

        if (Expert.ExpertName != User.Identity.Name) return Forbid();

        _repo.RemoveExpert(Expert);

        await _publishEndpoint.Publish<ExpertDeleted>(new { Id = Expert.Id.ToString() });

        var result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}

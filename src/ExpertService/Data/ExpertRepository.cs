using ExpertService.Data;
using ExpertService.DTOs;
using ExpertService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ExpertService;

public class ExpertRepository : IExpertRepository
{
    private readonly ExpertDbContext _context;
    private readonly IMapper _mapper;

    public ExpertRepository(ExpertDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddExpert(Expert Expert)
    {
        _context.Experts.Add(Expert);
    }

    public async Task<ExpertDto> GetExpertByIdAsync(Guid id)
    {
        return await _context.Experts
            .ProjectTo<ExpertDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Expert> GetExpertEntityById(Guid id)
    {
        return await _context.Experts
            .Include(x => x.Topics)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ExpertDto>> GetExpertsAsync(string date)
    {
        var query = _context.Experts.OrderBy(x => x.Topics.TopicName).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<ExpertDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public void RemoveExpert(Expert Expert)
    {
        _context.Experts.Remove(Expert);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}

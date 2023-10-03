using ExpertService.DTOs;
using ExpertService.Entities;

namespace ExpertService;

public interface IExpertRepository
{
    Task<List<ExpertDto>> GetExpertsAsync(string date);
    Task<ExpertDto> GetExpertByIdAsync(Guid id);
    Task<Expert> GetExpertEntityById(Guid id);
    void AddExpert(Expert Expert);
    void RemoveExpert(Expert Expert);
    Task<bool> SaveChangesAsync();
}

using AutoMapper;
using Contracts;

namespace SearchService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ExpertCreated, Item>();
        CreateMap<ExpertUpdated, Item>();
    }
}

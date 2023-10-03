using AutoMapper;
using Contracts;

namespace SelectingService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Selecting, SelectingDto>();
        CreateMap<Selecting, SelectingPlaced>();
    }
}

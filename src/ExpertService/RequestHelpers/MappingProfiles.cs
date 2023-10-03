using ExpertService.DTOs;
using ExpertService.Entities;
using AutoMapper;
using Contracts;

namespace ExpertService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Expert, ExpertDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, ExpertDto>();
        CreateMap<CreateExpertDto, Expert>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateExpertDto, Item>();
        CreateMap<ExpertDto, ExpertCreated>();
        CreateMap<Expert, ExpertUpdated>().IncludeMembers(a => a.Item);
        CreateMap<Item, ExpertUpdated>();
    }
}

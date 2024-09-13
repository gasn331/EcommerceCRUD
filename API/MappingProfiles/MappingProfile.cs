using AutoMapper;
using API.Models;
using Shared.DTOs;

namespace API.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Produto, ProdutoDTO>()
                .ForMember(dest => dest.Departamento, opt => opt.MapFrom(src => src.Departamento))
                .ReverseMap();
            CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}

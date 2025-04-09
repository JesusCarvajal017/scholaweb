using AutoMapper;
using Entity.DTOs;
using Entity.Model;

namespace Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.NameComplet, opt => opt.MapFrom(src => src.Name + " " + src.LastName))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Form, FormDto>().ReverseMap();


        }
    }
}

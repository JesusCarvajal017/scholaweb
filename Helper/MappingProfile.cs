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
            CreateMap<Module, ModuleDto>().ReverseMap();
            //CreateMap<ModuleForm, ModuleFormDto>().ReverseMap();

            CreateMap<ModuleForm, ModuleFormDto>()
             .ForMember(dest => dest.NameForm, opt => opt.MapFrom(src => src.Form.Name))
             .ForMember(dest => dest.NameModule, opt => opt.MapFrom(src => src.Module.Name))
             .ReverseMap();

            CreateMap<ModuleForm, ModuleFormCreateDto>().ReverseMap();

            CreateMap<Permission, PermissionDto>().ReverseMap();


            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<UserRol, UserRolDto>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
             .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
             .ReverseMap();

            CreateMap<UserRol, UserRolCreateDto>().ReverseMap();

            CreateMap<RolFormPermission, RolFormPermissionDto>()
             .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
             .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form.Name))
             .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name))

             .ReverseMap();

            CreateMap<RolFormPermission, RolFormPermissionCreateDto>().ReverseMap();


            //CreateMap<User, UserDto>()
            //    .ForMember(dest => dest.NameComplet, opt => opt.MapFrom(src => src.Name + " " + src.LastName))
            //    .ReverseMap()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        }
    }
}

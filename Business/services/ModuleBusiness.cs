using AutoMapper;
using Data.factories;
using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.services
{
    public class ModuleBusiness : GenericBusiness<Module, ModuleDto>
    {
        public ModuleBusiness
            (IDataFactoryGlobal factory, ILogger<Module> logger, IMapper mapper) : base(factory.CreateModuleData(), logger, mapper)
        {

        }

        protected override void Validate(ModuleDto moduleDto)
        {
            if (moduleDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(moduleDto.Name))
                throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }


    }
}

using AutoMapper;
using Data.factories;
using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.services
{
    public class RolBusiness : GenericBusiness<Rol, RolDto>
    {
        public RolBusiness(IDataFactoryGlobal factory, ILogger<Rol> logger, IMapper mapper) : base(factory.CreateRolData(), logger, mapper)
        {

        }

        protected override void Validate(RolDto rol)
        {
            if (rol == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(rol.Name))
                throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }

    }
}

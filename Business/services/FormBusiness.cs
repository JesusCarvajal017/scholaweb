using AutoMapper;
using Data.factories;
using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business.services
{
    public class FormBusiness :  GenericBusiness<Form, FormDto>
    {
        public FormBusiness(IDataFactoryGlobal factory, ILogger<Form> logger, IMapper mapper) : base(factory.CreateFormData(), logger, mapper)
        {

        }

        protected override void Validate(FormDto form)
        {
            if (form == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(form.Name))
                throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }
    }
}

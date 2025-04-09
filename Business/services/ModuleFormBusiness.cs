using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.factories;
using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;
using static Dapper.SqlMapper;

namespace Business.services
{
    public class ModuleFormBusiness : GenericBusiness<ModuleForm, ModuleFormDto>
    {
        public ModuleFormBusiness
            (IDataFactoryGlobal factory, ILogger<ModuleForm> logger, IMapper mapper) : base(factory.CreateModuleFormData(), logger, mapper)
        {

        }

        protected override void Validate(ModuleFormDto moduleFormDto)
        {
            if (moduleFormDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            //if (string.IsNullOrWhiteSpace(moduleFormDto.Name))
            //    throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }

        protected void Validate(ModuleFormCreateDto moduleFormDto)
        {
            if (moduleFormDto == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            //if (string.IsNullOrWhiteSpace(moduleFormDto.Name))
            //    throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }

        public async Task<ModuleFormCreateDto> CreateAsyncNew(ModuleFormCreateDto dtoExp)
        {
            try
            {
                Validate(dtoExp);
                var entity = _mapper.Map<ModuleForm>(dtoExp);
                var created = await _data.CreateAsyncLinq(entity);
                return _mapper.Map<ModuleFormCreateDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear el ModuleForm { dtoExp.Id }");
                throw new ExternalServiceException("Base de datos", $"Error al crear { dtoExp.Id }", ex);
            }
        }

        public async Task<ModuleFormCreateDto> UpdateNew(ModuleFormCreateDto dtoExp)
        {
            if (dtoExp == null)
                throw new ValidationException("Entidad", $"{dtoExp.Id} no puede ser nulo");

            try
            {
                Validate(dtoExp);

       
                if (dtoExp.Id == null || dtoExp.Id <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var entity = _mapper.Map<ModuleForm>(dtoExp);
                var updated = await _data.UpdateAsyncLinq(entity);

                if (!updated)
                    throw new ExternalServiceException("Base de datos", $"No se pudo actualizar {dtoExp.Id}");

                return dtoExp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar {dtoExp.Id}");
                throw new ExternalServiceException("Base de datos", $"Error al actualizar {dtoExp.Id}", ex);
            }
        }


    }
}

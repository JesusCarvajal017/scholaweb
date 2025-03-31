using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class ModuleBusiness
    {

        private readonly ModuleData _ModuleData;
        private readonly ILogger _logger;

        public ModuleBusiness(ModuleData ModuleData, ILogger logger)
        {
            _ModuleData = ModuleData;
            _logger = logger;
        }

        // Método para obtener todos los Modulees como DTOs
        public async Task<IEnumerable<ModuleDto>> GetAllModuleesAsync()
        {
            try
            {
                var Modules = await _ModuleData.GetAllAsync();
                return MapToDTOList(Modules); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Modulees");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Modulees", ex);
            }
        }

        // Método para obtener un Module por ID como DTO
        public async Task<ModuleDto> GetModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un Module con ID inválido: {ModuleId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Module debe ser mayor que cero");
            }

            try
            {
                var Module = await _ModuleData.GetByIdAsync(id);
                if (Module == null)
                {
                    _logger.LogInformation("No se encontró ningún Module con ID: {ModuleId}", id);
                    throw new EntityNotFoundException("Module", id);
                }

                return MapToDTO(Module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Module con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Module con ID {id}", ex);
            }
        }

        // Método para crear un Module desde un DTO
        public async Task<ModuleDto> CreateModuleAsync(ModuleDto ModuleDto)
        {
            try
            {
                ValidateModule(ModuleDto);

                var Module = MapToEntity(ModuleDto);

                var ModuleCreado = await _ModuleData.CreateAsync(Module);

                return MapToDTO(ModuleCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo Module: {ModuleNombre}", ModuleDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el Module", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateModule(ModuleDto ModuleDto)
        {
            if (ModuleDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto Module no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ModuleDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un Module con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del Module es obligatorio");
            }
        }


        // Método para mapear de Module a ModuleDTO
        private ModuleDto MapToDTO(Module Module)
        {
            return new ModuleDto
            {
                Id = Module.Id,
                Name = Module.Name,
                Description = Module.Description // Si existe en la entidad
            };
        }

        // Método para mapear de ModuleDTO a Module
        private Module MapToEntity(ModuleDto ModuleDTO)
        {
            return new Module
            {
                Id = ModuleDTO.Id,
                Name = ModuleDTO.Name,
                Description = ModuleDTO.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Module a una lista de ModuleDTO
        private IEnumerable<ModuleDto> MapToDTOList(IEnumerable<Module> Modules)
        {
            var ModulesDTO = new List<ModuleDto>();
            foreach (var Module in Modules)
            {
                ModulesDTO.Add(MapToDTO(Module));
            }
            return ModulesDTO;
        }
    }
}

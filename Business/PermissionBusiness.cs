

using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class PermissionBusiness
    {
        private readonly PermissionData _PermissionData;
        private readonly ILogger _logger;

        public PermissionBusiness(PermissionData PermissionData, ILogger logger)
        {
            _PermissionData = PermissionData;
            _logger = logger;
        }

        // Método para obtener todos los Permissiones como DTOs
        public async Task<IEnumerable<PermissionDto>> GetAllPermissionesAsync()
        {
            try
            {
                var Permissiones = await _PermissionData.GetAllAsync();
                return MapToDTOList(Permissiones); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Permissiones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Permissiones", ex);
            }
        }

        // Método para obtener un Permission por ID como DTO
        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un Permission con ID inválido: {PermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Permission debe ser mayor que cero");
            }

            try
            {
                var Permission = await _PermissionData.GetByIdAsync(id);
                if (Permission == null)
                {
                    _logger.LogInformation("No se encontró ningún Permission con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permission", id);
                }

                return MapToDTO(Permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Permission con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Permission con ID {id}", ex);
            }
        }

        // Método para crear un Permission desde un DTO
        public async Task<PermissionDto> CreatePermissionAsync(PermissionDto PermissionDto)
        {
            try
            {
                ValidatePermission(PermissionDto);

                var Permission = MapToEntity(PermissionDto);

                var PermissionCreado = await _PermissionData.CreateAsync(Permission);

                return MapToDTO(PermissionCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo Permission: {PermissionNombre}", PermissionDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el Permission", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePermission(PermissionDto PermissionDto)
        {
            if (PermissionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto Permission no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PermissionDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un Permission con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del Permission es obligatorio");
            }
        }


        // Método para mapear de Permission a PermissionDTO
        private PermissionDto MapToDTO(Permission Permission)
        {
            return new PermissionDto
            {
                Id = Permission.Id,
                Name = Permission.Name,
                Description = Permission.Description // Si existe en la entidad
            };
        }

        // Método para mapear de PermissionDTO a Permission
        private Permission MapToEntity(PermissionDto PermissionDTO)
        {
            return new Permission
            {
                Id = PermissionDTO.Id,
                Name = PermissionDTO.Name,
                Description = PermissionDTO.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Permission a una lista de PermissionDTO
        private IEnumerable<PermissionDto> MapToDTOList(IEnumerable<Permission> Permisssions)
        {
            var PermissionesDTO = new List<PermissionDto>();
            foreach (var Permission in Permisssions)
            {
                PermissionesDTO.Add(MapToDTO(Permission));
            }
            return PermissionesDTO;
        }
    }
}

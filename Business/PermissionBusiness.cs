using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class PermissionBusiness
    {
        private readonly PermissionData _PermissionData;
        private readonly ILogger<PermissionBusiness> _logger;

        public PermissionBusiness(PermissionData PermissionData, ILogger<PermissionBusiness> logger)
        {
            _PermissionData = PermissionData;
            _logger = logger;
        }

        // QUERY ALL
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

        // QUERY BY ID
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

        // INSERT 
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

      

        // UPDATE Permission
        public async Task<Object> UpdatePermissionAsync(PermissionDto PermissionDto)
        {
            try
            {

                ValidatePermission(PermissionDto);
                int id = PermissionDto.Id;
                var PermissionValid = await _PermissionData.GetByIdAsync(id);

                var Permission = MapToEntity(PermissionDto);
                var updatePermission = await _PermissionData.UpdateAsync(Permission);

                return new { status = updatePermission };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Permission con ID: {PermissionId}", PermissionDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Permission con ID {PermissionDto.Id}", ex);
            }
        }

        //DELETE Permission => LOGICAL
        public async Task<Object> DeletelogicaPermissionlAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {PermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Permission debe ser mayor que cero");
            }

            try
            {
                var Permission = await _PermissionData.DeleteLogicalAsync(id);
                if (Permission == null)
                {
                    _logger.LogInformation("No se encontró ningún Permission con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permission", id);
                }

                return Permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Permission con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Permission con ID {id}", ex);
            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistenPermissionAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {PermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Permission debe ser mayor que cero");
            }

            try
            {
                var Permission = await _PermissionData.DeletePersistentAsync(id);
                if (Permission == null)
                {
                    _logger.LogInformation("No se encontró ningún Permission con ID: {PermissionId}", id);
                    throw new EntityNotFoundException("Permission", id);
                }

                return Permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Permission con ID: {PermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Permission con ID {id}", ex);
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
                Description = Permission.Description,
                Status = Permission.Status
            };
        }

        // Método para mapear de PermissionDTO a Permission
        private Permission MapToEntity(PermissionDto PermissionDTO)
        {
            return new Permission
            {
                Id = PermissionDTO.Id,
                Name = PermissionDTO.Name,
                Description = PermissionDTO.Description,
                Status = PermissionDTO.Status
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

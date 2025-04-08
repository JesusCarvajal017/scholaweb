using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class RolFormPermissionBusiness
    {
        private readonly RolFormPermissionData _RolFormPermissionData;
        private readonly ILogger<RolFormPermissionBusiness> _logger;

        public RolFormPermissionBusiness(RolFormPermissionData RolFormPermissionData, ILogger<RolFormPermissionBusiness> logger)
        {
            _RolFormPermissionData = RolFormPermissionData;
            _logger = logger;
        }

        // QUERY ALL
        public async Task<IEnumerable<RolFormPermissionDto>> GetAllRolFormPermissionsAsync()
        {
            try
            {
                var RolFormPermissiones = await _RolFormPermissionData.GetAllAsync();
                return RolFormPermissiones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RolFormPermissiones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de RolFormPermissiones", ex);
            }
        }

        // QUERY BY ID  
        public async Task<RolFormPermissionDto> GetRolFormPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un RolFormPermission con ID inválido: {RolFormPermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del RolFormPermission debe ser mayor que cero");
            }

            try

            {
                var RolFormPermission = await _RolFormPermissionData.GetByIdAsync(id);
                if (RolFormPermission == null)
                {
                    _logger.LogInformation("No se encontró ningún RolFormPermission con ID: {RolFormPermissionId}", id);
                    throw new EntityNotFoundException("RolFormPermission", id);
                }

                return RolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolFormPermission con ID: {RolFormPermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolFormPermission con ID {id}", ex);
            }
        }

        // CREATE 
        public async Task<RolFormPermissionDto> CreateRolFormPermissionAsync(RolFormPermissionDto RolFormPermissionDto)
        {
            try
            {
                ValidateRolFormPermission(RolFormPermissionDto);

                var RolFormPermission = MapToEntity(RolFormPermissionDto);

                var RolFormPermissionCreado = await _RolFormPermissionData.CreateAsync(RolFormPermission);

                return MapToDTO(RolFormPermissionCreado);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al crear nuevo RolFormPermission: {RolFormPermissionNomb}", RolFormPermissionDto?.RolFormPermissionName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el RolFormPermission", ex);
            }
        }

        // UPDATE 
        public async Task<Object> UpdateRolFormPermissionAsync(RolFormPermissionDto RolFormPermissionDto)
        {
            try
            {

                ValidateRolFormPermission(RolFormPermissionDto);
                int id = RolFormPermissionDto.Id;
                var RolFormPermissionValid = await _RolFormPermissionData.GetByIdAsync(id);

                var RolFormPermission = MapToEntity(RolFormPermissionDto);
                var updateRolFormPermission = await _RolFormPermissionData.UpdateAsync(RolFormPermission);

                return new { status = updateRolFormPermission };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolFormPermission con ID: {RolFormPermissionId}", RolFormPermissionDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolFormPermission con ID {RolFormPermissionDto.Id}", ex);
            }
        }

        //DELETE RolFormPermission => LOGICAL
        public async Task<Object> DeletelogicaRolFormPermissionlAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {RolFormPermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del RolFormPermission debe ser mayor que cero");
            }

            try
            {
                var RolFormPermission = await _RolFormPermissionData.DeleteLogicalAsync(id);
                if (RolFormPermission == null)
                {
                    _logger.LogInformation("No se encontró ningún RolFormPermission con ID: {RolFormPermissionId}", id);
                    throw new EntityNotFoundException("RolFormPermission", id);
                }

                return RolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolFormPermission con ID: {RolFormPermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolFormPermission con ID {id}", ex);
            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistenRolFormPermissionAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {RolFormPermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del RolFormPermission debe ser mayor que cero");
            }

            try
            {
                var RolFormPermission = await _RolFormPermissionData.DeletePersistentAsync(id);
                if (RolFormPermission == null)
                {
                    _logger.LogInformation("No se encontró ningún RolFormPermission con ID: {RolFormPermissionId}", id);
                    throw new EntityNotFoundException("RolFormPermission", id);
                }

                return RolFormPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolFormPermission con ID: {RolFormPermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolFormPermission con ID {id}", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolFormPermission(RolFormPermissionDto RolFormPermissionDto)
        {
            if (RolFormPermissionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto RolFormPermission no puede ser nulo");
            }
        }

        // Método para mapear de RolFormPermission a RolFormPermissionDTO
        private RolFormPermissionDto MapToDTO(RolFormPermission RolFormPermission)
        {
            return new RolFormPermissionDto
            {
                Id = RolFormPermission.Id,
                FormId = RolFormPermission.FormId,
                RolId= RolFormPermission.RolId,
                PermissionId = RolFormPermission.PermissionId
            };
        }

        // Método para mapear de RolFormPermissionDTO a RolFormPermission
        private RolFormPermission MapToEntity(RolFormPermissionDto RolFormPermissionDTO)
        {
            return new RolFormPermission
            {
                Id = RolFormPermissionDTO.Id,
                FormId = RolFormPermissionDTO.FormId,
                RolId = RolFormPermissionDTO.RolId,
                PermissionId = RolFormPermissionDTO.PermissionId
            };
        }

        // Método para mapear una lista de RolFormPermission a una lista de RolFormPermissionDTO
        private IEnumerable<RolFormPermissionDto> MapToDTOList(IEnumerable<RolFormPermission> RolFormPermission)
        {
            var RolFormPermissionDTO = new List<RolFormPermissionDto>();
            foreach (var RolFormPermissions in RolFormPermission)
            {
                RolFormPermissionDTO.Add(MapToDTO(RolFormPermissions));
            }
            return RolFormPermissionDTO;
        }
    }
}

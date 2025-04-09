using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class UserRolBusinness
    {

        private readonly UserRolData _UserRolData;
        private readonly ILogger<UserRolBusinness> _logger;

        public UserRolBusinness(UserRolData UserRolData, ILogger<UserRolBusinness> logger)
        {
            _UserRolData = UserRolData;
            _logger = logger;
        }

        // QUERY ALL
        public async Task<IEnumerable<UserRolQueryDto>> GetAllUserRolsAsync()
        {
            try
            {
                var UserRoles = await _UserRolData.GetAllAsync();
                return UserRoles; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los UserRoles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de UserRoles", ex);
            }
        }

        // QUERY BY ID  
        public async Task<UserRolQueryDto> GetUserRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un UserRol con ID inválido: {UserRolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del UserRol debe ser mayor que cero");
            }

            try
            {
                var UserRol = await _UserRolData.GetByIdAsync(id);
                if (UserRol == null)
                {
                    _logger.LogInformation("No se encontró ningún UserRol con ID: {UserRolId}", id);
                    throw new EntityNotFoundException("UserRol", id);
                }

                return UserRol;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRol con ID: {UserRolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el UserRol con ID {id}", ex);
            }
        }

        // CREATE 
        public async Task<UserRolDto> CreateUserRolAsync(UserRolDto UserRolDto)
        {
            try
            {
                ValidateUserRol(UserRolDto);

                var UserRol = MapToEntity(UserRolDto);

                var UserRolCreado = await _UserRolData.CreateAsync(UserRol);

                return MapToDTO(UserRolCreado);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al crear nuevo UserRol: {UserRolNomb}", UserRolDto?.UserRolName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el UserRol", ex);
            }
        }

        // UPDATE 
        public async Task<Object> UpdateUserRolAsync(UserRolDto UserRolDto)
        {
            try
            {

                ValidateUserRol(UserRolDto);
                int id = UserRolDto.Id;
                var UserRolValid = await _UserRolData.GetByIdAsync(id);

                var UserRol = MapToEntity(UserRolDto);
                var updateUserRol = await _UserRolData.UpdateAsync(UserRol);

                return new { status = updateUserRol };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRol con ID: {UserRolId}", UserRolDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el UserRol con ID {UserRolDto.Id}", ex);
            }
        }

        //DELETE UserRol => LOGICAL
        public async Task<Object> DeletelogicaUserRollAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {UserRolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del UserRol debe ser mayor que cero");
            }

            try
            {
                var UserRol = await _UserRolData.DeleteLogicalAsync(id);
                if (UserRol == null)
                {
                    _logger.LogInformation("No se encontró ningún UserRol con ID: {UserRolId}", id);
                    throw new EntityNotFoundException("UserRol", id);
                }

                return UserRol;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRol con ID: {UserRolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el UserRol con ID {id}", ex);
            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistenUserRolAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {UserRolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del UserRol debe ser mayor que cero");
            }

            try
            {
                var UserRol = await _UserRolData.DeletePersistentAsync(id);
                if (UserRol == null)
                {
                    _logger.LogInformation("No se encontró ningún UserRol con ID: {UserRolId}", id);
                    throw new EntityNotFoundException("UserRol", id);
                }

                return UserRol;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRol con ID: {UserRolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el UserRol con ID {id}", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUserRol(UserRolDto UserRolDto)
        {
            if (UserRolDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto UserRol no puede ser nulo");
            }
        }

        // Método para mapear de UserRol a UserRolDTO
        private UserRolDto MapToDTO(UserRol UserRol)
        {
            return new UserRolDto
            {
                Id = UserRol.Id,
                UserId = UserRol.UserId,
                RolId = UserRol.RolId
            };
        }

        // Método para mapear de UserRolDTO a UserRol
        private UserRol MapToEntity(UserRolDto UserRolDTO)
        {
            return new UserRol
            {
                Id = UserRolDTO.Id,
                UserId = UserRolDTO.UserId,
                RolId = UserRolDTO.RolId
            };
        }

        // Método para mapear una lista de UserRol a una lista de UserRolDTO
        private IEnumerable<UserRolDto> MapToDTOList(IEnumerable<UserRol> UserRol)
        {
            var UserRolDTO = new List<UserRolDto>();
            foreach (var UserRols in UserRol)
            {
                UserRolDTO.Add(MapToDTO(UserRols));
            }
            return UserRolDTO;
        }

    }
}

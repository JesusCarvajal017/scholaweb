using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class UserBusiness
    {

        private readonly UserData _UserData;
        private readonly ILogger _logger;

        public UserBusiness(UserData UserData, ILogger logger)
        {
            _UserData = UserData;
            _logger = logger;
        }

        // Método para obtener todos los Useres como DTOs
        public async Task<IEnumerable<UserDto>> GetAllUseresAsync()
        {
            try
            {
                var Useres = await _UserData.GetAllAsync();
                return MapToDTOList(Useres); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Useres");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Useres", ex);
            }
        }

        // Método para obtener un User por ID como DTO
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un User con ID inválido: {UserId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del User debe ser mayor que cero");
            }

            try
            {
                var User = await _UserData.GetByIdAsync(id);
                if (User == null)
                {
                    _logger.LogInformation("No se encontró ningún User con ID: {UserId}", id);
                    throw new EntityNotFoundException("User", id);
                }

                return MapToDTO(User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el User con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el User con ID {id}", ex);
            }
        }

        // Método para crear un User desde un DTO
        public async Task<UserDto> CreateUserAsync(UserDto UserDto)
        {
            try
            {
                ValidateUser(UserDto);

                var User = MapToEntity(UserDto);

                var UserCreado = await _UserData.CreateAsync(User);

                return MapToDTO(UserCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo User: {UserNombre}", UserDto?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el User", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUser(UserDto UserDto)
        {
            if (UserDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto User no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(UserDto.UserName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un User con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del User es obligatorio");
            }
        }


        // Método para mapear de User a UserDTO
        private UserDto MapToDTO(User User)
        {
            return new UserDto
            {
                Id = User.Id,
                UserName = User.UserName,
            };
        }

        // Método para mapear de UserDTO a User
        private User MapToEntity(UserDto UserDTO)
        {
            return new User
            {
                Id = UserDTO.Id,
                UserName = UserDTO.UserName,
                Status = UserDTO.status
            };
        }

        // Método para mapear una lista de User a una lista de UserDTO
        private IEnumerable<UserDto> MapToDTOList(IEnumerable<User> User)
        {
            var UserDTO = new List<UserDto>();
            foreach (var users in User)
            {
                UserDTO.Add(MapToDTO(users));
            }
            return UserDTO;
        }

    }
}

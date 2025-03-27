
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class PersonBusiness
    {
        private readonly PersonData _PersonData;
        private readonly ILogger _logger;

        public PersonBusiness(PersonData PersonData, ILogger logger)
        {
            _PersonData = PersonData;
            _logger = logger;
        }

        // Método para obtener todos los Persones como DTOs
        public async Task<IEnumerable<PersonDto>> GetAllPersonesAsync()
        {
            try
            {
                var Person = await _PersonData.GetAllAsync();
                return MapToDTOList(Person); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Persones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Persones", ex);
            }
        }

        // Método para obtener un Person por ID como DTO
        public async Task<PersonDto> GetPersonByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un Person con ID inválido: {PersonId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Person debe ser mayor que cero");
            }

            try
            {
                var Person = await _PersonData.GetByIdAsync(id);
                if (Person == null)
                {
                    _logger.LogInformation("No se encontró ningún Person con ID: {PersonId}", id);
                    throw new EntityNotFoundException("Person", id);
                }

                return MapToDTO(Person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Person con ID: {PersonId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Person con ID {id}", ex);
            }
        }

        // Método para crear un Person desde un DTO
        public async Task<PersonDto> CreatePersonAsync(PersonDto PersonDto)
        {
            try
            {
                ValidatePerson(PersonDto);

                var Person = MapToEntity(PersonDto);

                var PersonCreado = await _PersonData.CreateAsync(Person);

                return MapToDTO(PersonCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo Person: {PersonNombre}", PersonDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el Person", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePerson(PersonDto PersonDto)
        {
            if (PersonDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto Person no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PersonDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un Person con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del Person es obligatorio");
            }
        }


        // Método para mapear de Person a PersonDTO
        private PersonDto MapToDTO(Person Person)
        {
            return new PersonDto
            {
                Id = Person.Id,
                Name = Person.Name,
                LastName = Person.LastName // Si existe en la entidad
            };
        }

        // Método para mapear de PersonDTO a Person
        private Person MapToEntity(PersonDto PersonDTO)
        {
            return new Person
            {
                Id = PersonDTO.Id,
                Name= PersonDTO.Name,
                LastName = PersonDTO.LastName // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Person a una lista de PersonDTO
        private IEnumerable<PersonDto> MapToDTOList(IEnumerable<Person> Person)
        {
            var PersonDTO = new List<PersonDto>();
            foreach (var Persons in Person)
            {
                PersonDTO.Add(MapToDTO(Persons));
            }
            return PersonDTO;
        }

    }
}

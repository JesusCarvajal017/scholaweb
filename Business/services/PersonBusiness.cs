
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Data.factories;
using Data.repositories.Global;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;


namespace Business.services
{
    public class PersonBusiness : GenericBusiness<Person, PersonDto>
    {

        public PersonBusiness(IDataFactoryGlobal factory, ILogger<Person> logger, IMapper mapper) : base(factory.CreatePersonData(), logger, mapper)
        {
        
        }

        protected override void Validate(PersonDto person)
        {
            if (person == null)
                throw new ValidationException("El formulario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(person.Name))
                throw new ValidationException("El título del formulario es obligatorio.");

            // Agrega más validaciones si necesitas
        }


        //private readonly IGlobalLinq<Person> _personData;
        //private readonly ILogger<PersonBusiness> _logger;
        //private readonly IMapper _mapper;

        //// Ahora se inyecta la factoría y se obtiene el repositorio
        //public PersonBusiness(IDataFactoryGlobal dataFactory, ILogger<PersonBusiness> logger, IMapper mapper)
        //{
        //    _personData = dataFactory.CreatePersonData();
        //    _logger = logger;
        //    _mapper = mapper;
        //}

        //// Obtener todos los Person como DTOs
        //public async Task<IEnumerable<PersonDto>> GetAllPersonesAsync()
        //{
        //    try
        //    {
        //        var persons = await _personData.GetAllAsyncLinq();
        //        return _mapper.Map<IEnumerable<PersonDto>>(persons);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al obtener todos los Persones");
        //        throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Persones", ex);
        //    }
        //}

        //// Obtener un Person por ID como DTO
        //public async Task<PersonDto> GetPersonByIdAsync(int id)
        //{
        //    if (id <= 0)
        //    {
        //        _logger.LogWarning("Se intentó obtener un Person con ID inválido: {PersonId}", id);
        //        throw new Utilities.Exceptions.ValidationException("id", "El ID del Person debe ser mayor que cero");
        //    }

        //    try
        //    {
        //        var person = await _personData.GetByIdAsyncLinq(id);
        //        if (person == null)
        //        {
        //            _logger.LogInformation("No se encontró ningún Person con ID: {PersonId}", id);
        //            throw new EntityNotFoundException("Person", id);
        //        }

        //        return _mapper.Map<PersonDto>(person);
        //        //return MapToDTO(person);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al obtener el Person con ID: {PersonId}", id);
        //        throw new ExternalServiceException("Base de datos", $"Error al recuperar el Person con ID {id}", ex);
        //    }
        //}

        //// Crear un Person desde un DTO
        //public async Task<PersonDto> CreatePersonAsync(PersonDto personDto)
        //{
        //    try
        //    {
        //        ValidatePerson(personDto);
        //        var personEntity = _mapper.Map<Person>(personDto);
        //        var personCreado = await _personData.CreateAsyncLinq(personEntity);
        //        return _mapper.Map<PersonDto>(personCreado); 
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al crear nuevo Person: {PersonNombre}", personDto?.Name ?? "null");
        //        throw new ExternalServiceException("Base de datos", "Error al crear el Person", ex);
        //    }
        //}

        //// Actualizar Person
        //public async Task<object> UpdatePersonAsync(PersonDto personDto)
        //{
        //    try
        //    {
        //        ValidatePerson(personDto);
        //        var id = personDto.Id;
        //        var personExistente = await GetPersonByIdAsync(id);
        //        if (personExistente == null)
        //        {
        //            _logger.LogInformation("No se encontró ningún Person con ID: {PersonId}", id);
        //            throw new EntityNotFoundException("Person", id);
        //        }

        //        var personEntity = _mapper.Map<Person>(personDto);
        //        bool updateResult = await _personData.UpdateAsyncLinq(personEntity);
        //        return new { status = updateResult };
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al actualizar el Person con ID: {PersonId}", personDto.Id);
        //        throw new ExternalServiceException("Base de datos", $"Error al actualizar el Person con ID {personDto.Id}", ex);
        //    }
        //}

        //// DELETE LOGICAL
        //public async Task<object> DeleteLogicalPersonAsync(int id)
        //{
        //    if (id <= 0)
        //    {
        //        _logger.LogWarning("Id inválido {PersonId}", id);
        //        throw new Utilities.Exceptions.ValidationException("id", "El ID del Person debe ser mayor que cero");
        //    }

        //    try
        //    {
        //        var result = await _personData.DeleteLogicalAsyncLinq(id);
        //        if (result == null)
        //        {
        //            _logger.LogInformation("No se encontró ningún Person con ID: {PersonId}", id);
        //            throw new EntityNotFoundException("Person", id);
        //        }
        //        return result;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al realizar delete lógico para el Person con ID: {PersonId}", id);
        //        throw new ExternalServiceException("Base de datos", $"Error al eliminar lógicamente el Person con ID {id}", ex);
        //    }
        //}

        //// DELETE PERSISTENT
        //public async Task<object> DeletePersistentPersonAsync(int id)
        //{
        //    if (id <= 0)
        //    {
        //        _logger.LogWarning("Id inválido {PersonId}", id);
        //        throw new Utilities.Exceptions.ValidationException("id", "El ID del Person debe ser mayor que cero");
        //    }

        //    try
        //    {
        //        var result = await _personData.DeletePersistentAsyncLinq(id);
        //        if (result == null)
        //        {
        //            _logger.LogInformation("No se encontró ningún Person con ID: {PersonId}", id);
        //            throw new EntityNotFoundException("Person", id);
        //        }
        //        return result;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al realizar delete persistente para el Person con ID: {PersonId}", id);
        //        throw new ExternalServiceException("Base de datos", $"Error al eliminar definitivamente el Person con ID {id}", ex);
        //    }
        //}

        //// Métodos privados de mapeo y validación:

        //private void ValidatePerson(PersonDto personDto)
        //{
        //    if (personDto == null)
        //        throw new Utilities.Exceptions.ValidationException("El objeto Person no puede ser nulo");

        //    if (string.IsNullOrWhiteSpace(personDto.Name))
        //    {
        //        _logger.LogWarning("Se intentó crear/actualizar un Person con Name vacío");
        //        throw new Utilities.Exceptions.ValidationException("Name", "El Name del Person es obligatorio");
        //    }
        //}


    }
}

using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    public class FormBusiness
    {
        private readonly FormData _FormData;
        private readonly ILogger<FormBusiness> _logger;

        public FormBusiness(FormData FormData, ILogger<FormBusiness> logger)
        {
            _FormData = FormData;
            _logger = logger;
        }

        // Método para obtener todos los Form como DTOs
        public async Task<IEnumerable<FormDto>> GetAllFormesAsync()
        {
            try
            {
                var Forms = await _FormData.GetAllAsyncLinq();
                return MapToDTOList(Forms); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los Formes");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de Formes", ex);
            }
        }

        // Método para obtener un Form por ID como DTO
        public async Task<FormDto> GetByIdFormAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un Form con ID inválido: {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Form debe ser mayor que cero");
            }

            try
            {
                var Form = await _FormData.GetByIdAsync(id);
                if (Form == null)
                {
                    _logger.LogInformation("No se encontró ningún Form con ID: {FormId}", id);
                    throw new EntityNotFoundException("Form", id);
                }

                return MapToDTO(Form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Form con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Form con ID {id}", ex);
            }
        }

        // Método para crear un Form desde un DTO
        public async Task<FormDto> CreateFormAsync(FormDto FormDto)
        {
            try
            {
                ValidateForm(FormDto);

                var Form = MapToEntity(FormDto);

                var FormCreado = await _FormData.CreateAsync(Form);

                return MapToDTO(FormCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo Form: {FormNombre}", FormDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el Form", ex);
            }
        }



        // UPDATE Form
        public async Task<Object> UpdateFormAsync(FormDto FormDto)
        {
            try
            {
                ValidateForm(FormDto);
                int id = FormDto.Id;
                var FormValid = await _FormData.GetByIdAsync(id);

                var Form = MapToEntity(FormDto);
                var updateForm = await _FormData.UpdateAsync(Form);

                return new { status = updateForm };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Form con ID: {FormId}", FormDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Form con ID {FormDto.Id}", ex);
            }
        }

        //DELETE Form => LOGICAL
        public async Task<Object> DeletelogicaFormlAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Form debe ser mayor que cero");
            }

            try
            {
                var Form = await _FormData.DeleteLogicalAsync(id);
                if (Form == null)
                {
                    _logger.LogInformation("No se encontró ningún Form con ID: {FormId}", id);
                    throw new EntityNotFoundException("Form", id);
                }

                return Form;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Form con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Form con ID {id}", ex);
            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistenFormAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del Form debe ser mayor que cero");
            }

            try
            {
                var Form = await _FormData.DeletePersistentAsync(id);
                if (Form == null)
                {
                    _logger.LogInformation("No se encontró ningún Form con ID: {FormId}", id);
                    throw new EntityNotFoundException("Form", id);
                }

                return Form;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Form con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Form con ID {id}", ex);
            }
        }




        // Método para validar el DTO
        private void ValidateForm(FormDto FormDto)
        {
            if (FormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto Form no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(FormDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un Form con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del Form es obligatorio");
            }
        }


        // Método para mapear de Form a FormDTO
        private FormDto MapToDTO(Form Form)
        {
            return new FormDto
            {
                Id = Form.Id,
                Name = Form.Name,
                Description = Form.Description,
                Status = Form.Status
            };
        }

        // Método para mapear de FormDTO a Form
        private Form MapToEntity(FormDto FormDTO)
        {
            return new Form
            {
                Id = FormDTO.Id,
                Name = FormDTO.Name,
                Description = FormDTO.Description,
                Status = FormDTO.Status 
            };
        }

        // Método para mapear una lista de Form a una lista de FormDTO
        private IEnumerable<FormDto> MapToDTOList(IEnumerable<Form> Forms)
        {
            var FormesDTO = new List<FormDto>();
            foreach (var Form in Forms)
            {
                FormesDTO.Add(MapToDTO(Form));
            }
            return FormesDTO;
        }
    }
}

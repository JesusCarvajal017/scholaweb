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
    public class ModuleFormBusiness
    {
        private readonly ModuleFormData _ModuleFormData;
        private readonly ILogger<ModuleFormBusiness> _logger;

        public ModuleFormBusiness(ModuleFormData ModuleFormData, ILogger<ModuleFormBusiness> logger)
        {
            _ModuleFormData = ModuleFormData;
            _logger = logger;
        }

        // QUERY ALL
        public async Task<IEnumerable<ModuleFormDto>> GetAllModuleFormsAsync()
        {
            try
            {
                var ModuleFormes = await _ModuleFormData.GetAllAsync();
                return ModuleFormes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los ModuleFormes");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de ModuleFormes", ex);
            }
        }

        // QUERY BY ID  
        public async Task<ModuleFormDto> GetModuleFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un ModuleForm con ID inválido: {ModuleFormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del ModuleForm debe ser mayor que cero");
            }

            try
            {
                var ModuleForm = await _ModuleFormData.GetByIdAsync(id);
                if (ModuleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún ModuleForm con ID: {ModuleFormId}", id);
                    throw new EntityNotFoundException("ModuleForm", id);
                }

                return ModuleForm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuleForm con ID: {ModuleFormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el ModuleForm con ID {id}", ex);
            }
        }

        // CREATE 
        public async Task<ModuleFormDto> CreateModuleFormAsync(ModuleFormDto ModuleFormDto)
        {
            try
            {
                ValidateModuleForm(ModuleFormDto);

                var ModuleForm = MapToEntity(ModuleFormDto);

                var ModuleFormCreado = await _ModuleFormData.CreateAsync(ModuleForm);

                return MapToDTO(ModuleFormCreado);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al crear nuevo ModuleForm: {ModuleFormNomb}", ModuleFormDto?.ModuleFormName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el ModuleForm", ex);
            }
        }

        // UPDATE 
        public async Task<Object> UpdateModuleFormAsync(ModuleFormDto ModuleFormDto)
        {
            try
            {

                ValidateModuleForm(ModuleFormDto);
                int id = ModuleFormDto.Id;
                var ModuleFormValid = await _ModuleFormData.GetByIdAsync(id);

                var ModuleForm = MapToEntity(ModuleFormDto);
                var updateModuleForm = await _ModuleFormData.UpdateAsync(ModuleForm);

                return new { status = updateModuleForm };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuleForm con ID: {ModuleFormId}", ModuleFormDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el ModuleForm con ID {ModuleFormDto.Id}", ex);
            }
        }

        //DELETE ModuleForm => LOGICAL
        public async Task<Object> DeletelogicaModuleFormlAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {ModuleFormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del ModuleForm debe ser mayor que cero");
            }

            try
            {
                var ModuleForm = await _ModuleFormData.DeleteLogicalAsync(id);
                if (ModuleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún ModuleForm con ID: {ModuleFormId}", id);
                    throw new EntityNotFoundException("ModuleForm", id);
                }

                return ModuleForm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuleForm con ID: {ModuleFormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el ModuleForm con ID {id}", ex);
            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistenModuleFormAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Id invalido {ModuleFormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del ModuleForm debe ser mayor que cero");
            }

            try
            {
                var ModuleForm = await _ModuleFormData.DeletePersistentAsync(id);
                if (ModuleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún ModuleForm con ID: {ModuleFormId}", id);
                    throw new EntityNotFoundException("ModuleForm", id);
                }

                return ModuleForm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuleForm con ID: {ModuleFormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el ModuleForm con ID {id}", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateModuleForm(ModuleFormDto ModuleFormDto)
        {
            if (ModuleFormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto ModuleForm no puede ser nulo");
            }
        }

        // Método para mapear de ModuleForm a ModuleFormDTO
        private ModuleFormDto MapToDTO(ModuleForm ModuleForm)
        {
            return new ModuleFormDto
            {
                Id = ModuleForm.Id,
                ModuleId = ModuleForm.ModuleId,
                FormId = ModuleForm.FormId
            };
        }

        // Método para mapear de ModuleFormDTO a ModuleForm
        private ModuleForm MapToEntity(ModuleFormDto ModuleFormDTO)
        {
            return new ModuleForm
            {
                Id = ModuleFormDTO.Id,
                ModuleId = ModuleFormDTO.ModuleId,
                FormId = ModuleFormDTO.FormId
            };
        }

        // Método para mapear una lista de ModuleForm a una lista de ModuleFormDTO
        private IEnumerable<ModuleFormDto> MapToDTOList(IEnumerable<ModuleForm> ModuleForm)
        {
            var ModuleFormDTO = new List<ModuleFormDto>();
            foreach (var ModuleForms in ModuleForm)
            {
                ModuleFormDTO.Add(MapToDTO(ModuleForms));
            }
            return ModuleFormDTO;
        }
    }
}

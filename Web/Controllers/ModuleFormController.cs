using Business.services;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModuleFormController : ControllerBase
    {
        private readonly ModuleFormBusiness _ModuleFormBusiness;
        private readonly ILogger<ModuleFormController> _logger;

        /// Constructor del controlador de permisos
        public ModuleFormController(ModuleFormBusiness ModuleFormBusiness, ILogger<ModuleFormController> logger)
        {
            _ModuleFormBusiness = ModuleFormBusiness;
            _logger = logger;
        }

        // QUERY ALL
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleFormDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllModuleForms()
        {
            try
            {
                var ModuleForms = await _ModuleFormBusiness.GetAllAsync();
                return Ok(ModuleForms);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // QUERY BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleFormDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetModuleFormById(int id)
        {
            try
            {
                var ModuleForm = await _ModuleFormBusiness.GetByIdAsync(id);
                return Ok(ModuleForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {ModuleFormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {ModuleFormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // INSERT 
        [HttpPost]
        [ProducesResponseType(typeof(ModuleFormCreateDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateModuleForm([FromBody] ModuleFormCreateDto ModuleFormDto)
        {
            try
            {
                var createdModuleForm = await _ModuleFormBusiness.CreateAsyncNew(ModuleFormDto);
                return CreatedAtAction(nameof(GetModuleFormById), new { id = createdModuleForm.Id }, createdModuleForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // UPDATE
        [HttpPut("update")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateModuleForm([FromBody] ModuleFormCreateDto ModuleFormDto)
        {
            try
            {
                var update = await _ModuleFormBusiness.UpdateNew(ModuleFormDto);
                return Ok(update);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizacion el ModuleForm con ID: {ModuleFormId}", ModuleFormDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "ModuleForm no encontrado con ID: {ModuleFormId}", ModuleFormDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el ModuleForm con ID: {ModuleFormId}", ModuleFormDto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE => LOGICAL
        //[HttpPut("logical/{id}")]
        //[ProducesResponseType(typeof(Object), 200)]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> DeletleModuleForm(int id)
        //{
        //    try
        //    {
        //        var response = await _ModuleFormBusiness.DeletelogicaModuleFormlAsync(id);
        //        return Ok(response);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        _logger.LogWarning(ex, "Validación fallida al eliminar el ModuleForm con ID: {ModuleFormId}", id);
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        _logger.LogInformation(ex, "ModuleForm no encontrado con ID: {ModuleFormId}", id);
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (ExternalServiceException ex)
        //    {
        //        _logger.LogError(ex, "Error al eliminar el ModuleForm con ID: {ModuleFormId}", id);
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        

        // DELETE => PERSISTENT
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteModuleForm(int id)
        {
            try
            {
                var response = await _ModuleFormBusiness.DeleteAsync(id);
                return Ok(response); // Código 204: Eliminación exitosa sin contenido
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el ModuleForm con ID: {ModuleFormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "ModuleForm no encontrado con ID: {ModuleFormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el ModuleForm con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

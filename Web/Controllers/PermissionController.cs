using Business;
using Business.services;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;


namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionBusiness _PermissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        /// Constructor del controlador de permisos
        public PermissionController(PermissionBusiness PermissionBusiness, ILogger<PermissionController> logger)
        {
            _PermissionBusiness = PermissionBusiness;
            _logger = logger;
        }

        // QUERY ALL
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var Permissions = await _PermissionBusiness.GetAllAsync();
                return Ok(Permissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // QUERY BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var Permission = await _PermissionBusiness.GetByIdAsync(id);
                return Ok(Permission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // INSERT 
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto PermissionDto)
        {
            try
            {
                var createdPermission = await _PermissionBusiness.CreateAsync(PermissionDto);
                return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
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
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionDto PermissionDto)
        {
            try
            {
                var update = await _PermissionBusiness.UpdateAsync(PermissionDto);
                return Ok(update);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizacion el Permission con ID: {PermissionId}", PermissionDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permission no encontrado con ID: {PermissionId}", PermissionDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el Permission con ID: {PermissionId}", PermissionDto.Id);
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
        //public async Task<IActionResult> DeletlePermission(int id)
        //{
        //    try
        //    {
        //        var response = await _PermissionBusiness.DeleteAsync(id);
        //        return Ok(response);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        _logger.LogWarning(ex, "Validación fallida al eliminar el Permission con ID: {PermissionId}", id);
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        _logger.LogInformation(ex, "Permission no encontrado con ID: {PermissionId}", id);
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (ExternalServiceException ex)
        //    {
        //        _logger.LogError(ex, "Error al eliminar el Permission con ID: {PermissionId}", id);
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}

        // DELETE => PERSISTENT
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Object), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                var response = await _PermissionBusiness.DeleteAsync(id);
                return Ok(response); // Código 204: Eliminación exitosa sin contenido
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el Permission con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permission no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el Permission con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

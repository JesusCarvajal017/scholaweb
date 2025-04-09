using Business.services;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;


namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolFormPermissionController : ControllerBase
    {
        private readonly RolFormPermissionBusiness _RolFormPermissionBusiness;
        private readonly ILogger<RolFormPermissionController> _logger;

        /// Constructor del controlador de permisos
        public RolFormPermissionController(RolFormPermissionBusiness RolFormPermissionBusiness, ILogger<RolFormPermissionController> logger)
        {
            _RolFormPermissionBusiness = RolFormPermissionBusiness;
            _logger = logger;
        }

        // QUERY ALL
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolFormPermissionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolFormPermissions()
        {
            try
            {
                var RolFormPermissions = await _RolFormPermissionBusiness.GetAllAsync();
                return Ok(RolFormPermissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // QUERY BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolFormPermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolFormPermissionById(int id)
        {
            try
            {
                var RolFormPermission = await _RolFormPermissionBusiness.GetByIdAsync(id);
                return Ok(RolFormPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {RolFormPermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolFormPermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {RolFormPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // INSERT 
        [HttpPost]
        [ProducesResponseType(typeof(RolFormPermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolFormPermission([FromBody] RolFormPermissionCreateDto RolFormPermissionDto)
        {
            try
            {
                var createdRolFormPermission = await _RolFormPermissionBusiness.CreateAsyncNew(RolFormPermissionDto);
                return CreatedAtAction(nameof(GetRolFormPermissionById), new { id = createdRolFormPermission.Id }, createdRolFormPermission);
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
        public async Task<IActionResult> UpdateRolFormPermission([FromBody] RolFormPermissionCreateDto RolFormPermissionDto)
        {
            try
            {
                var update = await _RolFormPermissionBusiness.UpdateNew(RolFormPermissionDto);
                return Ok(update);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizacion el RolFormPermission con ID: {RolFormPermissionId}", RolFormPermissionDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {RolFormPermissionId}", RolFormPermissionDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el RolFormPermission con ID: {RolFormPermissionId}", RolFormPermissionDto.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //// DELETE => LOGICAL
        //[HttpPut("logical/{id}")]
        //[ProducesResponseType(typeof(Object), 200)]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> DeletleRolFormPermission(int id)
        //{
        //    try
        //    {
        //        var response = await _RolFormPermissionBusiness.DeletelogicaRolFormPermissionlAsync(id);
        //        return Ok(response);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        _logger.LogWarning(ex, "Validación fallida al eliminar el RolFormPermission con ID: {RolFormPermissionId}", id);
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {RolFormPermissionId}", id);
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (ExternalServiceException ex)
        //    {
        //        _logger.LogError(ex, "Error al eliminar el RolFormPermission con ID: {RolFormPermissionId}", id);
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
        public async Task<IActionResult> DeleteRolFormPermission(int id)
        {
            try
            {
                var response = await _RolFormPermissionBusiness.DeleteAsync(id);
                return Ok(response); // Código 204: Eliminación exitosa sin contenido
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el RolFormPermission con ID: {RolFormPermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "RolFormPermission no encontrado con ID: {RolFormPermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolFormPermission con ID: {RolFormPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

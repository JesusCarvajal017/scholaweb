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
    public class UserRolController : ControllerBase
    {
        private readonly UserRolBusiness _UserRolBusiness;
        private readonly ILogger<UserRolController> _logger;

        /// Constructor del controlador de permisos
        public UserRolController(UserRolBusiness UserRolBusiness, ILogger<UserRolController> logger)
        {
            _UserRolBusiness = UserRolBusiness;
            _logger = logger;
        }

        // QUERY ALL
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserRolDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUserRols()
        {
            try
            {
                var UserRols = await _UserRolBusiness.GetAllAsync();
                return Ok(UserRols);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // QUERY BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserRolDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserRolById(int id)
        {
            try
            {
                var UserRol = await _UserRolBusiness.GetByIdAsync(id);
                return Ok(UserRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {UserRolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {UserRolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {UserRolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // INSERT 
        [HttpPost]
        [ProducesResponseType(typeof(UserRolDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUserRol([FromBody] UserRolCreateDto UserRolDto)
        {
            try
            {
                var createdUserRol = await _UserRolBusiness.CreateAsyncNew(UserRolDto);
                return CreatedAtAction(nameof(GetUserRolById), new { id = createdUserRol.Id }, createdUserRol);
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
        public async Task<IActionResult> UpdateUserRol([FromBody] UserRolCreateDto UserRolDto)
        {
            try
            {
                var update = await _UserRolBusiness.UpdateNew(UserRolDto);
                return Ok(update);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizacion el userrol con ID: {UserRolId}", UserRolDto.Id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "UserRol no encontrado con ID: {UserRolId}", UserRolDto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el userrol con ID: {UserRolId}", UserRolDto.Id);
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
        //public async Task<IActionResult> DeletleUserRol(int id)
        //{
        //    try
        //    {
        //        var response = await _UserRolBusiness.DeletelogicaUserRollAsync(id);
        //        return Ok(response);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        _logger.LogWarning(ex, "Validación fallida al eliminar el userrol con ID: {UserRolId}", id);
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        _logger.LogInformation(ex, "UserRol no encontrado con ID: {UserRolId}", id);
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (ExternalServiceException ex)
        //    {
        //        _logger.LogError(ex, "Error al eliminar el userrol con ID: {UserRolId}", id);
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
        public async Task<IActionResult> DeleteUserRol(int id)
        {
            try
            {
                var response = await _UserRolBusiness.DeleteAsync(id);
                return Ok(response); // Código 204: Eliminación exitosa sin contenido
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el userrol con ID: {UserRolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "UserRol no encontrado con ID: {UserRolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el userrol con ID: {UserRolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

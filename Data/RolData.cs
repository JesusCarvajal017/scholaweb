
using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public RolData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<Rol>> GetRolsAsync()
        {
            string query = "SELECT * FROM data.rol";
            return (IEnumerable<Rol>)await _context.QueryAsync<IEnumerable<Rol>>(query);
        }




        public async Task<Rol> GetRolAsync(int id)
        {
            try
            {
               
                //return await _context.QueryAsync<IEnumerable<Rol>>(query);
                return await _context.Set<Rol>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol con Id {RolId}", id);
                throw; 
            }
        }

        public async Task<Rol> CreateAsync(Rol rol)
        {
            try
            {
                await _context.Set<Rol>().AddAsync(rol);
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el rol");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Rol rol)
        {
            try
            {
                _context.Set<Rol>().Update(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rol = await _context.Set<Rol>().FindAsync(id);
                if(rol == null)
                    return false;

                _context.Set<Rol>().Remove(rol);
                await _context.SaveChangesAsync();
                return true;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el rol: {ex.Message}");
                return false;
            }
        }

    }
}

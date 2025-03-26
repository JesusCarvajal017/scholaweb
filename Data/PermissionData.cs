using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Contexts;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public PermissionData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            string query = "SELECT * FROM data.Permission";
            return (IEnumerable<Permission>)await _context.QueryAsync<IEnumerable<Permission>>(query);
        }




        public async Task<Permission> GetPermissionAsync(int id)
        {
            try
            {

                //return await _context.QueryAsync<IEnumerable<Permission>>(query);
                return await _context.Set<Permission>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Permission con Id {PermissionId}", id);
                throw;
            }
        }

        public async Task<Permission> CreateAsync(Permission Permission)
        {
            try
            {
                await _context.Set<Permission>().AddAsync(Permission);
                await _context.SaveChangesAsync();
                return Permission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el Permission");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Permission Permission)
        {
            try
            {
                _context.Set<Permission>().Update(Permission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el Permission: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Permission = await _context.Set<Permission>().FindAsync(id);
                if (Permission == null)
                    return false;

                _context.Set<Permission>().Remove(Permission);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el Permission: {ex.Message}");
                return false;
            }
        }
    }
}

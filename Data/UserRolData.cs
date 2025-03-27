using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UserRolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public UserRolData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserRol>> GetUserRolsAsync()
        {
            string query = @"SELECT Rol.Name AS NameRol, Rol.id AS idRol ,User.id AS idUser ,User.Name AS UserName 
                             FROM Form UserRol AS ur
                             INNER JOIN Rol AS rl
                                 ON rl.Id = ur.idRol
                             INNER JOIN User AS us
                                ON us.id = ur.idUser;";
            return (IEnumerable<UserRol>)await _context.QueryAsync<IEnumerable<UserRol>>(query);
        }

        public async Task<UserRol> GetUserRolIdAsync(int id)
        {
            try
            {
                string query = @"SELECT Rol.Name AS NameRol, Rol.id AS idRol ,User.id AS idUser ,User.Name AS UserName 
                                FROM Form UserRol AS ur
                                INNER JOIN Rol AS rl
                                    ON rl.Id = ur.idRol
                                INNER JOIN User AS us
                                    ON us.id = ur.idUser
                                    WHERE ur.id = @Id;";
               
                return await _context.QueryFirstOrDefaultAsync<UserRol>(query, new { Id = id} );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener UserRol con Id {UserRolId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<UserRol>> GetAllLinQAsync()
        {
            return await _context.Set<UserRol>()

                .Include(form => form.Form)
                .Include(form => form.Module)
                .ToListAsync();
        }

        public async Task<UserRol> CreateAsync(UserRol UserRol)
        {
            try
            {
                await _context.Set<UserRol>().AddAsync(UserRol);
                await _context.SaveChangesAsync();
                return UserRol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el UserRol");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(UserRol UserRol)
        {
            try
            {
                _context.Set<UserRol>().Update(UserRol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el UserRol: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var UserRol = await _context.Set<UserRol>().FindAsync(id);
                if (UserRol == null)
                    return false;

                _context.Set<UserRol>().Remove(UserRol);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el UserRol: {ex.Message}");
                return false;
            }
        }
    }
}

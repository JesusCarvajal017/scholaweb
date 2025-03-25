
using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public UserData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            string query = "SELECT * FROM data.User";
            return (IEnumerable<User>)await _context.QueryAsync<IEnumerable<User>>(query);
        }

        public async Task<User> GetUserAsync(int id)
        {
            try
            {

                //return await _context.QueryAsync<IEnumerable<User>>(query);
                return await _context.Set<User>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener User con Id {UserId}", id);
                throw;
            }
        }

        public async Task<User> CreateAsync(User User)
        {
            try
            {
                await _context.Set<User>().AddAsync(User);
                await _context.SaveChangesAsync();
                return User;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el User");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(User User)
        {
            try
            {
                _context.Set<User>().Update(User);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el User: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var User = await _context.Set<User>().FindAsync(id);
                if (User == null)
                    return false;

                _context.Set<User>().Remove(User);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el User: {ex.Message}");
                return false;
            }
        }

    }
}

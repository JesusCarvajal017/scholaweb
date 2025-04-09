using Data.interfaces;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class UserData : IGlobalLinq<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserData> _logger;
        public UserData(ApplicationDbContext context, ILogger<UserData> logger)
        {
            _context = context;
            _logger = logger;
        }

      

        //======================================______________  LINQ  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<User>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<User>()
                .Where(p => p.Status == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Useras");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<User?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Usera por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<User> CreateAsyncLinq(User User)
        {
            try
            {
                User.Status = 0; // Establece el estado de la Usera (activo e inactivo)
                await _context.Set<User>().AddAsync(User);
                await _context.SaveChangesAsync();
                return User;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Usera {User}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(User User)
        {
            try
            {
                _context.Set<User>().Update(User);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {User}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<User>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<User>().Remove(Delete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
        public async Task<bool> DeleteLogicalAsyncLinq(int id)
        {
            try
            {
                var entity = await _context.Set<User>().FindAsync(id);
                if (entity == null) return false; // usuario inexistente

                // Marcar como eliminado
                entity.Status = 0;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }

    }
}
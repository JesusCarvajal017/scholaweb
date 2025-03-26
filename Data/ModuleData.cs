
using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class ModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ModuleData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            string query = "SELECT * FROM data.Module";
            return (IEnumerable<Module>)await _context.QueryAsync<IEnumerable<Module>>(query);
        }

        public async Task<Module> GetModuleAsync(int id)
        {
            try
            {

                //return await _context.QueryAsync<IEnumerable<Module>>(query);
                return await _context.Set<Module>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Module con Id {ModuleId}", id);
                throw;
            }
        }

        public async Task<Module> CreateAsync(Module Module)
        {
            try
            {
                await _context.Set<Module>().AddAsync(Module);
                await _context.SaveChangesAsync();
                return Module;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el Module");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Module Module)
        {
            try
            {
                _context.Set<Module>().Update(Module);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el Module: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Module = await _context.Set<Module>().FindAsync(id);
                if (Module == null)
                    return false;

                _context.Set<Module>().Remove(Module);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el Module: {ex.Message}");
                return false;
            }
        }

    }
}

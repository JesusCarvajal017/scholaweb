
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

        public async Task<IEnumerable<ModuleData>> GetModulesAsync()
        {
            string query = "SELECT * FROM data.Module";
            return (IEnumerable<ModuleData>)await _context.QueryAsync<IEnumerable<ModuleData>>(query);
        }

        public async Task<ModuleData> GetModuleAsync(int id)
        {
            try
            {

                //return await _context.QueryAsync<IEnumerable<Module>>(query);
                return await _context.Set<ModuleData>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Module con Id {ModuleId}", id);
                throw;
            }
        }

        public async Task<ModuleData> CreateAsync(ModuleData Module)
        {
            try
            {
                await _context.Set<ModuleData>().AddAsync(Module);
                await _context.SaveChangesAsync();
                return Module;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el Module");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(ModuleData Module)
        {
            try
            {
                _context.Set<ModuleData>().Update(Module);
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
                var Module = await _context.Set<ModuleData>().FindAsync(id);
                if (Module == null)
                    return false;

                _context.Set<ModuleData>().Remove(Module);
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


using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class FormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public FormData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<FormData>> GetFormsAsync()
        {
            string query = "SELECT * FROM data.Form";
            return (IEnumerable<FormData>)await _context.QueryAsync<IEnumerable<FormData>>(query);
        }

        public async Task<FormData> GetFormAsync(int id)
        {
            try
            {

                //return await _context.QueryAsync<IEnumerable<Form>>(query);
                return await _context.Set<FormData>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Form con Id {FormId}", id);
                throw;
            }
        }

        public async Task<FormData> CreateAsync(FormData Form)
        {
            try
            {
                await _context.Set<FormData>().AddAsync(Form);
                await _context.SaveChangesAsync();
                return Form;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el Form");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(FormData Form)
        {
            try
            {
                _context.Set<FormData>().Update(Form);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el Form: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Form = await _context.Set<FormData>().FindAsync(id);
                if (Form == null)
                    return false;

                _context.Set<FormData>().Remove(Form);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el Form: {ex.Message}");
                return false;
            }
        }

    }
}

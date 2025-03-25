
using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PersonData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public PersonData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            string query = "SELECT * FROM data.Person";
            return (IEnumerable<Person>)await _context.QueryAsync<IEnumerable<Person>>(query);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            try
            {

                //return await _context.QueryAsync<IEnumerable<Person>>(query);
                return await _context.Set<Person>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Person con Id {PersonId}", id);
                throw;
            }
        }

        public async Task<Person> CreateAsync(Person Person)
        {
            try
            {
                await _context.Set<Person>().AddAsync(Person);
                await _context.SaveChangesAsync();
                return Person;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crer el Person");
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Person Person)
        {
            try
            {
                _context.Set<Person>().Update(Person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el Person: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Person = await _context.Set<Person>().FindAsync(id);
                if (Person == null)
                    return false;

                _context.Set<Person>().Remove(Person);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el Person: {ex.Message}");
                return false;
            }
        }

    }
}

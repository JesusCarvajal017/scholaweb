using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Data.interfaces;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class GenericData<T> : CrudBase<T> where T : class 
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<T> _logger;
        public GenericData(ApplicationDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        // SELECT ALL
        public virtual async Task<IEnumerable<T>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Formas");
                throw;
            }
        }

        // SELECT BY ID
        public virtual async Task<T?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Forma por id {id}");
                throw;
            }
        }

        // INSERT 
        public virtual async Task<T> CreateAsyncLinq(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Forma {entity}");
                throw;
            }
        }

        // UPDATE
        public virtual async Task<bool> UpdateAsyncLinq(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {entity}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public virtual async Task<Object> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await GetByIdAsyncLinq(id);

                if (Delete == null) return new { status = false }; // usuario inexistente

                _context.Set<T>().Remove(Delete);
                await _context.SaveChangesAsync();
                return new { status = true };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
        public virtual async Task<Object> DeleteLogicalAsyncLinq(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null) return new { status = false };

                await _context.SaveChangesAsync();
                return new { status = true };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }
    }
}


using System.Collections.Generic;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class ModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleData> _logger;
        public ModuleData(ApplicationDbContext context, ILogger<ModuleData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT * FROM ""Module"" WHERE ""Status"" = 1
                                        ORDER BY ""Id"" ASC ;";
                return await _context.QueryAsync<Module>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Moduleas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Module?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT * FROM public.""Module"" WHERE ""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Module>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Modulea por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Module> CreateAsync(Module Module)
        {
            try
            {
                const string query = @"
            
                            INSERT INTO public.""Module""(
	                            ""Name"", ""Description"", ""Status"")
	                            VALUES (@Name, @Description, @Status)
                            RETURNING ""Id"";";

                var parameters = new
                {
                    Module.Name,
                    Module.Description,
                    Status = 1
                };

                Module.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Module.Status = 1;

                return Module;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Modulea {Module}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Module Module)
        {
                try
                {
                const string query = @"
                                  UPDATE public.""Module""
	                                SET ""Name""=@Name, ""Description""=@Description
	                               WHERE ""Id""=@Id;";

                var parameters = new
                {
                    Module.Id,
                    Module.Name,
                    Module.Description
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Module}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM public.""Module""
                                        WHERE ""Id"" = @Id";
                var parameters = new { Id = id };
                var delete = await _context.ExecuteAsync(query, parameters);
                return new { rowAfefects = delete };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
        public async Task<Object> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE public.""Module""
                                SET ""Status"" = 0
                                WHERE ""Id"" = @Id;";

                var parameters = new { Id = id };
                var deleteLogical = await _context.ExecuteAsync(query, parameters);
                return new { rowAfectes = deleteLogical };

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar delete lógico: {ex.Message}");
                return false;
            }
        }


        //======================================______________  LINQ  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Module>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Module>()
                .Where(p => p.Status == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Moduleas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Module?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Module>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Modulea por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Module> CreateAsyncLinq(Module Module)
        {
            try
            {
                Module.Status = 0; // Establece el estado de la Modulea (activo e inactivo)
                await _context.Set<Module>().AddAsync(Module);
                await _context.SaveChangesAsync();
                return Module;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Modulea {Module}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(Module Module)
        {
            try
            {
                _context.Set<Module>().Update(Module);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Module}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<Module>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<Module>().Remove(Delete);
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
                var entity = await _context.Set<Module>().FindAsync(id);
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
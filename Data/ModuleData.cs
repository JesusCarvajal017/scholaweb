
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class ModuleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;
        public ModuleData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }


        //====================================== consultas por medio de SQL ====================================== 

        //Método para obtener todos los Moduleas con SQL
        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT * FROM Module WHERE Status = 0;";
                return await _context.QueryAsync<Module>(query);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Modulo");
                throw;
            }
        }


        //Método para obtener por Id con SQL
        public async Task<Module?> GetByIdAsync(int id)
        {
            try
            {
                const string query = "SELECT * FROM Module WHERE Id = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Module>(query, parameters);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Modulea por id {id}");
                throw;
            }
        }


        //====================================== consultas por medio de LINQ ====================================== 

        //Método para obtener todos los Moduleas con LINQ
        public async Task<IEnumerable<Module>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Module>()
                .Where(p => p.Status == 0)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Moduleas");
                throw;
            }
        }


        //Método para obtener por Id con LINQ
        public async Task<Module?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Module>().FindAsync(id);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Modulea por id {id}");
                throw;
            }
        }

        // ============================= Create Module SLQ =============================
        public async Task<Module> CreateAsync(Module Module)
        {
            try
            {
                const string query = @"
                            INSERT INTO Module (Name, Description, Code, Status)
                            OUTPUT INSERTED.Id
                            VALUES (@Name, @Description, @code, @Status);";

                var parameters = new
                {
                    Name = Module.Name,
                    Description = Module.Description,
                    Code = Module.Code,
                    Status = Module.Status
                };

                Module.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Module.Status = 0; // Establece el estado de la Modulo (activo e inactivo)
                return Module;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Modulea {Module}");
                throw;
            }
        }


        // ============================= Create Module LINQ =============================
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
                logger.LogError(ex, $"no se pudo agregar Modulea {Module}");
                throw;
            }
        }


        // ============================= Update Module sql==================
        public async Task<bool> UpdateAsync(Module Module)
        {
            try
            {
                const string query = @"
                                    UPDATE Module
                                    SET
                                        Name = @Name,
                                        Description = @Description,
                                        code = @code,
                                        Status = @Status,
                                    WHERE Id = @Id; ";

                var parameters = new
                {
                    Name = Module.Name,
                    Description = Module.Description,
                    Code = Module.Code,
                    Status = Module.Status
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {Module}");
                throw;

            }
        }

        // ============================= Update Module LINQ =========================
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
                logger.LogError(ex, $"No se pudo actualizar {Module}");
                throw;

            }
        }


        // ======================================================= Delte Persistent =======================================================

        // ============================= Delte Module sql =========================
        public async Task<bool> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Module
                                        WHERE Id = @Id";

                var parameters = new { Id = id };
                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // ============================= Delte Module LINQ =========================
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
                logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }


        // ======================================================= Delte Logical =======================================================

        // ============================= Delte Module  SQL =========================
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE Module 
                                        SET Status = 1 
                                        WHERE Id = @Id";
                var parameters = new { Id = id };
                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error al realizar delete lógico: {ex.Message}");
                return false;
            }
        }


        // ============================= Delte Module  LINQ =========================
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
                logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }

    }
}
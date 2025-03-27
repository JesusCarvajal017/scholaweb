
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;
        public PermissionData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }


        //====================================== consultas por medio de SQL ====================================== 

        //Método para obtener todos los Permissionas con SQL
        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT * FROM Permission WHERE Status = 0;";
                return await _context.QueryAsync<Permission>(query);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Permissionas");
                throw;
            }
        }


        //Método para obtener por Id con SQL
        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                const string query = "SELECT * FROM Permission WHERE Id = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Permission>(query, parameters);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Permissiona por id {id}");
                throw;
            }
        }


        //====================================== consultas por medio de LINQ ====================================== 
        //Método para obtener todos los Permissionas con LINQ
        public async Task<IEnumerable<Permission>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Permission>()
                .Where(p => p.Status == 0)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Permissionas");
                throw;
            }
        }


        //Método para obtener por Id con LINQ
        public async Task<Permission?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Permission>().FindAsync(id);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Permissiona por id {id}");
                throw;
            }
        }

        // ============================= Create Permission SLQ =============================
        public async Task<Permission> CreateAsync(Permission Permission)
        {
            try
            {
                const string query = @"

                            INSERT INTO Permission (Name, Description, Status)
                            OUTPUT INSERTED.Id
                            VALUES (@Name, @Description, @Status);";

                var parameters = new
                {
                    Name = Permission.Name,
                    Description = Permission.Description,
                    Status = Permission.Status
                };

                Permission.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Permission.Status = 0; // Establece el estado de la Permissiona (activo e inactivo)
                return Permission;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Permissiona {Permission}");
                throw;
            }
        }


        // ============================= Create Permission LINQ =============================
        public async Task<Permission> CreateAsyncLinq(Permission Permission)
        {
            try
            {
                Permission.Status = 0; // Establece el estado de la Permissiona (activo e inactivo)
                await _context.Set<Permission>().AddAsync(Permission);
                await _context.SaveChangesAsync();
                return Permission;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Permissiona {Permission}");
                throw;
            }
        }


        // ============================= Update Permission sql==================
        public async Task<bool> UpdateAsync(Permission Permission)
        {
            try
            {
                const string query = @"
                                    UPDATE Permission
                                    SET
                                        Name = @Name,
                                        Description = @Description,
                                        Status = @Status,
                                    WHERE Id = @Id; ";

                var parameters = new
                {
                    Name = Permission.Name,
                    Description = Permission.Description,
                    Status = Permission.Status
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {Permission}");
                throw;

            }
        }

        // ============================= Update Permission LINQ =========================
        public async Task<bool> UpdateAsyncLinq(Permission Permission)
        {
            try
            {
                _context.Set<Permission>().Update(Permission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {Permission}");
                throw;

            }
        }


        // ======================================================= Delte Persistent =======================================================

        // ============================= Delte Permission sql =========================
        public async Task<bool> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Permission
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

        // ============================= Delte Permission LINQ =========================
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<Permission>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<Permission>().Remove(Delete);
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

        // ============================= Delte Permission  =========================
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE Permission 
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


        //Método para Eliminar lógico linq
        public async Task<bool> DeleteLogicalAsyncLinq(int id)
        {
            try
            {
                var entity = await _context.Set<Permission>().FindAsync(id);
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
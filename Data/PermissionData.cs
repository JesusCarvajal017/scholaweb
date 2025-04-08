
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionData> _logger;
        public PermissionData(ApplicationDbContext context, ILogger<PermissionData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT * FROM Permission WHERE ""Status"" = 1
                                        ORDER BY ""Id"" ASC ;";
                return await _context.QueryAsync<Permission>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a los permisos");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT * FROM Permission WHERE ""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Permission>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Permissiona por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Permission> CreateAsync(Permission Permission)
        {
            try
            {
                const string query = @"
                           INSERT INTO public.permission(
	                        ""Name"", ""Description"", ""Status"")
	                        VALUES (@Name, @Description, @Status);";

                var parameters = new
                {
                    Permission.Name,
                    Permission.Description,
                    Status = 1
                };

                Permission.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Permission.Status = 1;

                return Permission;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Permissiona {Permission}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Permission Permission)
        {
            try
            {
                const string query = @"
                                  UPDATE public.permission
	                                SET ""Name""=@Name, ""Description""=@Description
	                                WHERE ""Id""=@Id;";

                var parameters = new
                {
                    Permission.Id,
                    Permission.Name,
                    Permission.Description
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Permission}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Permission
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
                const string query = @"UPDATE Permission 
                                        SET ""Status"" = 0 
                                        WHERE ""Id"" = @Id";
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
        public async Task<IEnumerable<Permission>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Permission>()
                .Where(p => p.Status == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Permissionas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Permission?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Permission>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Permissiona por id {id}");
                throw;
            }
        }

        // INSERT 
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
                _logger.LogError(ex, $"no se pudo agregar Permissiona {Permission}");
                throw;
            }
        }

        // UPDATE
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
                _logger.LogError(ex, $"No se pudo actualizar {Permission}");
                throw;

            }
        }

        // DELETE PERSISTENT
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
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
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
                _logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }

    }
}
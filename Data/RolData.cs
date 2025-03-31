using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolData> _logger;
        public RolData(ApplicationDbContext context, ILogger<RolData> logger)
        {
            _context = context;
            this._logger = logger;
        }



        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT * FROM rol WHERE ""Status"" = 1
                                        ORDER BY ""Id"" ASC ;";
                return await _context.QueryAsync<Rol>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Rolas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Rol?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT * FROM rol WHERE ""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Rol>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Rola por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Rol> CreateAsync(Rol Rol)
        {
            try
            {
                const string query = @"
                            INSERT INTO Rol (""Name"", ""Code"", ""Description"", ""Status"")
                                VALUES (@Name, @Code, @Description, @Status)
                            RETURNING ""Id"";";

                var parameters = new
                {
                    Name = Rol.Name,
                    Code = Rol.Code,
                    Description = Rol.Description,
                    Status = Rol.Status
                };

                Rol.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Rol.Status = 1;
                return Rol;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Rola {Rol}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Rol Rol)
        {
            try
            {
                const string query = @"
                                    UPDATE Rol
                                    SET
                                        ""Name"" = @Name,
                                        ""Code"" = @Code,
                                        ""Description"" = @Description,
                                        ""Status"" = @Status
                                    WHERE ""Id"" = @Id; ";

                var parameters = new
                {
                    Name = Rol.Name,
                    Code = Rol.Code,
                    Description = Rol.Description,
                    Status = Rol.Status
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Rol}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Rol
                                        WHERE Id = @Id";
                var parameters = new { Id = id };
                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE Rol 
                                        SET IsDeleted = 1 
                                        WHERE Id = @Id";
                var parameters = new { Id = id };
                await _context.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar delete lógico: {ex.Message}");
                return false;
            }
        }


        //======================================______________  LINQ  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Rol>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Rol>()
                .Where(p => p.Status == 0)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Rolas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Rol?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Rol>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Rola por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Rol> CreateAsyncLinq(Rol Rol)
        {
            try
            {
                Rol.Status = 0; // Establece el estado de la Rola (activo e inactivo)
                await _context.Set<Rol>().AddAsync(Rol);
                await _context.SaveChangesAsync();
                return Rol;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Rola {Rol}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(Rol Rol)
        {
            try
            {
                _context.Set<Rol>().Update(Rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Rol}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<Rol>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<Rol>().Remove(Delete);
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
                var entity = await _context.Set<Rol>().FindAsync(id);
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
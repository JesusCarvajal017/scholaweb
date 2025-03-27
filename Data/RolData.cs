using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;
        public RolData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }


        //====================================== consultas por medio de SQL ====================================== 

        //Método para obtener todos los Rolas con SQL
        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT * FROM Rol WHERE Status = 0;";
                return await _context.QueryAsync<Rol>(query);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Rolas");
                throw;
            }
        }


        //Método para obtener por Id con SQL
        public async Task<Rol?> GetByIdAsync(int id)
        {
            try
            {
                const string query = "SELECT * FROM Rol WHERE Id = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Rol>(query, parameters);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Rola por id {id}");
                throw;
            }
        }


        //====================================== consultas por medio de LINQ ====================================== 

        //Método para obtener todos los Rolas con LINQ
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
                logger.LogInformation(ex, "No se pudo obetner a las Rolas");
                throw;
            }
        }


        //Método para obtener por Id con LINQ
        public async Task<Rol?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Rol>().FindAsync(id);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Rola por id {id}");
                throw;
            }
        }

        // ============================= Create Rol SLQ =============================
        public async Task<Rol> CreateAsync(Rol Rol)
        {
            try
            {
                const string query = @"
                            INSERT INTO Rol (Name, Code, Description, Status)
                            OUTPUT INSERTED.Id
                            VALUES (@Name, @Code, @Description, @Status);";

                var parameters = new
                {
                    Name = Rol.Name,
                    Code = Rol.Code,
                    Description = Rol.Description,
                    Status = Rol.Status
                };

                Rol.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Rol.Status = 0; // Establece el estado de la Rola (activo e inactivo)
                return Rol;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Rola {Rol}");
                throw;
            }
        }


        // ============================= Create Rol LINQ =============================
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
                logger.LogError(ex, $"no se pudo agregar Rola {Rol}");
                throw;
            }
        }


        // ============================= Update Rol sql==================
        public async Task<bool> UpdateAsync(Rol Rol)
        {
            try
            {
                const string query = @"
                                    UPDATE Rol
                                    SET
                                        Name = @Name,
                                        Code = @Code,
                                        Description = @Description,
                                        Status = @Status
                                    WHERE Id = @Id; ";

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
                logger.LogError(ex, $"No se pudo actualizar {Rol}");
                throw;

            }
        }

        // ============================= Update Rol LINQ =========================
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
                logger.LogError(ex, $"No se pudo actualizar {Rol}");
                throw;

            }
        }


        // ======================================================= Delte Persistent =======================================================

        // ============================= Delte Rol sql =========================
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
                logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // ============================= Delte Rol LINQ =========================
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
                logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }


        // ======================================================= Delte Logical =======================================================

        // ============================= Delte Rol  =========================
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
                logger.LogInformation($"Error al realizar delete lógico: {ex.Message}");
                return false;
            }
        }


        //Método para Eliminar lógico linq
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
                logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }

    }
}
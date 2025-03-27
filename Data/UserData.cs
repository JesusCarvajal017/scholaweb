
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;
        public UserData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }


        //====================================== consultas por medio de SQL ====================================== 

        //Método para obtener todos los Useras con SQL
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT * FROM User WHERE Status = 0;";
                return await _context.QueryAsync<User>(query);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Useras");
                throw;
            }
        }


        //Método para obtener por Id con SQL
        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                const string query = "SELECT * FROM User WHERE Id = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<User>(query, parameters);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Usera por id {id}");
                throw;
            }
        }


        //====================================== consultas por medio de LINQ ====================================== 

        //Método para obtener todos los Useras con LINQ
        public async Task<IEnumerable<User>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<User>()
                .Where(p => p.Status == 0)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Useras");
                throw;
            }
        }


        //Método para obtener por Id con LINQ
        public async Task<User?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Usera por id {id}");
                throw;
            }
        }

        // ============================= Create User SLQ =============================
        public async Task<User> CreateAsync(User User)
        {
            try
            {
                const string query = @"
                            INSERT INTO User (PersonId, UserName,Password, Status)
                            OUTPUT INSERTED.Id
                            VALUES (@PersonId,@UserName, @Password, @@Status);";

                var parameters = new
                {
                    PersonId = User.PersonId,
                    UserName = User.UserName,
                    Password = User.Password,
                    Status = User.Status
                };

                User.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                User.Status = 0; // Establece el estado de la User (activo e inactivo)
                return User;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Usera {User}");
                throw;
            }
        }


        // ============================= Create User LINQ =============================
        public async Task<User> CreateAsyncLinq(User User)
        {
            try
            {
                User.Status = 0; // Establece el estado de la Usera (activo e inactivo)
                await _context.Set<User>().AddAsync(User);
                await _context.SaveChangesAsync();
                return User;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Usera {User}");
                throw;
            }
        }


        // ============================= Update User sql==================
        public async Task<bool> UpdateAsync(User User)
        {
            try
            {
                const string query = @"
                                    UPDATE User
                                    SET
                                        UserName = @UserName,
                                        Password = @Password,
                                        Status = @Status
                                    WHERE Id = @Id; ";

                var parameters = new
                {
                    UserName = User.UserName,
                    Password = User.Password,
                    Status = User.Status
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {User}");
                throw;

            }
        }

        // ============================= Update User LINQ =========================
        public async Task<bool> UpdateAsyncLinq(User User)
        {
            try
            {
                _context.Set<User>().Update(User);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {User}");
                throw;

            }
        }


        // ======================================================= Delte Persistent =======================================================

        // ============================= Delte User sql =========================
        public async Task<bool> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM User
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

        // ============================= Delte User LINQ =========================
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<User>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<User>().Remove(Delete);
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

        // ============================= Delte User  =========================
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE User 
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
                var entity = await _context.Set<User>().FindAsync(id);
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
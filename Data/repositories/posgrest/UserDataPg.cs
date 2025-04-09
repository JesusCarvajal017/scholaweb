using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.interfaces;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.posgrest
{
    public class UserDataPg: IGlobalCrud<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserDataPg> _logger;
        public UserDataPg(ApplicationDbContext context, ILogger<UserDataPg> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT * FROM ""user"" WHERE ""Status"" = 1
                                        ORDER BY ""Id"" ASC;";
                return await _context.QueryAsync<User>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Useras");
                throw;
            }

        }

        // SELECT BY ID
        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT * FROM public.""user"" WHERE ""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<User>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Usera por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<User> CreateAsync(User User)
        {
            try
            {
                const string query = @"
                           INSERT INTO public.""user""(
	                            ""UserName"", ""Password"", ""Status"", ""PersonId"")
	                       VALUES (@UserName, @Password, @Status, @PersonId)
                            RETURNING ""Id"";";

                var parameters = new
                {
                    User.UserName,
                    User.Password,
                    User.PersonId,
                    Status = 1
                };

                User.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                User.Status = 1;

                return User;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar User {User}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(User User)
        {
            try
            {
                const string query = @"
                                    UPDATE public.""user""
	                                    SET ""UserName""=@UserName, 
                                            ""Password""=@Password, 
                                            ""PersonId""=@PersonId
	                                WHERE ""Id"" = @Id;";

                var parameters = new
                {
                    User.Id,
                    User.UserName,
                    User.Password,
                    User.PersonId
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {User}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM ""user""
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
                const string query = @"UPDATE ""user"" 
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
        public async Task<IEnumerable<User>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<User>()
                .Where(p => p.Status == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Useras");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<User?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Usera por id {id}");
                throw;
            }
        }

        // INSERT 
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
                _logger.LogError(ex, $"no se pudo agregar Usera {User}");
                throw;
            }
        }

        // UPDATE
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
                _logger.LogError(ex, $"No se pudo actualizar {User}");
                throw;

            }
        }

        // DELETE PERSISTENT
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
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
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
                _logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }
    }
}

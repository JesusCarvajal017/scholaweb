
using Entity;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UserRolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRolData> _logger;
        public UserRolData(ApplicationDbContext context, ILogger<UserRolData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<UserRolQueryDto>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT ur.""Id"" AS idRolUser, ur.""UserId"",us.""UserName"", ur.""RolId"", rl.""Name"" AS ""RolName"" 
	                                        FROM ""userRol"" AS ur
                                        INNER JOIN ""user"" AS us
	                                        ON ur.""UserId"" = us.""Id""
                                        INNER JOIN rol AS  rl
	                                        ON ur.""RolId"" = rl.""Id""
                                        ORDER BY ur.""Id"" ASC;";

                

                return await _context.QueryAsync<UserRolQueryDto>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las UserRol");
                throw;
            }

        }

        // SELECT BY ID
        public async Task<UserRolQueryDto?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT ur.""Id"" AS idRolUser, ur.""UserId"",us.""UserName"", ur.""RolId"", rl.""Name"" AS ""RolName"" 
	                                        FROM ""userRol"" AS ur
                                        INNER JOIN ""user"" AS us
	                                        ON ur.""UserId"" = us.""Id""
                                        INNER JOIN rol AS  rl
	                                        ON ur.""RolId"" = rl.""Id""
                                        WHERE ur.""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<UserRolQueryDto>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una UserRola por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<UserRol> CreateAsync(UserRol UserRol)
        {
            try
            {
                const string query = @"
                          INSERT INTO public.""userRol""(
	                        ""UserId"", ""RolId"")
	                       VALUES (@UserId, @RolId)
                            RETURNING ""Id"";";

                var parameters = new
                {
                    UserRol.UserId,
                    UserRol.RolId
                };

                UserRol.Id = await _context.ExecuteScalarAsync<int>(query, parameters);

                return UserRol;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar UserRol {UserRol}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(UserRol UserRol)
        {
            try
            {
                const string query = @"
                                   UPDATE public.""userRol""
	                                SET ""UserId""=@UserId, ""RolId""=@RolId
	                               WHERE ""Id""=@Id;";

                var parameters = new
                {
                    UserRol.Id,
                    UserRol.UserId,
                    UserRol.RolId
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {UserRol}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM public.""userRol""
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
                const string query = @"UPDATE ""UserRol"" 
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
        public async Task<IEnumerable<UserRol>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<UserRol>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las UserRolas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<UserRol?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<UserRol>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una UserRola por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<UserRol> CreateAsyncLinq(UserRol UserRol)
        {
            try
            {
                await _context.Set<UserRol>().AddAsync(UserRol);
                await _context.SaveChangesAsync();
                return UserRol;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar UserRola {UserRol}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(UserRol UserRol)
        {
            try
            {
                _context.Set<UserRol>().Update(UserRol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {UserRol}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<UserRol>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<UserRol>().Remove(Delete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }
    }
}

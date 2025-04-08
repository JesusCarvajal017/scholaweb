using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RolFormPermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolFormPermissionData> _logger;
        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermissionData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<RolFormPermissionDto>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT rfp.""Id"", ""RolId"", rl.""Name"" AS ""RolName"", ""FormId"", fr.""Name"" AS ""FormName"", ""PermissionId"", prmsion.""Name"" AS ""PermissionName""
	                                    FROM public.""RolFormPermission"" AS rfp
                                    INNER JOIN public.rol AS rl	
	                                    ON rfp.""RolId"" = rl.""Id""
                                    INNER JOIN public.form AS fr	
	                                    ON rfp.""FormId"" = fr.""Id""
                                    INNER JOIN public.permission AS prmsion
	                                    ON rfp.""PermissionId"" = prmsion.""Id""
                                        ORDER BY rfp.""Id"" ASC;";
                return await _context.QueryAsync<RolFormPermissionDto>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las RolFormPermission");
                throw;
            }

        }

        // SELECT BY ID
        public async Task<RolFormPermissionDto?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT rfp.""Id"", ""RolId"", rl.""Name"" AS ""RolName"", ""FormId"", fr.""Name"" AS ""FormName"", ""PermissionId"", prmsion.""Name"" AS ""PermissionName""
	                                    FROM public.""RolFormPermission"" AS rfp
                                    INNER JOIN public.rol AS rl	
	                                    ON rfp.""RolId"" = rl.""Id""
                                    INNER JOIN public.form AS fr	
	                                    ON rfp.""FormId"" = fr.""Id""
                                    INNER JOIN public.permission AS prmsion
	                                    ON rfp.""PermissionId"" = prmsion.""Id""
                                        WHERE rfp.""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<RolFormPermissionDto>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una RolFormPermissiona por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<RolFormPermission> CreateAsync(RolFormPermission RolFormPermission)
        {
            try
            {
                const string query = @"
                          INSERT INTO public.""RolFormPermission""(
	                       ""UserId"", ""RolId"")
	                        VALUES (@UserId, @RolId);
                            RETURNING ""Id"";";

                var parameters = new
                {
                    RolFormPermission.PermissionId,
                    RolFormPermission.FormId
                };

                RolFormPermission.Id = await _context.ExecuteScalarAsync<int>(query, parameters);

                return RolFormPermission;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar RolFormPermission {RolFormPermission}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(RolFormPermission RolFormPermission)
        {
            try
            {
                const string query = @"
                                    UPDATE public.""RolFormPermission""
	                                    SET ""RolFormPermissionName""=@RolFormPermissionName, 
                                            ""Password""=@Password, 
                                            ""PersonId""=@PersonId
	                                WHERE ""Id"" = @Id;";

                var parameters = new
                {
                    RolFormPermission.PermissionId,
                    RolFormPermission.RolId
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {RolFormPermission}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM ""RolFormPermission""
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
                const string query = @"UPDATE ""RolFormPermission"" 
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
        public async Task<IEnumerable<RolFormPermission>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<RolFormPermission>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las RolFormPermissionas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<RolFormPermission?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<RolFormPermission>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una RolFormPermissiona por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<RolFormPermission> CreateAsyncLinq(RolFormPermission RolFormPermission)
        {
            try
            {
                await _context.Set<RolFormPermission>().AddAsync(RolFormPermission);
                await _context.SaveChangesAsync();
                return RolFormPermission;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar RolFormPermissiona {RolFormPermission}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(RolFormPermission RolFormPermission)
        {
            try
            {
                _context.Set<RolFormPermission>().Update(RolFormPermission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {RolFormPermission}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<RolFormPermission>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<RolFormPermission>().Remove(Delete);
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

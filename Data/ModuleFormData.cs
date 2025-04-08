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
    public class ModuleFormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleFormData> _logger;
        public ModuleFormData(ApplicationDbContext context, ILogger<ModuleFormData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<ModuleFormDto>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT mf.""Id"", ""ModuleId"", ml.""Name"" AS ""ModuleName"", ""FormId"" , fr.""Name"" AS ""FormName""
	                                    FROM public.""ModuleForm"" AS mf
                                    INNER JOIN public.""Module"" AS ml
	                                    ON mf.""ModuleId"" = ml.""Id""
                                    INNER JOIN public.form AS fr
	                                    ON mf.""FormId"" = fr.""Id"";";

                return await _context.QueryAsync<ModuleFormDto>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las ModuleForm");
                throw;
            }

        }

        // SELECT BY ID
        public async Task<ModuleFormDto?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT mf.""Id"", ""ModuleId"", ml.""Name"" AS ""ModuleName"", ""FormId"" , fr.""Name"" AS ""FormName""
	                                    FROM public.""ModuleForm"" AS mf
                                    INNER JOIN public.""Module"" AS ml
	                                    ON mf.""ModuleId"" = ml.""Id""
                                    INNER JOIN public.form AS fr
	                                    ON mf.""FormId"" = fr.""Id""
                                     WHERE mf.""Id"" = @Id;";

                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<ModuleFormDto>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una ModuleForma por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<ModuleForm> CreateAsync(ModuleForm ModuleForm)
        {
            try
            {
                const string query = @"
                          INSERT INTO public.""ModuleForm""(
	                            ""ModuleId"", ""FormId"")
	                            VALUES (@ModuleId, @FormId)
                            RETURNING ""Id"";";

                var parameters = new
                {
                    ModuleForm.FormId,
                    ModuleForm.ModuleId
                };

                ModuleForm.Id = await _context.ExecuteScalarAsync<int>(query, parameters);

                return ModuleForm;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar ModuleForm {ModuleForm}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(ModuleForm ModuleForm)
        {
            try
            {
                const string query = @"
                                  UPDATE public.""ModuleForm""
	                                SET ""ModuleId""=@ModuleId, ""FormId""=@FormId
	                               WHERE ""Id""=Id;";

                var parameters = new
                {
                    ModuleForm.Id,
                    ModuleForm.ModuleId,
                    ModuleForm.FormId
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {ModuleForm}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM ""ModuleForm""
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
                const string query = @"UPDATE ""ModuleForm"" 
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
        public async Task<IEnumerable<ModuleForm>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<ModuleForm>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las ModuleFormas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<ModuleForm?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<ModuleForm>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una ModuleForma por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<ModuleForm> CreateAsyncLinq(ModuleForm ModuleForm)
        {
            try
            {
                await _context.Set<ModuleForm>().AddAsync(ModuleForm);
                await _context.SaveChangesAsync();
                return ModuleForm;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar ModuleForma {ModuleForm}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(ModuleForm ModuleForm)
        {
            try
            {
                _context.Set<ModuleForm>().Update(ModuleForm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {ModuleForm}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<ModuleForm>().FindAsync(id);

                if (Delete == null) return false; 

                _context.Set<ModuleForm>().Remove(Delete);
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

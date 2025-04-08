using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class FormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormData> _logger;
        public FormData(ApplicationDbContext context, ILogger<FormData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Form>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT * FROM Form WHERE ""Status"" = 1
                                        ORDER BY ""Id"" ASC ;";
                return await _context.QueryAsync<Form>(query);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Formas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Form?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT * FROM Form WHERE ""Id"" = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Form>(query, parameters);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Forma por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Form> CreateAsync(Form Form)
        {
            try
            {
                const string query = @"
                                INSERT INTO public.form(
	                            ""Name"", ""Description"", ""Status"")
	                            VALUES (@Name, @Description, @Status)
                               RETURNING ""Id"";";

                var parameters = new
                {
                    Name = Form.Name,
                    Description = Form.Description,
                    Status = 1
                };

                Form.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Form.Status = 1;

                return Form;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Forma {Form}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Form Form)
        {
            try
            {
                const string query = @"
                                  UPDATE public.form
	                                SET ""Name""=@Name, ""Description""=@Description
	                                WHERE ""Id"" = @Id;";

                var parameters = new
                {
                    Form.Id,
                    Form.Name,
                    Form.Description
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Form}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Form
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
                const string query = @"UPDATE public.form 
                                        SET ""Status"" = 0 
                                        WHERE ""Id"" = @Id";
                var parameters = new { Id = id };
                var deleteLogical = await _context.ExecuteAsync(query, parameters);
                return new { rowAfectes = deleteLogical };

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar delete lógico: {ex.Message}");
                return new { Status = false };
            }
        }


        //======================================______________  LINQ  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Form>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Form>()
                .Where(p => p.Status == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Formas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Form?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Form>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Forma por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Form> CreateAsyncLinq(Form Form)
        {
            try
            {
                Form.Status = 0; // Establece el estado de la Forma (activo e inactivo)
                await _context.Set<Form>().AddAsync(Form);
                await _context.SaveChangesAsync();
                return Form;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Forma {Form}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(Form Form)
        {
            try
            {
                _context.Set<Form>().Update(Form);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Form}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<Form>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<Form>().Remove(Delete);
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
                var entity = await _context.Set<Form>().FindAsync(id);
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
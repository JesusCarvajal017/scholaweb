using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class FormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;
        public FormData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }


        //====================================== consultas por medio de SQL ====================================== 

        //Método para obtener todos los Formas con SQL
        public async Task<IEnumerable<Form>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT * FROM Form WHERE Status = 0;";
                return await _context.QueryAsync<Form>(query);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Formas");
                throw;
            }
        }


        //Método para obtener por Id con SQL
        public async Task<Form?> GetByIdAsync(int id)
        {
            try
            {
                const string query = "SELECT * FROM Form WHERE Id = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Form>(query, parameters);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Forma por id {id}");
                throw;
            }
        }


        //====================================== consultas por medio de LINQ ====================================== 

        //Método para obtener todos los Formas con LINQ
        public async Task<IEnumerable<Form>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Form>()
                .Where(p => p.Status == 0)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las Formas");
                throw;
            }
        }


        //Método para obtener por Id con LINQ
        public async Task<Form?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Form>().FindAsync(id);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una Forma por id {id}");
                throw;
            }
        }

        // ============================= Create Form SLQ =============================
        public async Task<Form> CreateAsync(Form Form)
        {
            try
            {
                const string query = @"
                            INSERT INTO Form (Name, Description, Status)
                            OUTPUT INSERTED.Id
                            VALUES (@Name, @Description, @Status);";

                var parameters = new
                {
                    Name = Form.Name,
                    Description = Form.Description,
                    Status = Form.Status
                };

                Form.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Form.Status = 0; // Establece el estado de la Forma (activo e inactivo)
                return Form;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar Forma {Form}");
                throw;
            }
        }


        // ============================= Create Form LINQ =============================
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
                logger.LogError(ex, $"no se pudo agregar Forma {Form}");
                throw;
            }
        }


        // ============================= Update Form sql==================
        public async Task<bool> UpdateAsync(Form Form)
        {
            try
            {
                const string query = @"
                                    UPDATE Form
                                    SET
                                        Name = @Name,
                                        Description = @Description,
                                        Status = @Status,
                                    WHERE Id = @Id; ";

                var parameters = new
                {
                    Name = Form.Name,
                    Description = Form.Description,
                    Status = Form.Status
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {Form}");
                throw;

            }
        }

        // ============================= Update Form LINQ =========================
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
                logger.LogError(ex, $"No se pudo actualizar {Form}");
                throw;

            }
        }


        // ======================================================= Delte Persistent =======================================================

        // ============================= Delte Form sql =========================
        public async Task<bool> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Form
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

        // ============================= Delte Form LINQ =========================
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
                logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }


        // ======================================================= Delte Logical =======================================================

        // ============================= Delte Form  =========================
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE Form 
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
                var entity = await _context.Set<Form>().FindAsync(id);
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
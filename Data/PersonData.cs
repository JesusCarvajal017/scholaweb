
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PersonData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger logger;
        public PersonData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            this.logger = logger;
        }


        //====================================== consultas por medio de SQL ====================================== 

        //Método para obtener todos los Personas con SQL
        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT * FROM Person WHERE Status = 0;";
                return await _context.QueryAsync<Person>(query);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las personas");
                throw;
            }
        }


        //Método para obtener por Id con SQL
        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                const string query = "SELECT * FROM Person WHERE Id = @Id;";
                var parameters = new { Id = id };
                return await _context.QueryFirstOrDefaultAsync<Person>(query, parameters);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una persona por id {id}");
                throw;
            }
        }


        //====================================== consultas por medio de LINQ ====================================== 

        //Método para obtener todos los Personas con LINQ
        public async Task<IEnumerable<Person>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Person>()
                .Where(p => p.Status == 0)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "No se pudo obetner a las personas");
                throw;
            }
        }


        //Método para obtener por Id con LINQ
        public async Task<Person?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Person>().FindAsync(id);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al traer una persona por id {id}");
                throw;
            }
        }

        // ============================= Create Person SLQ =============================
        public async Task<Person> CreateAsync(Person person)
        {
            try
            {
                const string query = @"
                            INSERT INTO Person (Name, LastName, Email, Identification, Age, Status)
                            OUTPUT INSERTED.Id
                            VALUES (@Name, @LastName, @Email, @Identification, @Age, @Status);";

                var parameters = new
                {
                    Name = person.Name,                        
                    LastName = person.LastName,               
                    Email = person.Email,                      
                    Identification = person.Identification,        
                    Age = person.Age,              
                    Status = person.Status    
                };

                person.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                person.Status = 0; // Establece el estado de la persona (activo e inactivo)
                return person;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar persona {person}");
                throw;
            }
        }


        // ============================= Create Person LINQ =============================
        public async Task<Person> CreateAsyncLinq(Person person)
        {
            try
            {
                person.Status = 0; // Establece el estado de la persona (activo e inactivo)
                await _context.Set<Person>().AddAsync(person);
                await _context.SaveChangesAsync();
                return person;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"no se pudo agregar persona {person}");
                throw;
            }
        }


        // ============================= Update Person sql==================
        public async Task<bool> UpdateAsync(Person person)
        {
            try
            {
                const string query = @"
                                    UPDATE Person
                                    SET
                                        Name = @Name,
                                        LastName = @LastName,
                                        Email = @Email,
                                        Identification = @Identification,
                                        Age = @Age,
                                        Status = @Status,
                                    WHERE Id = @Id; ";

                var parameters = new
                {
                    Name = person.Name,
                    LastName = person.LastName,
                    Email = person.Email,
                    Identification = person.Identification,
                    Age = person.Age,
                    Status = person.Status
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {person}");
                throw;

            }
        }

        // ============================= Update Person LINQ =========================
        public async Task<bool> UpdateAsyncLinq(Person person)
        {
            try
            {
                _context.Set<Person>().Update(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"No se pudo actualizar {person}");
                throw;

            }
        }


        // ======================================================= Delte Persistent =======================================================

        // ============================= Delte Person sql =========================
        public async Task<bool> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Person
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

        // ============================= Delte Person LINQ =========================
        public async Task<bool> DeletePersistentAsyncLinq(int id)
        {
            try
            {
                var Delete = await _context.Set<Person>().FindAsync(id);

                if (Delete == null) return false; // usuario inexistente

                _context.Set<Person>().Remove(Delete);
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

        // ============================= Delte Person  =========================
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                const string query = @"UPDATE Person 
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
                var entity = await _context.Set<Person>().FindAsync(id);
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
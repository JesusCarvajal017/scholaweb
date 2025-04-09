using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.interfaces;
using Data.repositories.posgrest;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Mysql
{
    public class PersonData : IGlobalCrud<Person>
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonData> _logger;
        public PersonData(ApplicationDbContext context, ILogger<PersonData> logger)
        {
            _context = context;
            this._logger = logger;
        }

        //======================================______________  SQL  ______________====================================== 

        // SELECT ALL
        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT * FROM public.""Person""
                                        WHERE ""Status"" = 1
                                        ORDER BY ""Id"" ASC ;";

                return await _context.QueryAsync<Person>(query);

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Personas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT * FROM `segurity`.`person` WHERE `Status` = 1;";

                var parameters = new { Id = id };

                return await _context.QueryFirstOrDefaultAsync<Person>(query, parameters);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Persona por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Person> CreateAsync(Person Person) // { }
        {
            try
            {
                const string query = @"
                            INSERT INTO public.""Person""(
	                            ""Name"", ""LastName"", ""Email"", ""Identification"", ""Age"", ""Status"")
	                            VALUES (@Name, @LastNamee, @Email, @Identification, @Age, @Status)
                            RETURNING ""Id"";";

                var parameters = new
                {
                    Name = Person.Name,
                    LastNamee = Person.LastName,
                    Person.Email,
                    Person.Identification,
                    Person.Age,
                    Status = 1
                };

                Person.Id = await _context.ExecuteScalarAsync<int>(query, parameters);
                Person.Status = 1;

                return Person;

                // { }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Persona {Person}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Person Person) // <= { Id = 1, Name = "hola"}
        {
            try
            {
                const string query = @"
                                  UPDATE public.""Person""
	                                SET ""Name""=@Name, 
                                        ""LastName""=@LastName, 
                                        ""Email""=@Email, 
                                        ""Identification""=@Identification, 
                                        ""Age""=@Age
	                               WHERE ""Id"" = @Id;";

                var parameters = new
                {
                    Person.Id,
                    Person.Name,
                    Person.LastName,
                    Person.Email,
                    Person.Identification,
                    Person.Age
                };

                int rowsAffected = await _context.ExecuteAsync(query, parameters);

                return rowsAffected > 0; // true or false
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Person}");
                throw;

            }
        }

        // DELETE PERSISTENT
        public async Task<Object> DeletePersistentAsync(int id)
        {
            try
            {
                const string query = @"DELETE FROM Person
                                        WHERE ""Id"" = @Id";
                var parameters = new { Id = id };
                var delete = await _context.ExecuteAsync(query, parameters);
                return new { rowAfefects = delete }; // { rowAfefects = 1}
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
                const string query = @"UPDATE public.""Person"" 
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

        // ORM

        // SELECT ALL
        public async Task<IEnumerable<Person>> GetAllAsyncLinq()
        {
            try
            {
                return await _context.Set<Person>()
                .Where(p => p.Status == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se pudo obetner a las Personas");
                throw;
            }
        }

        // SELECT BY ID
        public async Task<Person?> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await _context.Set<Person>().FindAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer una Persona por id {id}");
                throw;
            }
        }

        // INSERT 
        public async Task<Person> CreateAsyncLinq(Person Person)
        {
            try
            {
                Person.Status = 1; // Establece el estado de la Persona (activo e inactivo)
                await _context.Set<Person>().AddAsync(Person);
                await _context.SaveChangesAsync();
                return Person;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"no se pudo agregar Persona {Person}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateAsyncLinq(Person Person)
        {
            try
            {
                _context.Set<Person>().Update(Person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar {Person}");
                throw;

            }
        }

        // DELETE PERSISTENT
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
                _logger.LogInformation($" error al eliminar {ex.Message}");
                return false;
            }
        }

        // DELETE LOGICAL
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
                _logger.LogInformation($"Error al realizar delete lógico con LINQ: {ex.Message}");
                return false;
            }
        }
    }
}

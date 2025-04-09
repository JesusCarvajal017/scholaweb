using Entity;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class PersonData : GenericData<Person>
    {

        public PersonData(ApplicationDbContext context, ILogger<Person> logger) : base(context, logger)
        {

        }
    }
}

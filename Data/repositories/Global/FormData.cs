using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.repositories.Global
{
    public class FormData : GenericData<Form>
    {
        public FormData(ApplicationDbContext context, ILogger<Form> logger) : base(context, logger)
        {

        }
    }
}
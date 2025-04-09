using Data.interfaces;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class RolData : GenericData<Rol>
    {
        public RolData(ApplicationDbContext context, ILogger<Rol> logger) : base(context, logger)
        {

        }

    }
}
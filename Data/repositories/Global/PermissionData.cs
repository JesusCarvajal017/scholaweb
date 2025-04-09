using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Data.repositories.Global
{
    public class PermissionData : GenericData<Permission>
    {
        public PermissionData(ApplicationDbContext context, ILogger<Permission> logger) : base(context, logger)
        {

        }
    }
}
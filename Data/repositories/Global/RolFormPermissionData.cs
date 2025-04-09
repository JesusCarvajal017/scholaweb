using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class RolFormPermissionData : GenericData<RolFormPermission>
    {
        private ApplicationDbContext context;
        private ILogger<RolFormPermissionData> _logger;
        public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermission> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<RolFormPermission>> GetAllAsyncLinq()
        {
            try
            {
                return await context.RolFormPermission
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving RolFormPermissions");
                throw;
            }
        }

        public override async Task<RolFormPermission> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await context.RolFormPermission
                    .Include(rfp => rfp.Rol)
                    .Include(rfp => rfp.Form)
                    .Include(rfp => rfp.Permission)
                    .FirstOrDefaultAsync(rfp => rfp.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving RolFormPermissions");
                throw;
            }
        }



    }
}


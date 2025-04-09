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
    public class UserRolData : GenericData<UserRol>
    {
        private ApplicationDbContext context;
        private ILogger<UserRolData> _logger;
        public UserRolData(ApplicationDbContext context, ILogger<UserRol> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<UserRol>> GetAllAsyncLinq()
        {
            try
            {
                return await context.userRol
                    .Include(ur => ur.User)
                    .Include(ur => ur.Rol)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }

        public override async Task<UserRol> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await context.userRol
                    .Include(ur => ur.User)
                    .Include(ur => ur.Rol)
                    .FirstOrDefaultAsync(ur => ur.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving UserRols");
                throw;
            }
        }



    }
}


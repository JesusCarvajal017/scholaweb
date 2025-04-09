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
    public class ModuleFormData : GenericData<ModuleForm>
    {
        private ApplicationDbContext context;
        private ILogger<ModuleFormData> _logger;
        public ModuleFormData(ApplicationDbContext context, ILogger<ModuleForm> logger) : base(context, logger)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<ModuleForm>> GetAllAsyncLinq()
        {
            try
            {
                return await context.ModuleForm
                    .Include(mf => mf.Module)
                    .Include(mf => mf.Form)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ModuleForms");
                throw;
            }
        }

        public override async Task<ModuleForm> GetByIdAsyncLinq(int id)
        {
            try
            {
                return await context.ModuleForm
                    .Include(mf => mf.Module)
                    .Include(mf => mf.Form)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ModuleForms");
                throw;
            }
        }

        

    }
}


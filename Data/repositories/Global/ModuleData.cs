
using System.Collections.Generic;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class ModuleData : GenericData<Module>
    {
        public ModuleData(ApplicationDbContext context, ILogger<Module> logger) : base(context, logger)
        {

        }

    }
}
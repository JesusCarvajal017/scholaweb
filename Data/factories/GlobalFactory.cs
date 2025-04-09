using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.interfaces;
using Data.repositories.Global;
using Entity;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.factories
{
    public class GlobalFactory : IDataFactoryGlobal
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerFactory _loggerFactory;
        public GlobalFactory(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
        }

        public CrudBase<Person> CreatePersonData()
        {
            var logger = _loggerFactory.CreateLogger<Person>();
            return new PersonData(_context, logger);
        }

        public CrudBase<Rol> CreateRolData()
        {
            var logger = _loggerFactory.CreateLogger<Rol>();
            return new RolData(_context, logger);
        }

        public CrudBase<Form> CreateFormData()
        {
            var logger = _loggerFactory.CreateLogger<Form>();
            return new FormData(_context, logger);
        }

        public CrudBase<Module> CreateModuleData()
        {
            var logger = _loggerFactory.CreateLogger<Module>();
            return new ModuleData(_context, logger);
        }

        //public IGlobalLinq<RolFormPermission> CreateRolFormPermissionData()
        //{
        //    var logger = _loggerFactory.CreateLogger<RolFormPermissionData>();
        //    return new RolFormPermissionData(_context, logger);

        //}
    }
}

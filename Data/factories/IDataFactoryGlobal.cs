using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.interfaces;
using Entity.Model;

namespace Data.factories
{
    public interface IDataFactoryGlobal
    {

        CrudBase<Person> CreatePersonData();
        CrudBase<Rol> CreateRolData();
        CrudBase<Form> CreateFormData();

        CrudBase<Module> CreateModuleData();

        //CrudBase<User> CreateUserData();
        //IGlobalLinq<RolFormPermission> CreateRolFormPermissionData();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Entity.DTOs
{
    public class ModuleFormDto
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }

        //public Module module { get; set; }

        public string NameModule { get; set; }
        //public string ModuleName { get; set; }
        public int FormId { get; set; }

        public string NameForm { get; set; }
        //public string FormName { get; set; }
    }
}

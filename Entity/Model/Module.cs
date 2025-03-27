using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Module
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }
    }
}

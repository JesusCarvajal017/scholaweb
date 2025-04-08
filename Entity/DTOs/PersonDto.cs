using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Entity.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string NameComplet { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Identification { get; set; }
        public int Age { get; set; }
        public int Status { get; set; }
    }
}

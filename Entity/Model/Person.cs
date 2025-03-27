using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Identification { get; set; }

        public int Age { get; set; }
        public int Status { get; set; }

    }
}

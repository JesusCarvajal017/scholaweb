using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UserRolQueryDto
    {
        public int idRolUser { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }   

        public int RolId { get; set; }

        public string RolName { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class UserRolDto
    {
        //public int IdRolUser { get; set; }

        public int UserId { get; set; }
        public string NameUser { get; set; }

        public int RolId { get; set; }
        public string RolName { get; set; }

    }
}

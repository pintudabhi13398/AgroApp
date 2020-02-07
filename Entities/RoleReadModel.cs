using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class RoleReadModel 
    {
        [ScaffoldColumn(false)]
        public int RoleId { get; set; }

        [UIHint("TextBox")]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}

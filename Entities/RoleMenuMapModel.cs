using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class RoleMenuMapModel
    {
        public int RoleMenuMapId { get; set; }
        public int RoleId { get; set; }
        public short MenuId { get; set; }
        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsChangeStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class UserAccessPermissionModel
    {
        public short MenuId { get; set; }
        public Nullable<short> ParentMenuId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int DisplayOrder { get; set; }
        public Nullable<bool> IsView { get; set; }
        public Nullable<bool> IsInsert { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsChangeStatus { get; set; }
    }
}

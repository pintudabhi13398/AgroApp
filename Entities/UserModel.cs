using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        
        [Required (ErrorMessage ="Full Name Required")]
        [StringLength(50, ErrorMessage ="Maximum Length is 50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User Name Required")]
        [StringLength(50, MinimumLength =5,ErrorMessage = "Maximum Length is 50 And Minimum length is 5")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Maximum Length is 50 And Minimum length is 6")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
       
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "EmailAddress Required")]
        [StringLength(50, ErrorMessage = "Maximum Length is 50 ")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", ErrorMessage = "Invalid Email")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public int RoleId { get; set; }

       
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Phone no. Required")]
        [StringLength(10, ErrorMessage = "Maximum Length is 10 ")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Mobile number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address Required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Maximum Length is 100 And Minimum length is 6")]
        public string Address { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Dist is Required")]
        public int DistId { get; set; }

        [Required(ErrorMessage = "ZipCode Required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Maximum Length is 6 And Minimum length is 6")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Number Allow")]
        public string ZipCode { get; set; }

        public string Image { get; set; }
        public bool IsActive { get; set; }
        [ScaffoldColumn(false)]
        public string Token { get; set; }
        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> TokenExpiryDate { get; set; }
        [ScaffoldColumn(false)]
        public bool IsSuperAdmin { get; set; }
    }
}

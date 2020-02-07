using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm Password")]
        //[Required(ErrorMessage = "Confirm password is required")]
        //[Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}

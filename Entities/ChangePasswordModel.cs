using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class ChangePasswordModel
    {

        public string RealPassword { get; set; }

        [Display(Name = "OldPassword")]
        [Required(ErrorMessage = "OldPassword is Required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "NewPassword is Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare("NewPassword", ErrorMessage = "NewPassword and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public int CurrentUserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Channels;
using System.Text;

namespace Entities
{
    public class StateMasterModel
    {
        public class State
        {
            public int StateId { get; set; }
          
            [Display(Name = "State Name")]
            [Required(ErrorMessage = "FullNameRequired")]
            [StringLength(50, ErrorMessage =  "FullNameLength")]
            public string StateName { get; set; }
            public bool IsActive { get; set; }
        }
        public class District
        {
            public int DistId { get; set; }

            [Required(ErrorMessage = "FullNameRequired")]
            [StringLength(50, ErrorMessage =  "FullNameLength")]
            [Display(Name ="District Name")]
            public string DistName { get; set; }

            [Required(ErrorMessage ="Satate Required")]
            [Display(Name = "District Name")]
            public int StateId { get; set; }
            public string StateName { get; set; }
            public bool DistIsActive { get; set; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTest.DAL.Models
{
    public class Department : ModelBase
    {

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; } // .Net 5.0 Allow Null
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
    }
}

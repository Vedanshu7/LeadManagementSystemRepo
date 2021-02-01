using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Description = "Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Password must contain Upper Case, Lower Case, Number and a Special Character")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The fields Password and Confirm Password should be equal")]
        [Display(Description = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
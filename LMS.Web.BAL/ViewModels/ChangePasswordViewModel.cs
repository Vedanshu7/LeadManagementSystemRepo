using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Description = "Current Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Password must contain Upper Case, Lower Case, Number and a Special Character")]
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Description = "New Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Password must contain Upper Case, Lower Case, Number and a Special Character")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The fields Password and Confirm Password should be equal")]
        [Display(Description = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}

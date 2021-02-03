using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Common;

namespace LMS.Web.BAL.Interface
{
    public interface ILoginManager
    {
        LoginResult Login(LoginViewModel login);
        string ResetPassword(string userEmail, ResetPasswordViewModel resetPassword);
        bool IsValidUser(string userEmail);
    }
}

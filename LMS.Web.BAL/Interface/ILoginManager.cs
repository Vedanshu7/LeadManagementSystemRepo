using LMS.Web.BAL.ViewModels;
using LMS.Common;

namespace LMS.Web.BAL.Interface
{
    public interface ILoginManager
    {
        LoginResult Login(LoginViewModel login);
        string ResetPassword(string userEmail, ResetPasswordViewModel resetPassword);
        bool IsValidUser(string userEmail);
        string ChangePassword(ChangePasswordViewModel changePassword,int loggedInUserId);
    }
}

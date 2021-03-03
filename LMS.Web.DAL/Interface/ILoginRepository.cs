using LMS.Common;

namespace LMS.Web.DAL.Interface
{
    public interface ILoginRepository
    {
        LoginResult Login(string email, string password);
        string ResetPassword(string email, string password);
        bool IsValidUser(string userEmail);
        string ChangePassword(string currentPassword, string newPassword,int loggedInUserId);
    }
}

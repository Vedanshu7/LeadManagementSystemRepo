using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Manager
{
    public class LoginManager : ILogin
    {
        private readonly ILoginRepository _loginRepository;
        public LoginManager(ILoginRepository loginReposiotry)
        {
            _loginRepository = loginReposiotry;
        }
        public int Login(LoginViewModel login)
        {
            return _loginRepository.Login(login.Email, login.Password, login.Role);
        }

        public string ResetPassword(string userEmail, ResetPasswordViewModel resetPassword)
        {
            return _loginRepository.ResetPassword(userEmail, resetPassword.Password);
        }

        public bool IsValidUser(string userEmail)
        {
            return _loginRepository.IsValidUser(userEmail);
        }
    }
}

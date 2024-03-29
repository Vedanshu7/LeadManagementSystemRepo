﻿using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Interface;
using LMS.Common;

namespace LMS.Web.BAL.Manager
{
    public class LoginManager : ILoginManager
    {
        private readonly ILoginRepository _loginRepository;
        public LoginManager(ILoginRepository loginReposiotry)
        {
            _loginRepository = loginReposiotry;
        }
        public LoginResult Login(LoginViewModel login)
        {
            return _loginRepository.Login(login.Email, login.Password);
        }
        public string ResetPassword(string userEmail, ResetPasswordViewModel resetPassword)
        {
            return _loginRepository.ResetPassword(userEmail, resetPassword.Password);
        }
        public bool IsValidUser(string userEmail)
        {
            return _loginRepository.IsValidUser(userEmail);
        }

        public string ChangePassword(ChangePasswordViewModel changePassword,int loggedInUserId)
        {
            return _loginRepository.ChangePassword(changePassword.CurrentPassword, changePassword.NewPassword,loggedInUserId);

        }
    }
}

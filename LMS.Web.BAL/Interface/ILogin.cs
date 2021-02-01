﻿using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Interface
{
    public interface ILogin
    {
        int Login(LoginViewModel login);

        int ResetPassword(string userEmail, ResetPasswordViewModel resetPassword);
    }
}
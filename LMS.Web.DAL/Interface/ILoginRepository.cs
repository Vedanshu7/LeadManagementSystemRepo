using LMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Interface
{
    public interface ILoginRepository
    {
        LoginResult Login(string email, string password);

        string ResetPassword(string email, string password);

        bool IsValidUser(string userEmail);
    }
}

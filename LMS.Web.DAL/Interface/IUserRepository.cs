using LMS.Web.DAL.Database;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Interface
{
    public interface IUserRepository
    {
        bool CreateUser(string name,string email,string password,string mobileNumber);
        List<Users> UserDetails();
    }
}

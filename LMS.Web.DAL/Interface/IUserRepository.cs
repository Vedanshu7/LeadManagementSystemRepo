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
        bool CreateUser(Users users);
        List<Users> UserDetails(int dealerId);
        bool EditUser(Users user);
        Users GetUser(int Id);
        int GetDealerId(int loggedInUserId);
        List<Users> GetUsers(int leadId);

    }
}

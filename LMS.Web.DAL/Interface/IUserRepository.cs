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
        string CreateUser(Users users);
        List<Users> UserDetails(int dealerId);
        string EditUser(Users user);
        Users GetUser(int dealerId, int Id);
        int GetDealerId(int loggedInUserId);
        List<Users> GetUsersByLeadType(int leadId);
        IEnumerable<Roles> GetUserRoleDropDown();

    }
}

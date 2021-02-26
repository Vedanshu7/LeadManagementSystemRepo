using LMS.Web.DAL.Database;
using LMS.Web.DAL.Models;
using System.Collections.Generic;

namespace LMS.Web.DAL.Interface
{
    public interface IUserRepository
    {
        string CreateUser(Users users);
        List<Users> GetUsers(int dealerId);
        List<UserLeadCountModel> GetUsersLeadCount(int loggedInUserId);
        string EditUser(Users user);
        Users GetUser(int dealerId, int Id);
        List<Users> GetUsersByLeadType(int leadId);
        IEnumerable<Roles> GetUserRoleDropDown();
    }
}

using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;

namespace LMS.Web.BAL.Interface
{
    public interface IUserManager
    {
        string CreateUser(UserViewModel users, int dealerId);
        string EditUser(UserViewModel users, int dealerId);
        List<UserLeadCountViewModel> GetUsersLeadCount(int loggedInUserId);
        UserViewModel GetUser(int dealerId, int Id);
        List<UserViewModel> GetUsersByLeadType(int leadId);
        List<UserViewModel> GetUsers(int dealerId);
        IEnumerable<UserRoleViewModel> GetUserRoleDropDown();
    }
}

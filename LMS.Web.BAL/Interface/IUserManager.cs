using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Interface
{
    public interface IUserManager
    {
        string CreateUser(UserViewModel users, int dealerId);
        string EditUser(UserViewModel users, int dealerId);
        UserViewModel GetUser(int dealerId, int Id);
        int GetDealerId(int loggedInUserId);
        List<UserViewModel> GetUsersByLeadType(int leadId);
        List<UserViewModel> GetUsers(int dealerId);
        IEnumerable<UserRoleViewModel> GetUserRoleDropDown();
    }
}

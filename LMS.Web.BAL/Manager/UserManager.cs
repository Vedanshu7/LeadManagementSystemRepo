using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserRepository userReposiotry)
        {
            _userRepository = userReposiotry;
        }
        public bool CreateUser(UserViewModel users)
        {
            return _userRepository.CreateUser(users.Name,users.Email,users.Password,users.MobileNumber);
        }

        public List<Users> UserDetail()
        {
            return _userRepository.UserDetails();
        }
    }
}

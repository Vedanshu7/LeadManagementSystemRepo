using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        public IMapper mapper;
        public MapperConfiguration config;

        public UserManager(IUserRepository userReposiotry)
        {
            _userRepository = userReposiotry;
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserViewModel, Users>();
                cfg.CreateMap<Users, UserViewModel>();
                cfg.CreateMap<UserRoleViewModel, Roles>();
                cfg.CreateMap<Roles, UserRoleViewModel>();
            });

            mapper = config.CreateMapper();
        }

        public string CreateUser(UserViewModel users, int dealerId)
        {
            Users user = mapper.Map<UserViewModel, Users>(users);
            user.DealerId = dealerId;
            return _userRepository.CreateUser(user);
        }
        public string EditUser(UserViewModel users, int dealerId)
        {
            Users user = mapper.Map<UserViewModel, Users>(users);
            user.DealerId = dealerId;
            return _userRepository.EditUser(user);
        }
        public UserViewModel GetUser(int dealerId, int Id)
        {
            Users users = _userRepository.GetUser(dealerId, Id);
            if (users != null)
            {
                UserViewModel user = mapper.Map<Users, UserViewModel>(users);
                user.Role = users.Roles.Name;
                return user;
            }
            else
            {
                return null;
            }

        }
        public List<UserViewModel> GetUsers(int dealerId)
        {
            List<Users> usersFromDb = _userRepository.GetUsers(dealerId);
            List<UserViewModel> users = mapper.Map<List<Users>, List<UserViewModel>>(usersFromDb);

            //Populating Role Name Field
            for (int i = 0; i < users.Count; i++)
            {
                users[i].Role = usersFromDb[i].Roles.Name;
            }
            return users;
        }
        public List<UserViewModel> GetUsersByLeadType(int leadId)
        {
            var usersFromDb = _userRepository.GetUsersByLeadType(leadId);
            List<UserViewModel> users = mapper.Map<List<Users>, List<UserViewModel>>(usersFromDb);

            //Populating Role Name Field
            for (int i = 0; i < users.Count; i++)
            {
                users[i].Role = usersFromDb[i].Roles.Name;
            }

            return users;
        }
        public IEnumerable<UserRoleViewModel> GetUserRoleDropDown()
        {
            var userRoleFromDb = _userRepository.GetUserRoleDropDown();
            IEnumerable<UserRoleViewModel> roles = mapper.Map<IEnumerable<Roles>, IEnumerable<UserRoleViewModel>>(userRoleFromDb);
            return roles;
        }
    }
}

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
            });

            mapper = config.CreateMapper();
        }
        public bool CreateUser(UserViewModel users, int dealerId)
        {
            Users user = mapper.Map<UserViewModel, Users>(users);
            user.DealerId = dealerId;
            return _userRepository.CreateUser(user);
        }

        public bool EditUser(UserViewModel users, int dealerId)
        {
            Users user = mapper.Map<UserViewModel, Users>(users);
            user.DealerId = dealerId;
            return _userRepository.EditUser(user);
        }

        public UserViewModel GetUser(int Id)
        {
            Users users = _userRepository.GetUser(Id);
            if (users != null)
            {
                UserViewModel user = mapper.Map<Users, UserViewModel>(users);
                return user;
            }
            else
            {
                return null;
            }

        }

        public List<UserViewModel> UserDetails(int dealerId)
        {
            List<Users> usersFromDb = _userRepository.UserDetails(dealerId);
            List<UserViewModel> users = mapper.Map<List<Users>, List<UserViewModel>>(usersFromDb);
            return users;
        }

        public int GetDealerId(int loggedInUserId)
        {
            return _userRepository.GetDealerId(loggedInUserId);
        }

        public List<UserViewModel> GetUsers(int leadId)
        {
            var usersFromDb = _userRepository.GetUsers(leadId);
            List<UserViewModel> users = mapper.Map<List<Users>, List<UserViewModel>>(usersFromDb);
            return users;
        }
    }
}

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
                cfg.CreateMap<List<Users>,List<UserViewModel>>();
            });

            mapper = config.CreateMapper();
        }
        public bool CreateUser(UserViewModel users)
        {
            Users user = mapper.Map<UserViewModel, Users>(users);
            return _userRepository.CreateUser(user);
        }

        public bool EditUser(UserViewModel users)
        {
            Users user=mapper.Map<UserViewModel,Users>(users);
            return _userRepository.EditUser(user);
        }

        public List<UserViewModel> UserDetail()
        {
            List<UserViewModel> users= mapper.Map<List<Users>, List<UserViewModel>>(_userRepository.UserDetails());
            return users;
        }
    }
}

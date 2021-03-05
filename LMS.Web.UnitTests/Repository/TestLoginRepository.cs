using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Common;
using LMS.Common.Enums;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;

namespace LMS.Web.UnitTests.Repository
{
    class TestLoginRepository : ILoginRepository
    {
        private List<Users> _db = new List<Users>
        {
            new Users{ Id = 1, Name = "test user", Password = "Lms@2021", Email = "test@test.com", DealerId = 1, CreatedBy = 1, CreatedDate = DateTime.Now, RoleId = 1, MobileNumber = "111222", IsActive = true}
        };

        public LoginResult Login(string email, string password)
        {
            try
            {
                var user = _db.Where(u => u.Email == email).FirstOrDefault();
                var loginResult = new LoginResult();
                if (user == null)
                {
                    loginResult.result = LoginResultEnum.NotFound;
                    return loginResult; //Not found
                }

                if (user.Password != password)
                {
                    loginResult.result = LoginResultEnum.Invalid;
                    return loginResult; //Invalid Username or Password
                }

                //Set User Role and Success result
                loginResult.result = LoginResultEnum.Success;
                loginResult.role = (RolesEnum)user.RoleId;
                return loginResult; //Success
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string ResetPassword(string email, string password) 
        {
            try
            {
                var user = _db.Where(u => u.Email == email).FirstOrDefault();

                if (user == null)
                {
                    return "Not Found";
                }

                user.Password = password; //Update password

                return "Success"; //Success

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsValidUser(string userEmail)
        {

            if (_db.Where(u => u.Email == userEmail).Any())
                return true; //Success

            //If no user is found
            return false;
        }

        public string ChangePassword(string currentPassword, string newPassword, int loggedInUserId)
        {
            throw new NotImplementedException();
        }
    }
}

using LMS.Common;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private static Database.LMSEntitiesAzure _db;

        public LoginRepository()
        {
            _db = new Database.LMSEntitiesAzure();
        }

        public LoginResult Login(string email, string password)
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == email).FirstOrDefault();
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

        public string ResetPassword(string email, string password) //TODO: Change Return type to LoginResultEnum
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == email).FirstOrDefault();

                if (user == null)
                {
                    return "Not Found";
                }

                user.Password = password; //Update password

                _db.SaveChanges();

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

            if (_db.Users.Where(u => u.Email == userEmail).Any())
                return true; //Success

            //If no user is found
            return false;
        }
    }
}

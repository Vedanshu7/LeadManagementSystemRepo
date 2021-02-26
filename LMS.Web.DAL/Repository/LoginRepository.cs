using LMS.Common;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Linq;

namespace LMS.Web.DAL.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly LMSAzureEntities _db;

        public LoginRepository()
        {
            _db = new LMSAzureEntities();
        }

        public LoginResult Login(string email, string password)
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == email && u.IsActive == true).FirstOrDefault();
                var loginResult = new LoginResult();
                if (user == null) //If user not found
                {
                    loginResult.result = LoginResultEnum.NotFound;
                    return loginResult; //Not found
                }

                if (user.Password != password) //If password doesn't match
                {
                    loginResult.result = LoginResultEnum.Invalid;
                    return loginResult; //Invalid Username or Password
                }

                //Set UserId, User Role and Success result
                loginResult.LoggedInUserId = user.Id;
                loginResult.result = LoginResultEnum.Success;
                loginResult.role = (RolesEnum)user.RoleId;

                if (loginResult.role == RolesEnum.DealerManager) //Set DealerId in LoginResult if it's a Dealer
                    loginResult.DealerId = (int)user.DealerId;

                return loginResult; //Success
            }
            catch (Exception e)
            {
                Console.WriteLine(e); //TODO: Log Errors in File (Inner message, Stacktrace)
                throw;
            }
        }
        public string ResetPassword(string email, string password) //TODO: Change Return type to LoginResultEnum
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == email && u.IsActive == true).FirstOrDefault();

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

            if (_db.Users.Where(u => u.Email == userEmail && u.IsActive == true).Any())
                return true; //Success

            //If no user is found
            return false;
        }
    }
}

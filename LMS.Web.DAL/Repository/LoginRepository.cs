using LMS.Common;
using LMS.Common.Enums;
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using log4net;
using System;
using System.Data.Entity;
using System.Linq;

namespace LMS.Web.DAL.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly LMSAzureEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginRepository));

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
                loginResult.UserName = user.Name;

                if (loginResult.role == RolesEnum.DealerManager) //Set DealerId in LoginResult if it's a Dealer
                    loginResult.DealerId = (int)user.DealerId;

                return loginResult; //Success
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return new LoginResult() { result = LoginResultEnum.Error };
            }
        }
        public string ResetPassword(string email, string password)
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
                Log.Error(e.Message, e);
                throw;
            }
        }
        public bool IsValidUser(string userEmail)
        {
            try
            {
                if (_db.Users.Where(u => u.Email == userEmail && u.IsActive == true).Any())
                    return true; //Success

                //If no user is found
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return false;
            }
        }
        public string ChangePassword(string currentPassword, string newPassword,int loggedInUserId)
        {
            try
            {
                var userFromDb = _db.Users.Where(u => u.Id == loggedInUserId).FirstOrDefault();
                if (userFromDb != null)
                {
                    if (userFromDb.Password == currentPassword)
                    {
                        userFromDb.Password = newPassword;
                        _db.Entry(userFromDb).State = EntityState.Modified;
                        _db.SaveChanges();
                        return "Success";
                    }
                    else
                    {
                        return "Enter Current Password";
                    }
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occurred";
            }
            
        }
    }
}

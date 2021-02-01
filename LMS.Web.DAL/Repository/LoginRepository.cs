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
        private static Database.LMSEntities _db;

        public LoginRepository()
        {
            _db = new Database.LMSEntities();
        }

        public int Login(string email, string password, int role)
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == email).First();

                if (user.Password != password)
                {
                    return 2; //Invalid Username or Password
                }
                //TODO: Validate User Role
                return 1; //Success
            }
            catch (Exception e) //If no such user is found
            {
                return 3; //No user found
            }
        }

        public string ResetPassword(string email, string password)
        {
            try
            {
                var users = _db.Users.Where(u => u.Email == email).First();

                users.Password = password; //Update password

                _db.SaveChanges();

                return "Success"; //Success

            }
            catch (Exception e) //If no user is found
            {
                Console.WriteLine(e);
                return "Not Found";
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

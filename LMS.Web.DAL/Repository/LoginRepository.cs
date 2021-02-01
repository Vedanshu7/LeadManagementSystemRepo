using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Web.DAL.Enums;

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
                    return (int)LoginResult.Invalid; //Invalid Username or Password
                }
                //TODO: Validate User Role
                return (int)LoginResult.Success; //Success
            }
            catch (Exception e) //If no such user is found
            {
                return (int)LoginResult.NotFound; //No user found
            }
        }

        public int ResetPassword(string email, string password)
        {
            try
            {
                var users = _db.Users.Where(u => u.Email == email).First();

                users.Password = password; //Update password

                _db.SaveChanges();

                return 1; //Success

            }
            catch (Exception e) //If no user is found
            {
                Console.WriteLine(e);
                return 0;
            }
        }
    }
}

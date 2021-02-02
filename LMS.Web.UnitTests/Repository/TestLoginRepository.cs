using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int Login(string email, string password, int role)
        {
            try
            {
                var user = _db.Where(u => u.Email == email).First();

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
                var users = _db.Where(u => u.Email == email).First();

                users.Password = password; //Update password

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

            if (_db.Where(u => u.Email == userEmail).Any())
                return true; //Success

            //If no user is found
            return false;
        }
    }
}

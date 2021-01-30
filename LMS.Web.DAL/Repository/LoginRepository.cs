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
            var users = _db.Users.Where(u => u.Email == email).ToList();
            if (users != null)
            {
                if (users[0].Password != password)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 3;
            }

        }

        public int RestPassword(string Email, string Password)
        {
            var users = _db.Users.Where(u => u.Email == Email).First();
            if (users != null)
            {
                users.Password = Password;

                _db.SaveChanges();

                return 1;
            }
            else
            {
                return 0;
            }
          

        }
    }
}

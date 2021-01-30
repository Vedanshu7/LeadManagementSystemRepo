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
            _db = new  Database.LMSEntitiesAzure();
        }
        public int Login(string Email, string Password, int Role)
        {
            var Users =_db.Users.Find(Email);
            if (Users != null)
            {
                if (Users.Password != Password)
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
    }
}

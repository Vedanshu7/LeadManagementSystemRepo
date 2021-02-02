
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Database.LMSEntitiesAzure _db;
        public UserRepository()
        {
            _db = new Database.LMSEntitiesAzure();
        }
        public bool CreateUser(Users users)
        {

          
            var emailId = _db.Users.Where(m => m.Email == users.Email).Any();
            if (emailId != true)
            {
                users.CreatedBy = 1;
                users.CreatedDate = DateTime.UtcNow;
                users.IsActive = true;
                users.DealerId = 1;

                _db.Users.Add(users);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }



        }

        public bool EditUser(Users user)
        {
            var emailId = _db.Users.Where(m => m.Email == user.Email).Any();
            if (emailId != true)
            {
                user.UpdatedBy = 1;
                user.UpdatedDate = DateTime.UtcNow;
                
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Users> UserDetails()
        {
           
            List<Users> list = _db.Users.ToList();

            return list;
        }
    }
}

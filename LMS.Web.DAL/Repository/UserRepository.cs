
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Database.LMSEntitiesAzure _db;
        //TODO: add mapper
        public UserRepository()
        {
            _db = new Database.LMSEntitiesAzure();
        }
        public bool CreateUser(Users user)
        {
            //Check if user already exists or not
            var emailId = _db.Users.Where(m => m.Email == user.Email).Any();
            if (emailId != true)
            {
                user.CreatedBy = user.DealerId;
                user.CreatedDate = DateTime.UtcNow;
                user.IsActive = true;

                _db.Users.Add(user);
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
            //var emailId = _db.Users.Where(m => m.Email == user.Email).Any();
            var userFromDb = _db.Users.Where(u => u.Id == user.Id).FirstOrDefault();

            if (userFromDb != null)
            {
                userFromDb.Name = user.Name;
                userFromDb.MobileNumber = user.MobileNumber;
                userFromDb.Email = user.Email;
                userFromDb.RoleId = user.RoleId;
                userFromDb.UpdatedBy = user.DealerId;
                userFromDb.UpdatedDate = DateTime.UtcNow;

                _db.Entry(userFromDb).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public Users GetUser(int Id)
        {
            if (Id > 0)
            {
                Users users = _db.Users.Find(Id);
                return users;
            }
            else
            {
                return null;
            }
        }

        public List<Users> UserDetails()
        {
           
            List<Users> list = _db.Users.ToList();

            return list;
        }
    }
}

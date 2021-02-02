
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
        private readonly Database.LMSEntitiesAzure db;
        public UserRepository()
        {
            db = new Database.LMSEntitiesAzure();
        }
        public bool CreateUser(string name, string email, string password, string mobileNumber)
        {

            Database.Users users = new Database.Users();
            var emailId = db.Users.Where(m => m.Email == email).Any();
            if (emailId != true)
            {
                users.Name = name;
                users.Email = email;

                users.DealerId = 1;
                users.Password = password;
                users.MobileNumber = mobileNumber;
                users.RoleId = 2;
                users.CreatedDate = DateTime.Now;
                users.CreatedBy = 1;
                users.IsActive = true;

                db.Users.Add(users);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }



        }

        public List<Users> UserDetails()
        {
            var entities = db.Users.ToList();
            List<Users> list = new List<Users>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    Users users = new Users();
                    users.Name = item.Name;
                    users.Email = item.Email;
                    users.DealerId = item.DealerId;
                    users.MobileNumber = item.MobileNumber;
                    users.RoleId = item.RoleId;
                    users.CreatedDate = item.CreatedDate;
                    users.CreatedBy = item.CreatedBy;
                    users.IsActive = item.IsActive;

                    list.Add(users);
                }
            }
            return list;
        }
    }
}

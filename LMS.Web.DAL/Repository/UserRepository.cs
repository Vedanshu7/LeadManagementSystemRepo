
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
        private readonly LMSAzureEntities _db;
        public UserRepository()
        {
            _db = new LMSAzureEntities();
        }
        public bool CreateUser(Users user)
        {
            //Check if user already exists or not
            var emailId = _db.Users.Any(m => m.Email == user.Email);
            if (!emailId)
            {
                user.CreatedBy = (int)user.DealerId;
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
            var userFromDb = _db.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            if (userFromDb != null)
            {
                userFromDb.Name = user.Name;
                userFromDb.MobileNumber = user.MobileNumber;
                userFromDb.Password = user.Password;
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

        public Users GetUser(int dealerId, int Id)
        {
            Users user = _db.Users.Where(u => u.Id == Id && u.DealerId == dealerId).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public List<Users> UserDetails(int dealerId)
        {
            List<Users> list = _db.Users.Where(u => u.DealerId == dealerId && u.RoleId != 4).ToList();
            return list;
        }

        public int GetDealerId(int loggedInUserId)
        {
            var dealerInDb = _db.Users.Find(loggedInUserId);
            return (int)dealerInDb.DealerId;
        }

        public List<Users> GetUsers(int leadId) //Returns Users concerned with Lead Type (of provided leadId)
        {
            var lead = _db.Leads.Find(leadId); //fetching the lead based on leadId
            var users = _db.Users //fetching users according to the lead type and dealerId
                .Where(u =>
                u.RoleId == lead.LeadTypeId &&
                u.DealerId == lead.DealerId)
                .ToList();

            return users;
        }
    }
}

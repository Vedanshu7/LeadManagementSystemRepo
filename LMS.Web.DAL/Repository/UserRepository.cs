﻿
using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using Constants = LMS.Common.Constants;
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

        //Dealer Methods
        public string CreateUser(Users user)
        {
            try
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
                    return "Success";
                }
                else
                {
                    return "Error occured.";
                }
            }
            catch (Exception)
            {
                //TODO:Add Logger.
                return "Error occured.";
                throw;
            }
        }
        public string EditUser(Users user)
        {
            try
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
                    return "Success";
                }
                return "Error occured";
            }
            catch (Exception e)
            {
                //TODO:Add logger.
                return "Error occured";
                throw;
            }
        }
        public Users GetUser(int dealerId, int Id)
        {
            try
            {
                Users user = _db.Users.Where(u => u.Id == Id && u.DealerId == dealerId).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception)
            {
                //TODO:Add logger.
                return null;
                throw;
            }
        }
        public List<Users> GetUsers(int dealerId)
        {
            List<Users> list = _db.Users.Where(u => u.DealerId == dealerId && u.Roles.RoleCode != Constants.Roles.Dealer).ToList();
            return list;
        }
        public int GetDealerId(int loggedInUserId)
        {
            try
            {
                var dealerInDb = _db.Users.Find(loggedInUserId);
                return (int)dealerInDb.DealerId;
            }
            catch (Exception e)
            {
                //TODO:Add logger.
                return 0;
                throw;
            }

        }
        public List<Users> GetUsersByLeadType(int leadId)
        {
            var lead = _db.Leads.Find(leadId);

            //If Leads are Sales Leads, return Sales Users
            if (lead.LeadType.LeadTypeCode == Constants.LeadType.Sales)
            {
                var users = _db.Users //fetching users according to the lead type and dealerId
                .Where(u =>
                    u.Roles.RoleCode == Constants.Roles.Sales &&
                    u.DealerId == lead.DealerId)
                    .ToList();
                return users;
            }
            else //Return AfterSales Users
            {
                var users = _db.Users //fetching users according to the lead type and dealerId
                .Where(u =>
                u.Roles.RoleCode == Constants.Roles.AfterSales &&
                u.DealerId == lead.DealerId)
                .ToList();
                return users;
            }
        }

        public IEnumerable<Roles> GetUserRoleDropDown()
        {
            return _db.Roles;
        }
    }
}

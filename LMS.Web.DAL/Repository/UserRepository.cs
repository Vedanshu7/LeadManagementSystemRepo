using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using Constants = LMS.Common.Constants;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LMS.Web.DAL.Models;
using log4net;

namespace LMS.Web.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LMSAzureEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserRepository));
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
                var emailId = _db.Users.Any(m => m.Email == user.Email && m.IsActive == true);
                var rolecode = _db.Roles.FirstOrDefault(r => r.Id == user.RoleId).RoleCode;
                if (rolecode.Equals(Common.Constants.Roles.Sales) ||
                    rolecode.Equals(Common.Constants.Roles.AfterSales))
                {
                    if (!emailId)
                    {
                        user.CreatedBy = (int)user.DealerId;
                        user.CreatedDate = DateTime.Now;
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
                else
                {
                    return "Error occured.";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occured.";
            }
        }
        public string EditUser(Users user)
        {
            try
            {
                var userFromDb = _db.Users.Where(u => u.Id == user.Id && u.IsActive == true).FirstOrDefault();
                var rolecode = _db.Roles.FirstOrDefault(r => r.Id == user.RoleId).RoleCode;
                if (rolecode.Equals(Common.Constants.Roles.Sales) ||
                    rolecode.Equals(Common.Constants.Roles.AfterSales))
                {
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
                else
                {
                    return "Error occured.";
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occured";
            }
        }
        public Users GetUser(int dealerId, int Id)
        {
            try
            {
                Users user = _db.Users.Where(u => u.Id == Id && u.DealerId == dealerId && u.Roles.RoleCode != Constants.Roles.Dealer && u.IsActive == true).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
        public List<Users> GetUsers(int dealerId)
        {
            try
            {
                List<Users> list = _db.Users.Where(u => u.DealerId == dealerId && u.Roles.RoleCode != Constants.Roles.Dealer && u.IsActive == true).ToList();
                return list;
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
        public List<Users> GetUsersByLeadType(int leadId)
        {
            try
            {
                var lead = _db.Leads.Find(leadId);

                //If Leads are Sales Leads, return Sales Users
                if (lead.LeadType.LeadTypeCode == Constants.LeadType.Sales)
                {
                    var users = _db.Users //fetching users according to the lead type and dealerId
                    .Where(u =>
                        u.Roles.RoleCode == Constants.Roles.Sales &&
                        u.DealerId == lead.DealerId &&
                        u.IsActive == true)
                        .ToList();
                    return users;
                }
                else //Return AfterSales Users
                {
                    var users = _db.Users //fetching users according to the lead type and dealerId
                    .Where(u =>
                    u.Roles.RoleCode == Constants.Roles.AfterSales &&
                    u.DealerId == lead.DealerId &&
                    u.IsActive == true)
                    .ToList();
                    return users;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }

        public IEnumerable<Roles> GetUserRoleDropDown()
        {
            try
            {
                return _db.Roles.Where(r=>r.RoleCode.Equals(Common.Constants.Roles.Sales) 
                                       || r.RoleCode.Equals(Common.Constants.Roles.AfterSales)).ToList();
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }

        public List<UserLeadCountModel> GetUsersLeadCount(int loggedInUserId)
        {
            try
            {
                var loggedInUser = _db.Users.Where(u => u.Id == loggedInUserId).First();
                var Leads = _db.Leads.Where(l => l.DealerId == loggedInUser.DealerId).ToList();
                var Users = _db.Users.Where(l => l.DealerId == loggedInUser.DealerId
                && (l.Roles.RoleCode == Constants.Roles.Sales || l.Roles.RoleCode == Constants.Roles.AfterSales)).ToList();
                var detail = Leads.GroupBy(l => l.AssignedUserId).Select(
                      userleads => new
                      {
                          UserId = userleads.Key,
                          Name = userleads.Key == null ? "null" : userleads.Where(l => l.AssignedUserId == userleads.Key).FirstOrDefault().Users.Name,
                          CloseCount = userleads.Where(
                          l => l.AssignedUserId == userleads.Key && l.LeadStatus.LeadStatusCode == Common.Constants.LeadStatus.AfterSalesLost
                          || l.LeadStatus.LeadStatusCode == Common.Constants.LeadStatus.AfterSalesSuccess
                          || l.LeadStatus.LeadStatusCode == Common.Constants.LeadStatus.SalesSuccess
                          || l.LeadStatus.LeadStatusCode == Common.Constants.LeadStatus.SalesSalesLost).ToList().Count(),
                          ActiveCount = userleads.Where(
                          l => l.AssignedUserId == userleads.Key && l.LeadStatus.LeadStatusCode != Common.Constants.LeadStatus.AfterSalesLost
                          && l.LeadStatus.LeadStatusCode != Common.Constants.LeadStatus.AfterSalesSuccess
                          && l.LeadStatus.LeadStatusCode != Common.Constants.LeadStatus.SalesSuccess
                          && l.LeadStatus.LeadStatusCode != Common.Constants.LeadStatus.SalesSalesLost).ToList().Count()
                      }).OrderBy(l => l.UserId).ToList();
                List<UserLeadCountModel> leadCountModel = new List<UserLeadCountModel>();
                List<int> UserId = new List<int>();
                foreach (var item in detail)
                {
                    if (item.UserId != null)
                    {
                        UserLeadCountModel temp = new UserLeadCountModel();
                        temp.Id = (int)item.UserId;
                        temp.Name = item.Name;
                        temp.AciveCounts = item.ActiveCount;
                        temp.ClosedCounts = item.CloseCount;
                        leadCountModel.Add(temp);
                        UserId.Add(temp.Id);
                    }

                }
                var NotAssignedUsers = Users.Where(u => !UserId.Contains(u.Id)).Select(
                    InvalidUsers => new
                    {
                        UserId = InvalidUsers.Id,
                        Name = InvalidUsers.Name,
                        ActiveCount = 0,
                        CloseCount = 0
                    }
                    ).OrderBy(l => l.UserId).ToList();
                foreach (var item in NotAssignedUsers)
                {
                    UserLeadCountModel temp = new UserLeadCountModel();
                    temp.Id = (int)item.UserId;
                    temp.Name = item.Name;
                    temp.AciveCounts = item.ActiveCount;
                    temp.ClosedCounts = item.CloseCount;
                    leadCountModel.Add(temp);
                }
                return leadCountModel;
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
    }
}

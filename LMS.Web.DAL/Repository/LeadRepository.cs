﻿using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using Constants = LMS.Common.Constants;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using log4net;

namespace LMS.Web.DAL.Repository
{
    public class LeadRepository : ILeadRepository
    {
        private readonly LMSAzureEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(LeadRepository));
        public LeadRepository()
        {
            _db = new LMSAzureEntities();
        }

        //Common
        public List<Leads> GetLeadList(DateTime? startDate, DateTime? endDate, int? leadStatusId, int? leadTypeId, int loggedInUserId)
        {
            try
            {
                var loggedInUser = _db.Users.Where(u => u.Id == loggedInUserId && u.IsActive == true).First();

                switch (loggedInUser.Roles.RoleCode)
                {
                    case Constants.Roles.Dealer:
                        var dealerLeads = _db.Leads.Where(l =>
                        l.DealerId == loggedInUser.DealerId &&
                        (leadTypeId == null ? true : l.LeadTypeId == leadTypeId) &&
                        (leadStatusId == null ? true : l.LeadStatusId == leadStatusId) &&
                        (startDate == null ? true : (l.CreatedDate >= startDate && l.CreatedDate <= endDate)))
                        .ToList();
                        return dealerLeads;

                    case Constants.Roles.Sales:
                        var salesLeads = _db.Leads.Where(l =>
                           l.DealerId == loggedInUser.DealerId &&
                           l.LeadType.LeadTypeCode == Constants.LeadType.Sales && //Only return Sales Leads
                           (l.AssignedUserId == null || l.AssignedUserId == loggedInUserId) && //It has to be unassigned or assigned to self
                           (leadTypeId == null ? true : l.LeadTypeId == leadTypeId) &&
                           (leadStatusId == null ? true : l.LeadStatusId == leadStatusId) &&
                           (startDate == null ? true : (l.CreatedDate >= startDate && l.CreatedDate <= endDate)))
                           .ToList();
                        return salesLeads;

                    case Constants.Roles.AfterSales:
                        var afterSalesLeads = _db.Leads.Where(l =>
                           l.DealerId == loggedInUser.DealerId &&
                           l.LeadType.LeadTypeCode == Constants.LeadType.AfterSales && //Only return Sales Leads
                           (l.AssignedUserId == null || l.AssignedUserId == loggedInUserId) && //It has to be unassigned or assigned to self
                           (leadTypeId == null ? true : l.LeadTypeId == leadTypeId) &&
                           (leadStatusId == null ? true : l.LeadStatusId == leadStatusId) &&
                           (startDate == null ? true : (l.CreatedDate >= startDate && l.CreatedDate <= endDate)))
                           .ToList();
                        return afterSalesLeads;
                    default:
                        return null;
                }

            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }

        //Dealers
        public Leads GetLeadDetailForDealer(int leadId, int dealerId)
        {
            try
            {
                return _db.Leads.Where(l => l.Id == leadId && l.DealerId == dealerId).FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
        public string AssignLeadForDealer(int selectedUserId, int leadId, int dealerId)
        {
            try
            {
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == dealerId).FirstOrDefault();
                if (lead != null)
                {
                    var userToBeAssigned = _db.Users.Where(u => u.Id == selectedUserId && u.DealerId == dealerId && u.IsActive == true).FirstOrDefault();
                    if (userToBeAssigned == null)
                    {
                        return "Invalid User";
                    }

                    //Check if it's Sales - 1 or AfterSales - 2
                    if (lead.LeadType.LeadTypeCode == Constants.LeadType.Sales) //If it's Sales
                    {
                        //Only assign if userToBeAssigned is Sales
                        if (userToBeAssigned.Roles.RoleCode != Constants.Roles.Sales)
                        {
                            return "Invalid user role.";
                        }

                        lead.AssignedUserId = selectedUserId;

                        //Change Lead Status - If it's New -> Change to Accepted
                        if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.SalesNew)
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.SalesAccepted).First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }
                    else //If it's AfterSales
                    {
                        //Only assign if userToBeAssigned is AfterSales
                        if (userToBeAssigned.Roles.RoleCode != Constants.Roles.AfterSales)
                        {
                            return "Invalid user role.";
                        }

                        lead.AssignedUserId = selectedUserId;

                        //Change Lead Status - If it's New -> Change to Accepted
                        if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.AfterSalesNew)
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.AfterSalesAccepted).First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }

                    _db.Entry(lead).State = EntityState.Modified;
                    _db.SaveChanges();
                    return "Success";
                }
                return "Lead not found";
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occurred";
            }
        }
        public string DeAssignLeadForDealer(int leadId, int dealerId)
        {
            try
            {
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == dealerId).FirstOrDefault();

                if (lead != null)
                {
                    //Check if it's Sales - 1 or AfterSales - 2
                    if (lead.Users.RoleId == 1) //If it's Sales
                    {
                        //Change Lead Status - If it's Accepted -> Change to New
                        if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.SalesAccepted)
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.SalesNew).First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }
                    else //If it's AfterSales
                    {
                        //Change Lead Status - If it's Accepted -> Change to New
                        if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.AfterSalesAccepted)
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.AfterSalesNew).First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }

                    lead.AssignedUserId = null;
                    _db.Entry(lead).State = EntityState.Modified;
                    _db.SaveChanges();
                    return "Success";
                }
                return "Invalid Lead";
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occurred";
            }
        }


        //Users
        public Leads GetLeadDetailForUser(int loggedInUserId, int id)
        {
            try
            {
                var user = _db.Users.Where(u => u.Id == loggedInUserId && u.IsActive == true).First();
                var lead = _db.Leads.Where(l => l.Id == id && l.DealerId == user.DealerId).FirstOrDefault();
                if (lead == null)
                {
                    return null;
                }

                if (lead.LeadType.LeadTypeCode == Constants.LeadType.Sales) //If it's Sales
                {
                    //Only return if both the roles match
                    if (user.Roles.RoleCode == Constants.Roles.Sales)
                    {
                        return lead;
                    }
                    return null;
                }
                else //If it's AfterSales
                {
                    //Only return if both the roles match
                    if (user.Roles.RoleCode == Constants.Roles.AfterSales)
                    {
                        return lead;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
        public string UpdateLeadDetails(Leads model, int loggedInUserId)
        {
            try
            {
                string result = "";
                var leadFromDb = _db.Leads.Where(m => m.Id == model.Id).FirstOrDefault();
                if (leadFromDb != null)
                {
                    leadFromDb.UpdatedBy = loggedInUserId;
                    leadFromDb.UpdatedDate = DateTime.Now;
                    leadFromDb.Comments = model.Comments;
                    var leadstatuscode = _db.LeadStatus.FirstOrDefault(l => l.Id == model.LeadStatusId).LeadStatusCode;
                    if (leadstatuscode == null)
                    {
                        return "Error occurred";
                    }
                    if (Common.Utility.LeadStatusValidation.IsPrevious(leadFromDb.LeadStatus.LeadStatusCode,leadstatuscode))
                    {
                        leadFromDb.LeadStatusId = model.LeadStatusId;
                        result += "Lead Status updated. ";
                    }
                    else
                    {
                        result += "Lead Status can not be updated.";
                    }
                    leadFromDb.UserComments = model.UserComments;
                    result += "User comments updated";
                    _db.Entry(leadFromDb).State = EntityState.Modified;
                    _db.SaveChanges();
                    return result;
                }
                return "Error occurred";
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occured";
            }
        }
        public string AssignLeadForUser(int loggedInUserId, int leadId)
        {
            try
            {
                var userToBeAssigned = _db.Users.Where(u => u.Id == loggedInUserId && u.IsActive == true).First();
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == userToBeAssigned.DealerId).FirstOrDefault();
                if (lead == null)
                {
                    return "Error occurred";
                }

                if (lead.LeadType.LeadTypeCode == Constants.LeadType.Sales) //If it's Sales
                {
                    //Only assign if userToBeAssigned is Sales
                    if (userToBeAssigned.Roles.RoleCode != Constants.Roles.Sales)
                    {
                        return "Invalid user role.";
                    }

                    lead.AssignedUserId = loggedInUserId;

                    //Change Lead Status - If it's New -> Change to Accepted
                    if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.SalesNew)
                    {
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.SalesAccepted).First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                }
                else //If it's AfterSales
                {
                    //Only assign if userToBeAssigned is AfterSales
                    if (userToBeAssigned.Roles.RoleCode != Constants.Roles.AfterSales)
                    {
                        return "Invalid user role.";
                    }

                    lead.AssignedUserId = loggedInUserId;

                    //Change Lead Status - If it's New -> Change to Accepted
                    if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.AfterSalesNew)
                    {
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.AfterSalesAccepted).First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                }

                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occured";
            }
        }
        public string DeAssignLeadForUser(int loggedInUserId, int leadId)
        {
            try
            {
                var userToBeAssigned = _db.Users.Where(u => u.Id == loggedInUserId && u.IsActive == true).First();
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == userToBeAssigned.DealerId).FirstOrDefault();
                if (lead == null)
                {
                    return "Operation can not be performed";
                }

                if (lead.LeadType.LeadTypeCode == Constants.LeadType.Sales) //If it's Sales
                {
                    //Only assign if userToBeAssigned is Sales
                    if (userToBeAssigned.Roles.RoleCode != Constants.Roles.Sales)
                    {
                        return "Invalid user role.";
                    }

                   

                    //Change Lead Status - If it's Accepted -> Change to New
                    if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.SalesAccepted)
                    {
                        lead.AssignedUserId = null;
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.SalesNew).First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                    else
                    {
                        return "Operation can not be performed";
                    }
                }
                else //If it's AfterSales
                {
                    //Only assign if userToBeAssigned is AfterSales
                    if (userToBeAssigned.Roles.RoleCode != Constants.Roles.AfterSales)
                    {
                        return "Invalid user role.";
                    }

                    

                    //Change Lead Status - If it's Accepted-> Change to New 
                    if (lead.LeadStatus.LeadStatusCode == Constants.LeadStatus.AfterSalesAccepted)
                    {
                        lead.AssignedUserId = null;
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == Constants.LeadStatus.AfterSalesNew).First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                    else
                    {
                        return "Operation can not be performed";
                    }
                }

                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return "Error occured.";
            }
        }

        //Dropdown Methods
        public IEnumerable<LeadStatus> GetLeadStatusDropDown(int loggedInUserId, string leadTypeCode)
        {
            try
            {
                var loggedInUser = _db.Users.Where(u => u.Id == loggedInUserId && u.IsActive == true).First();

                switch (loggedInUser.Roles.RoleCode)
                {
                    case Constants.Roles.Dealer:
                        return _db.LeadStatus.Where(l =>
                           (string.IsNullOrEmpty(leadTypeCode) ? true : l.LeadType.LeadTypeCode == leadTypeCode));
                    case Constants.Roles.Sales:
                        return _db.LeadStatus.Where(x => x.LeadType.LeadTypeCode == Constants.LeadType.Sales);
                    case Constants.Roles.AfterSales:
                        return _db.LeadStatus.Where(x => x.LeadType.LeadTypeCode == Constants.LeadType.AfterSales);
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
        public IEnumerable<LeadType> GetLeadTypeDropDown()
        {
            try
            {
                return _db.LeadType;
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                return null;
            }
        }
    }
}

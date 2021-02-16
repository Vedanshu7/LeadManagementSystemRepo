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
    public class LeadRepository : ILeadRepository
    {
        private readonly LMSAzureEntities _db;
        public LeadRepository()
        {
            _db = new LMSAzureEntities();
        }

        //Dealers
        public List<Leads> GetDealerLeadList(int dealerId)
        {
            return _db.Leads.Where(m => m.DealerId == dealerId).ToList();
        }
        public Leads GetLeadDetailForDealer(int leadId, int dealerId)
        {
            return _db.Leads.Where(l => l.Id == leadId && l.DealerId == dealerId).FirstOrDefault();
        }
        public string AssignLeadForDealer(int selectedUserId, int leadId, int dealerId)
        {
            try
            {
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == dealerId).FirstOrDefault();
                if (lead != null)
                {
                    var userToBeAssigned = _db.Users.Where(u => u.Id == selectedUserId && u.DealerId == dealerId).FirstOrDefault();
                    if (userToBeAssigned == null)
                    {
                        return "Invalid User";
                    }

                    //Check if it's Sales - 1 or AfterSales - 2

                    if (lead.LeadType.LeadTypeCode == "SL") //If it's Sales
                    {
                        //Only assign if userToBeAssigned is Sales
                        if (userToBeAssigned.Roles.RoleCode != "S")
                        {
                            return "Invalid user role.";
                        }

                        lead.AssignedUserId = selectedUserId;

                        //Change Lead Status - If it's New -> Change to Accepted
                        if (lead.LeadStatus.LeadStatusCode == "S-N1")
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-A2").First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }
                    else //If it's AfterSales
                    {
                        //Only assign if userToBeAssigned is AfterSales
                        if (userToBeAssigned.Roles.RoleCode != "AS")
                        {
                            return "Invalid user role.";
                        }

                        lead.AssignedUserId = selectedUserId;

                        //Change Lead Status - If it's New -> Change to Accepted
                        if (lead.LeadStatus.LeadStatusCode == "AS-N8")
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "AS-A9").First().Id;
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
                //TODO: Add Logger
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
                        if (lead.LeadStatus.LeadStatusCode == "S-A2")
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-N1").First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }
                    else //If it's AfterSales
                    {
                        //Change Lead Status - If it's Accepted -> Change to New
                        if (lead.LeadStatus.LeadStatusCode == "AS-A9")
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "AS-N8").First().Id;
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
                //TODO: Add Logger
                return "Error occurred";
            }
        }

        //Users
        public Leads GetLeadDetailForUser(int loggedInUserId, int id)
        {
            //TODO:Return Only LogedIn Dealers Leads
            var user = _db.Users.Where(u => u.Id == loggedInUserId).First();
            var lead = _db.Leads.Where(l => l.Id == id && l.DealerId == user.DealerId).FirstOrDefault();
            if (lead == null)
            {
                return null;
            }

            if (lead.LeadType.LeadTypeCode == "SL") //If it's Sales
            {
                //Only return if both the roles match
                if (user.Roles.RoleCode == "S")
                {
                    return lead;
                }
                return null;
            }
            else //If it's AfterSales
            {
                //Only assign if userToBeAssigned is AfterSales
                if (user.Roles.RoleCode == "AS")
                {
                    return lead;
                }
                return null;
            }
        }
        public List<Leads> GetUserLeadList(int loggedInUserId)
        {
            try
            {
                var dealerId = _db.Users.Find(loggedInUserId).DealerId;
                List<Leads> list = _db.Leads.
                    Where(
                    m => m.DealerId == dealerId && //To return only concerned dealers
                    m.LeadType.LeadTypeCode == "SL" &&  //To return only Sales Leads
                    (m.AssignedUserId == null || m.AssignedUserId == loggedInUserId)) //Either the Lead has to be Unassigned, or assigned to the current user
                    .ToList(); //TODO: Change static LeadType Checking
                return list;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                throw;
            }
        }
        public string UpdateLeadDetails(Leads model, int loggedInUserId)
        {
            try
            {
                var leadFromDb = _db.Leads.Where(m => m.Id == model.Id).FirstOrDefault();
                if (leadFromDb != null)
                {
                    leadFromDb.UpdatedBy = loggedInUserId;
                    leadFromDb.UpdatedDate = DateTime.Now;
                    leadFromDb.Comments = model.Comments;
                    leadFromDb.LeadStatusId = model.LeadStatusId;

                    _db.Entry(leadFromDb).State = EntityState.Modified;
                    _db.SaveChanges();
                    return "Success";
                }
                return "Error occurred";
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                throw;
            }
        }
        public string AssignLeadForUser(int loggedInUserId, int leadId)
        {
            try
            {
                var userToBeAssigned = _db.Users.Where(u => u.Id == loggedInUserId).First();
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == userToBeAssigned.DealerId).FirstOrDefault();
                if (lead == null)
                {
                    return "Error occurred";
                }

                if (lead.LeadType.LeadTypeCode == "SL") //If it's Sales
                {
                    //Only assign if userToBeAssigned is Sales
                    if (userToBeAssigned.Roles.RoleCode != "S")
                    {
                        return "Invalid user role.";
                    }

                    lead.AssignedUserId = loggedInUserId;

                    //Change Lead Status - If it's New -> Change to Accepted
                    if (lead.LeadStatus.LeadStatusCode == "S-N1")
                    {
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-A2").First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                }
                else //If it's AfterSales
                {
                    //Only assign if userToBeAssigned is AfterSales
                    if (userToBeAssigned.Roles.RoleCode != "AS")
                    {
                        return "Invalid user role.";
                    }

                    lead.AssignedUserId = loggedInUserId;

                    //Change Lead Status - If it's New -> Change to Accepted
                    if (lead.LeadStatus.LeadStatusCode == "AS-N8")
                    {
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "AS-A9").First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                }

                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                return "Error occurred";
            }
        }

        public string DeAssignLeadForUser(int loggedInUserId, int leadId)
        {
            try
            {
                var userToBeAssigned = _db.Users.Where(u => u.Id == loggedInUserId).First();
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == userToBeAssigned.DealerId).FirstOrDefault();
                if (lead == null)
                {
                    return "Error occurred";
                }

                if (lead.LeadType.LeadTypeCode == "SL") //If it's Sales
                {
                    //Only assign if userToBeAssigned is Sales
                    if (userToBeAssigned.Roles.RoleCode != "S")
                    {
                        return "Invalid user role.";
                    }

                    lead.AssignedUserId = null;

                    //Change Lead Status - If it's Accepted -> Change to New
                    if (lead.LeadStatus.LeadStatusCode == "S-A2")
                    {
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-N1").First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                }
                else //If it's AfterSales
                {
                    //Only assign if userToBeAssigned is AfterSales
                    if (userToBeAssigned.Roles.RoleCode != "AS")
                    {
                        return "Invalid user role.";
                    }

                    lead.AssignedUserId = loggedInUserId;

                    //Change Lead Status - If it's Accepted-> Change to New 
                    if (lead.LeadStatus.LeadStatusCode == "AS-A9")
                    {
                        var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "AS-N8").First().Id;
                        lead.LeadStatusId = idOfStatusCode;
                    }
                }

                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                return "Error occurred";
            }
        }
    }
}

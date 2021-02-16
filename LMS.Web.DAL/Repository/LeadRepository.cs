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
        public bool AssignLeadForDealer(int selectedUserId, int leadId, int dealerId)
        {
            try
            {
                var lead = _db.Leads.Where(l => l.Id == leadId && l.DealerId == dealerId).FirstOrDefault();
                if (lead != null)
                {
                    lead.AssignedUserId = selectedUserId;

                    //Check if it's Sales - 1 or AfterSales - 2

                    if (lead.LeadType.LeadTypeCode == "SL") //If it's Sales
                    {
                        //Change Lead Status - If it's New -> Change to Accepted
                        if (lead.LeadStatus.LeadStatusCode == "S-N1")
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-A2").First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }
                    else //If it's AfterSales
                    {
                        //Change Lead Status - If it's New -> Change to Accepted
                        if (lead.LeadStatus.LeadStatusCode == "AS-N8")
                        {
                            var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "AS-A9").First().Id;
                            lead.LeadStatusId = idOfStatusCode;
                        }
                    }

                    _db.Entry(lead).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                return false;
            }
        }
        public bool DeAssignLeadForDealer(int leadId, int dealerId)
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
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                return false;
            }
        }

        //Users
        public Leads GetLeadDetailForUser(int id)
        {
            //TODO:Return Only LogedIn Dealers Leads
            //_db.Leads.Where(l => l.DealerId == dealerId && l.Id==Id).First();
            Leads data = _db.Leads.Find(id);
            return data;
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
        public bool UpdateLeadDetails(Leads model, int loggedInUserId)
        {
            try
            {
                var leadFromDb = _db.Leads.Where(m => m.Id == model.Id).FirstOrDefault();
                if (leadFromDb != null)
                {
                    leadFromDb.UpdatedBy = loggedInUserId;
                    leadFromDb.UpdatedDate = DateTime.Now;
                    leadFromDb.CustomerEmail = model.CustomerEmail;
                    leadFromDb.CustomerContactNumber = model.CustomerContactNumber;
                    leadFromDb.LeadStatusId = model.LeadStatusId;

                    _db.Entry(leadFromDb).State = EntityState.Modified;
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                throw;
            }
        }
        public bool AssignLeadForUser(int loggedInUserId, int leadId)
        {
            try
            {
                var lead = _db.Leads.Find(leadId);
                lead.AssignedUserId = loggedInUserId;

                //Change Lead Status - If it's New -> Change to Accepted
                if (lead.LeadStatus.LeadStatusCode == "S-N1")
                {
                    var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-A2").First().Id;
                    lead.LeadStatusId = idOfStatusCode;
                }



                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                return false;
            }
        }
        public bool DeAssignLeadForUser(int leadId)
        {
            try
            {
                var lead = _db.Leads.Find(leadId);
                lead.AssignedUserId = null;

                //Change Lead Status - If it's Accepted -> Change to New
                if (lead.LeadStatus.LeadStatusCode == "S-A2")
                {
                    var idOfStatusCode = _db.LeadStatus.Where(s => s.LeadStatusCode == "S-N1").First().Id;
                    lead.LeadStatusId = idOfStatusCode;
                }

                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                return false;
            }
        }
    }
}

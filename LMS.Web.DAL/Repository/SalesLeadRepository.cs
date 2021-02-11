using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Common;
using System.Data.Entity;

namespace LMS.Web.DAL.Repository
{
    public class SalesLeadRepository : ISalesLeadRepository
    {
        private readonly Database.LMSEntitiesAzure _db;
        public SalesLeadRepository()
        {
            _db = new Database.LMSEntitiesAzure();
        }

        public Leads GetLeadDetail(int id)
        {
            //TODO:Return Only LogedIn Dealers Leads
            //_db.Leads.Where(l => l.DealerId == dealerId && l.Id==Id).First();
            Leads data = _db.Leads.Find(id);
            return data;
        }

        public List<Leads> GetSalesLeadList(int loggedInUserId)
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

        public bool AssignLead(int loggedInUserId, int leadId)
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

        public bool DeAssignLead(int leadId)
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

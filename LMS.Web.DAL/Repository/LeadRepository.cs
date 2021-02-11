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
        private readonly Database.LMSEntitiesAzure _db;
        public LeadRepository()
        {
            _db = new Database.LMSEntitiesAzure();
        }
        public List<Leads> GetDealerLeadList(int dealerId)
        {
            return _db.Leads.Where(m => m.DealerId == dealerId).ToList();
        }
        public Leads GetLead(int leadId)
        {
            //TODO: Returns only LogedIn Dealers Lead
            return _db.Leads.Find(leadId);
        }

        public bool AssignLead(int selectedUserId,int leadId)
        {
            try
            {
                //TODO: Assign only LogedIn Dealers Lead
                var lead = _db.Leads.Find(leadId);
                lead.AssignedUserId = selectedUserId;
                //TODO: Check the role or LeadStatusCode of lead and change status accordingly 
                _db.Entry(lead).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                //TODO: Add Logger
                return false;
            }
        }

        public bool DeAssignLead(int leadId)
        {
            try
            {
                //TODO: DeAssign only LogedIn Dealers Lead
                var lead = _db.Leads.Find(leadId);
                //TODO: Check the role or LeadStatusCode of lead and change status accordingly 
                lead.AssignedUserId = null;
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

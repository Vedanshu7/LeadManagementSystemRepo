using LMS.Web.DAL.Database;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
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
            return _db.Leads.Find(leadId);
        }
    }
}

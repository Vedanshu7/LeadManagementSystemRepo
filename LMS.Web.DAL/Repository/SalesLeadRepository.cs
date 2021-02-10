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
            Leads data = _db.Leads.Find(id);
            return data;
        }

        public List<Leads> GetSalesLeadList(int dealerId,int loggedInUserId)
        {
            

            List<Leads> list = _db.Leads.Where(m=>m.DealerId==dealerId && m.AssignedUserId == loggedInUserId).ToList();
            
            return list;
        }

        public bool UpdateLeadDetails(Leads model,int loggedInUserId)
        {
            var status = _db.LeadStatus.Where(m=>m.DisplayName.Equals(model.LeadStatus.DisplayName));
            //TODO:Convert lead status string to Id.
            foreach(var item in status)
            {
                if (item.DisplayName != model.LeadStatus.DisplayName.ToString())
                {
                    continue;
                }
                model.LeadStatusId = item.Id;
            }
            var leadFromDb = _db.Leads.Where(m => m.Id == model.Id).FirstOrDefault();
            if(leadFromDb!=null)
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
    }
}

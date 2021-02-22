using LMS.Api.DAL.Database;
using LMS.Api.DAL.Interface;
using LMS.Common.Dtos;
using LMS.Common.Enums;
using LMS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Api.DAL.Repository
{
    public class LeadRepository : ILeadRepository
    {
        private readonly LMSAzureEntities _db;
        public LeadRepository()
        {
            _db = new LMSAzureEntities();
        }

        public LeadResult AddLead(LeadDto leadDto)
        {
            try
            {
                var lead = new Leads();
                lead.CustomerName = leadDto.CustomerName;
                lead.DealerId = _db.Dealers.Where(d => d.DealerCode == leadDto.DealerCode).First().Id;
                lead.ModelId = _db.Models.Where(m => m.ModelCode == leadDto.ModelCode).First().Id;
                lead.CustomerEmail = leadDto.CustomerEmail;
                lead.CustomerContactNumber = leadDto.CustomerContactNumber;
                lead.LeadTypeId = _db.LeadType.Where(x => x.LeadTypeCode == leadDto.LeadTypeCode).First().Id;

                switch (leadDto.LeadTypeCode)
                {
                    case Common.Constants.LeadType.Sales:
                        lead.LeadStatusId = _db.LeadStatus.Where(x => x.LeadStatusCode == Common.Constants.LeadStatus.SalesNew).First().Id;
                        break;
                    case Common.Constants.LeadType.AfterSales:
                        lead.LeadStatusId = _db.LeadStatus.Where(x => x.LeadStatusCode == Common.Constants.LeadStatus.AfterSalesNew).First().Id;
                        break;
                }

                if (leadDto.ServiceType != null)
                {
                    lead.ServiceId = _db.Services.Where(s => s.Type == leadDto.ServiceType).First().Id;
                }

                if (leadDto.Comments != null)
                {
                    lead.Comments = leadDto.Comments;
                }

                lead.CreatedDate = DateTime.Now;
                _db.Leads.Add(lead);
                _db.SaveChanges();

                var result = new LeadResult();
                result.result = LeadResultEnum.Success;

                //TODO: Create Mailing List

                List<string> mailingList = new List<string>();
                string testEmail = "syserrorlogin@gmail.com";
                mailingList.Add(testEmail);
                result.mailingList = mailingList;

                return result;
            }
            catch (Exception e)
            {
                //TODO: Add Logger
                throw;
            }
        }
    }
}

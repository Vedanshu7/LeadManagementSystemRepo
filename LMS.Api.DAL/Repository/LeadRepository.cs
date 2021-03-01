using LMS.Api.DAL.Database;
using LMS.Api.DAL.Interface;
using LMS.Common.Dtos;
using LMS.Common.Enums;
using LMS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                //Mapping LeadDto -> Leads
                var lead = new Leads();
                lead.CustomerName = leadDto.CustomerName;
                lead.DealerId = _db.Dealers.Where(d => d.DealerCode == leadDto.DealerCode && d.IsActive == true).First().Id;
                lead.ModelId = _db.Models.Where(m => m.ModelCode == leadDto.ModelCode && m.IsActive == true).First().Id;

                //Check if brand exists for that dealer
                var brandId = _db.Brands.Where(b => b.BrandCode == leadDto.BrandCode && b.IsActive == true).First().Id;
                var dealerBrandMapping = _db.DealerBrandMappings.Where(x => x.DealerId == lead.DealerId && x.BrandId == brandId && x.IsActive == true).FirstOrDefault();
                if (dealerBrandMapping == null)
                {
                    return new LeadResult() { result = LeadResultEnum.Invalid };
                }
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
                        if (leadDto.ServiceType != null && leadDto.Date != null && leadDto.VIN != null)
                        {
                            var service = new Services();
                            service.Type = leadDto.ServiceType;
                            service.Date = leadDto.Date;
                            service.VIN = leadDto.VIN;
                            _db.Services.Add(service);
                            _db.SaveChanges();
                            lead.ServiceId = _db.Services.Where(s => s.Type == leadDto.ServiceType && s.Date == leadDto.Date && s.VIN == leadDto.VIN).First().Id;
                        }
                        break;
                }

                if (leadDto.Comments != null)
                {
                    lead.Comments = leadDto.Comments;
                }

                lead.CreatedDate = DateTime.Now;
                _db.Leads.Add(lead);
                _db.SaveChanges();

                //Forming Result Object
                var result = new LeadResult
                {
                    result = LeadResultEnum.Success
                };

                //Creating Mailing List
                List<string> mailingList = new List<string>();
                if (leadDto.LeadTypeCode == Common.Constants.LeadType.Sales)
                {
                    string salesMail = _db.Dealers.Where(m => m.Id == lead.DealerId && m.IsActive == true).FirstOrDefault().SalesEmail;
                    mailingList.Add(salesMail);
                    if (salesMail == null)
                    {
                        var mailingListFromDB = _db.Users.Where(m =>
                            m.DealerId == lead.DealerId &&
                            m.Roles.RoleCode == Common.Constants.Roles.Sales &&
                            m.IsActive == true)
                            .ToList();

                        foreach (var item in mailingListFromDB)
                        {
                            mailingList.Add(item.Email);
                        }
                    }
                }
                else
                {
                    string afterSalesMail = _db.Dealers.Where(m => m.Id == lead.DealerId && m.IsActive == true).FirstOrDefault().AfterSalesEmail;
                    mailingList.Add(afterSalesMail);
                    if (afterSalesMail == null)
                    {
                        var mailingListFromDB = _db.Users.Where(m =>
                        m.DealerId == lead.DealerId &&
                        m.Roles.RoleCode == Common.Constants.Roles.AfterSales &&
                        m.IsActive == true)
                        .ToList();
                        foreach (var item in mailingListFromDB)
                        {
                            mailingList.Add(item.Email);
                        }

                    }
                }

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

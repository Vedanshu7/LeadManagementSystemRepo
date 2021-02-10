using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using LMS.Web.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Web.DAL.Database;

namespace LMS.Web.BAL.Manager
{
    public class SalesLeadManager : ISalesLeadManager
    {
        private readonly ISalesLeadRepository _salesLeadRepository;
        public IMapper mapper;
        public MapperConfiguration config;
        public SalesLeadManager(ISalesLeadRepository salesLeadRepository)
        {
            _salesLeadRepository = salesLeadRepository;
            config = new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<SalesLeadViewModel, Leads>();
                 cfg.CreateMap<Leads, SalesLeadViewModel>();
             });
            mapper = config.CreateMapper();
        }

        public SalesLeadViewModel GetLeadDetail(int id)
        {
            Leads leadFromDb = _salesLeadRepository.GetLeadDetail(id);
            SalesLeadViewModel salesLead = mapper.Map<Leads, SalesLeadViewModel>(leadFromDb);
            salesLead.ModelName = leadFromDb.Models.Name;
            if (leadFromDb.AssignedUserId != null)
                salesLead.AssignedUserName = leadFromDb.Users.Name;
            return salesLead;
        }

        public List<SalesLeadViewModel> GetSalesLeadList(int loggedInUserId)
        {
            List<Leads> leadsFromDb = _salesLeadRepository.GetSalesLeadList(loggedInUserId);

            List<SalesLeadViewModel> salesLeads = mapper.Map<List<Leads>, List<SalesLeadViewModel>>(leadsFromDb);
            for (int i = 0; i < salesLeads.Count; i++)
            {
                salesLeads[i].LeadStatus = leadsFromDb[i].LeadStatus.DisplayName;
                salesLeads[i].ModelName = leadsFromDb[i].Models.Name;
                if (leadsFromDb[i].AssignedUserId != null)
                    salesLeads[i].AssignedUserName = leadsFromDb[i].Users.Name;
            }
            return salesLeads;

        }

        public bool UpdateLeadDetails(SalesLeadViewModel model, int loggedInUserId)
        {
            //TODO:Map lead status id.
            Leads lead = mapper.Map<SalesLeadViewModel, Leads>(model);
            lead.UpdatedBy = loggedInUserId;
            return _salesLeadRepository.UpdateLeadDetails(lead, loggedInUserId);
        }

        public bool AssignLead(int loggedInUserId, int leadId)
        {
            return _salesLeadRepository.AssignLead(loggedInUserId, leadId);
        }

        public bool DeAssignLead(int leadId)
        {
            return _salesLeadRepository.DeAssignLead(leadId);
        }
    }
}

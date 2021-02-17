using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;
using LMS.Web.DAL.Interface;
using AutoMapper;
using LMS.Web.DAL.Database;

namespace LMS.Web.BAL.Manager
{
    //TODO: Check for Null in every method
    public class LeadManager : ILeadManager
    {
        private readonly ILeadRepository _leadRepository;
        public IMapper mapper;
        public MapperConfiguration config;

        public LeadManager(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
            config = new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<UserLeadViewModel, Leads>();
                 cfg.CreateMap<Leads, UserLeadViewModel>();
                 cfg.CreateMap<LeadStatusViewModel, LeadStatus>();
                 cfg.CreateMap<LeadStatus, LeadStatusViewModel>();
             });
            mapper = config.CreateMapper();
        }
        //Dealers
        public List<DealerLeadViewModel> GetDealerLeadList(int dealerId)
        {
            var leads = _leadRepository.GetDealerLeadList(dealerId);
            List<DealerLeadViewModel> dealerLeads = new List<DealerLeadViewModel>();
            foreach (var lead in leads)
            {
                DealerLeadViewModel dealerLead = new DealerLeadViewModel();
                dealerLead.Id = lead.Id;
                dealerLead.CustomerName = lead.CustomerName;
                dealerLead.ModelName = lead.Models.Name;
                if (lead.AssignedUserId != null)
                    dealerLead.AssignedUserName = lead.Users.Name;
                dealerLead.CustomerEmail = lead.CustomerEmail;
                dealerLead.CustomerContactNumber = lead.CustomerContactNumber;
                dealerLead.LeadStatus = lead.LeadStatus.DisplayName;
                dealerLead.LeadType = lead.LeadType.DisplayName;
                if (lead.ServiceId != null)
                    dealerLead.ServiceType = lead.Services.Type;
                dealerLead.Comments = lead.Comments;
                dealerLeads.Add(dealerLead);
            }

            return dealerLeads;
        }
        public DealerLeadViewModel GetLeadDetailForDealer(int leadId, int dealerId)
        {
            var lead = _leadRepository.GetLeadDetailForDealer(leadId, dealerId);

            if (lead != null)
            {
                DealerLeadViewModel dealerLead = new DealerLeadViewModel();
                dealerLead.Id = lead.Id;
                dealerLead.CustomerName = lead.CustomerName;
                dealerLead.ModelName = lead.Models.Name;
                if (lead.AssignedUserId != null)
                    dealerLead.AssignedUserName = lead.Users.Name;
                dealerLead.CustomerEmail = lead.CustomerEmail;
                dealerLead.CustomerContactNumber = lead.CustomerContactNumber;
                dealerLead.LeadStatus = lead.LeadStatus.DisplayName;
                dealerLead.LeadType = lead.LeadType.DisplayName;
                if (lead.ServiceId != null)
                    dealerLead.ServiceType = lead.Services.Type;
                dealerLead.Comments = lead.Comments;
                return dealerLead;
            }
            return null;
        }
        public string AssignLeadForDealer(int selectedUserId, int leadId, int dealerId)
        {
            return _leadRepository.AssignLeadForDealer(selectedUserId, leadId, dealerId);
        }
        public string DeAssignLeadForDealer(int leadId, int dealerId)
        {
            return _leadRepository.DeAssignLeadForDealer(leadId, dealerId);
        }

        //Users
        public UserLeadViewModel GetLeadDetailForUser(int loggedInUserId, int id)
        {
            Leads leadFromDb = _leadRepository.GetLeadDetailForUser(loggedInUserId, id);
            if (leadFromDb != null)
            {
                UserLeadViewModel salesLead = mapper.Map<Leads, UserLeadViewModel>(leadFromDb);
                salesLead.ModelName = leadFromDb.Models.Name;
                if (leadFromDb.AssignedUserId != null)
                    salesLead.AssignedUserName = leadFromDb.Users.Name;
                return salesLead;
            }
            return null;
        }
        public List<UserLeadViewModel> GetUserLeadList(int loggedInUserId)
        {
            List<Leads> leadsFromDb = _leadRepository.GetUserLeadList(loggedInUserId);

            List<UserLeadViewModel> salesLeads = mapper.Map<List<Leads>, List<UserLeadViewModel>>(leadsFromDb);
            for (int i = 0; i < salesLeads.Count; i++)
            {
                salesLeads[i].LeadStatus = leadsFromDb[i].LeadStatus.DisplayName;
                salesLeads[i].ModelName = leadsFromDb[i].Models.Name;
                if (leadsFromDb[i].AssignedUserId != null)
                    salesLeads[i].AssignedUserName = leadsFromDb[i].Users.Name;
            }
            return salesLeads;

        }
        public string UpdateLeadDetails(UserLeadViewModel model, int loggedInUserId)
        {
            //TODO:Map lead status id.
            Leads lead = mapper.Map<UserLeadViewModel, Leads>(model);
            lead.UpdatedBy = loggedInUserId;
            return _leadRepository.UpdateLeadDetails(lead, loggedInUserId);
        }
        public string AssignLeadForUser(int loggedInUserId, int leadId)
        {
            return _leadRepository.AssignLeadForUser(loggedInUserId, leadId);
        }
        public string DeAssignLeadForUser(int loggedInUserId, int leadId)
        {
            return _leadRepository.DeAssignLeadForUser(loggedInUserId, leadId);
        }

        public IEnumerable<LeadStatusViewModel> GetLeadStatusDropDown()
        {
            var leadStatusFromDb = _leadRepository.GetLeadStatusDropDown();
            var leadStatusViewModels = mapper.Map<IEnumerable<LeadStatus>, IEnumerable<LeadStatusViewModel>>(leadStatusFromDb);
            return leadStatusViewModels;
        }
    }
}

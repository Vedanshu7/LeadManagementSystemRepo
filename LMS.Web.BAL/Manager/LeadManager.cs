using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;
using LMS.Web.DAL.Interface;
using AutoMapper;
using LMS.Web.DAL.Database;
using System;
using System.Globalization;

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
                 cfg.CreateMap<LeadType, LeadTypeViewModel>();
             });
            mapper = config.CreateMapper();
        }

        //Dealers
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
                dealerLead.CreatedDate = lead.CreatedDate;
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
                salesLead.LeadStatus = leadFromDb.LeadStatus.DisplayName;
                return salesLead;
            }
            return null;
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

        //Common
        public List<DealerLeadViewModel> GetLeadList(FilterLeadListViewModel filter, int loggedInUserId)
        {
            List<Leads> leads;
            if (filter != null)
            {
                //Converting date of filter from string to DateTime 
                DateTime? startDate = null, endDate = null;
                if (filter.startDate != null)
                {
                    startDate = DateTime.ParseExact(filter.startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
                    endDate = DateTime.ParseExact(filter.endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
                }

                leads = _leadRepository.GetLeadList(startDate, endDate, filter.leadStatusId, filter.leadTypeId, loggedInUserId);
            }
            else
            {
                leads = _leadRepository.GetLeadList(null, null, null, null, loggedInUserId);
            }

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
                dealerLead.CreatedDate = lead.CreatedDate;
                dealerLead.LeadType = lead.LeadType.DisplayName;
                if (lead.ServiceId != null)
                    dealerLead.ServiceType = lead.Services.Type;
                dealerLead.Comments = lead.Comments;
                dealerLeads.Add(dealerLead);
            }
            return dealerLeads;
        }

        //Dropdowns
        public IEnumerable<LeadTypeViewModel> GetLeadTypeDropDown()
        {
            var leadType = _leadRepository.GetLeadTypeDropDown();
            var leadTypeViewModel = mapper.Map<IEnumerable<LeadType>, IEnumerable<LeadTypeViewModel>>(leadType);
            return leadTypeViewModel;
        }
        public IEnumerable<LeadStatusViewModel> GetLeadStatusDropDown(int loggedInUserId, string leadTypeCode)
        {
            var leadStatusFromDb = _leadRepository.GetLeadStatusDropDown(loggedInUserId, leadTypeCode);
            var leadStatusViewModels = mapper.Map<IEnumerable<LeadStatus>, IEnumerable<LeadStatusViewModel>>(leadStatusFromDb);
            return leadStatusViewModels;
        }
    }
}

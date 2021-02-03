using LMS.Web.BAL.Interface;
using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Web.DAL.Interface;

namespace LMS.Web.BAL.Manager
{
    public class LeadManager : ILeadManager
    {
        private readonly ILeadRepository _leadRepository;

        public LeadManager(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }
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
                dealerLead.AssignedUserName = lead.Users.Name;
                dealerLead.CustomerEmail = lead.CustomerEmail;
                dealerLead.CustomerContactNumber = lead.CustomerContactNumber;
                dealerLead.LeadStatus = lead.LeadStatus.DisplayName;
                dealerLead.LeadType = lead.LeadType.DisplayName;
                dealerLead.ServiceType = lead.Services.Type;
                dealerLead.Comments = lead.Comments;
                dealerLeads.Add(dealerLead);
            }

            return dealerLeads;
        }
    }
}

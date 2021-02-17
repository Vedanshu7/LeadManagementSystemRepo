using LMS.Web.BAL.ViewModels;
using System.Collections.Generic;

namespace LMS.Web.BAL.Interface
{
    public interface ILeadManager
    {
        //Dealer
        List<DealerLeadViewModel> GetDealerLeadList(int dealerId);
        DealerLeadViewModel GetLeadDetailForDealer(int leadId, int dealerId);
        string AssignLeadForDealer(int selectedUserId, int leadId, int dealerId);
        string DeAssignLeadForDealer(int leadId, int dealerId);

        //User
        List<UserLeadViewModel> GetUserLeadList(int loggedInUserId);
        UserLeadViewModel GetLeadDetailForUser(int loggedInUserId, int id);
        string UpdateLeadDetails(UserLeadViewModel model, int loggedInUserId);
        string AssignLeadForUser(int loggedInUserId, int leadId);
        string DeAssignLeadForUser(int loggedInUserId, int leadId);
        IEnumerable<LeadStatusViewModel> GetLeadStatusDropDown(int leadId);
    }
}

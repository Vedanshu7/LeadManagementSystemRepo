using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Interface
{
    public interface ILeadManager
    {
        //Dealer
        List<DealerLeadViewModel> GetDealerLeadList(int dealerId);
        DealerLeadViewModel GetLeadDetailForDealer(int leadId, int dealerId);
        bool AssignLeadForDealer(int selectedUserId, int leadId, int dealerId);
        bool DeAssignLeadForDealer(int leadId, int dealerId);

        //User
        List<UserLeadViewModel> GetUserLeadList(int loggedInUserId);
        UserLeadViewModel GetLeadDetailForUser(int id);
        bool UpdateLeadDetails(UserLeadViewModel model, int loggedInUserId);
        bool AssignLeadForUser(int loggedInUserId, int leadId);
        bool DeAssignLeadForUser(int leadId);
    }
}

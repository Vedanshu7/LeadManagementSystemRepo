using LMS.Web.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Interface
{
    public interface ILeadRepository
    {
        //Dealer
        List<Leads> GetDealerLeadList(int dealerId);
        Leads GetLeadDetailForDealer(int leadId, int dealerId);
        bool AssignLeadForDealer(int selectedUserId, int leadId, int dealerId);
        bool DeAssignLeadForDealer(int leadId, int dealerId);

        //User
        List<Leads> GetUserLeadList(int loggedInUserId);
        Leads GetLeadDetailForUser(int id);
        bool UpdateLeadDetails(Leads model, int loggedInUserId);
        bool AssignLeadForUser(int loggedInUserId, int leadId);
        bool DeAssignLeadForUser(int leadId);
    }
}

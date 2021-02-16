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
        string AssignLeadForDealer(int selectedUserId, int leadId, int dealerId);
        string DeAssignLeadForDealer(int leadId, int dealerId);

        //User
        List<Leads> GetUserLeadList(int loggedInUserId);
        Leads GetLeadDetailForUser(int loggedInUserId, int id);
        string UpdateLeadDetails(Leads model, int loggedInUserId);
        string AssignLeadForUser(int loggedInUserId, int leadId);
        string DeAssignLeadForUser(int loggedInUserId, int leadId);
    }
}

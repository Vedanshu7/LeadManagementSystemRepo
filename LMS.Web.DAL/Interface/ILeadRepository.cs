using LMS.Web.DAL.Database;
using System;
using System.Collections.Generic;

namespace LMS.Web.DAL.Interface
{
    public interface ILeadRepository
    {
        //Common
        List<Leads> GetLeadList(DateTime? startDate, DateTime? endDate, int? leadStatusId, int? leadTypeId, int loggedInUserId);

        //Dealer
        Leads GetLeadDetailForDealer(int leadId, int dealerId);
        string AssignLeadForDealer(int selectedUserId, int leadId, int dealerId);
        string DeAssignLeadForDealer(int leadId, int dealerId);

        //User
        Leads GetLeadDetailForUser(int loggedInUserId, int id);
        string UpdateLeadDetails(Leads model, int loggedInUserId);
        string AssignLeadForUser(int loggedInUserId, int leadId);
        string DeAssignLeadForUser(int loggedInUserId, int leadId);

        //Dropdowns
        IEnumerable<LeadStatus> GetLeadStatusDropDown(int loggedInUserId, string leadTypeCode);
        IEnumerable<LeadType> GetLeadTypeDropDown();
    }
}

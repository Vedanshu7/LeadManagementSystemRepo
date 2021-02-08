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
        List<Leads> GetDealerLeadList(int dealerId);
        Leads GetLead(int leadId);
        bool AssignLead(int selectedUserId, int leadId);
    }
}

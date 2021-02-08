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
        List<DealerLeadViewModel> GetDealerLeadList(int dealerId);
        DealerLeadViewModel GetLead(int leadId);

        bool AssignLead(int selectedUserId, int leadId);
    }
}

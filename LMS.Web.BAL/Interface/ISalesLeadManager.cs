
using LMS.Web.BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.Interface
{
    public interface ISalesLeadManager
    {
        List<SalesLeadViewModel> GetSalesLeadList(int dealerId,int loggedInUserId );

        SalesLeadViewModel GetLeadDetail(int id);

        bool UpdateLeadDetails(SalesLeadViewModel model,int loggedInUserId);
    }
}

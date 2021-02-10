using LMS.Web.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Interface
{
    public interface ISalesLeadRepository
    {
        List<Leads> GetSalesLeadList(int dealerId, int loggedInUserId);

        Leads GetLeadDetail(int id);

        bool UpdateLeadDetails(Leads model,int loggedInUserId);
    }
}

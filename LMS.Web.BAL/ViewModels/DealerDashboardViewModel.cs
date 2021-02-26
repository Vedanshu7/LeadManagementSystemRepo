using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class DealerDashboardViewModel
    {
        public List<UserLeadCountViewModel> userLeadCounts { get; set; }
        public List<DealerLeadViewModel> LatestLeads { get; set; }
    }
}

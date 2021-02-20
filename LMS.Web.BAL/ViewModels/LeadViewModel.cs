using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class LeadViewModel
    {
        public IEnumerable<DealerLeadViewModel> Leads { get; set; }
        public FilterLeadListViewModel Filters { get; set; }
    }
}

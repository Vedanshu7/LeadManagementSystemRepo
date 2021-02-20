using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class FilterLeadListViewModel
    {
        public FilterLeadListViewModel()
        {
            startDate = DateTime.Today.AddDays(-7);
            endDate = DateTime.Today;
        }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? leadStatusId { get; set; }
        public int? leadTypeId { get; set; }
        public int? loggedInUserId { get; set; }
    }
}

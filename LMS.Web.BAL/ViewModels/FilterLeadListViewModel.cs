using System;
using System.Globalization;

namespace LMS.Web.BAL.ViewModels
{
    public class FilterLeadListViewModel
    {
        public FilterLeadListViewModel()
        {
            startDate = DateTime.Today.AddDays(-7).Date.ToString("MM-dd-yyyy").Replace('-', '/');
            endDate = DateTime.Today.Date.ToString("MM-dd-yyyy").Replace('-', '/');
        }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? leadStatusId { get; set; }
        public int? leadTypeId { get; set; }
        public int? loggedInUserId { get; set; }
        public bool flag { get; set; }
    }
}

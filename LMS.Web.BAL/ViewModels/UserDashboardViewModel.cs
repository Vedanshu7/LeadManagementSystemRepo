using System.Collections.Generic;

namespace LMS.Web.BAL.ViewModels
{
    public class UserDashboardViewModel
    {
        public List<UserLeadViewModel> userAssignedLeadList { get; set; }
        public List<UserLeadViewModel> userNewLeadList { get; set; }      
    }   
}

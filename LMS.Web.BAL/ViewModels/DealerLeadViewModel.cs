using System;

namespace LMS.Web.BAL.ViewModels
{
    public class DealerLeadViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ModelName { get; set; }
        public string AssignedUserName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContactNumber { get; set; }
        public string LeadStatus { get; set; }
        public string LeadType { get; set; }
        public string ServiceType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class SalesLeadViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ModelName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContactNumber { get; set; }
        public int? LeadStatusId { get; set; }
        public string LeadStatus { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string LastUpdatedBy { get; set; }

        //TODO:Remove created date,updated date etc.

    }
}

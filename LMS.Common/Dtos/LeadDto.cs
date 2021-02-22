using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Common.Dtos
{
    public class LeadDto
    {
        public string CustomerName { get; set; }
        public string DealerCode { get; set; }
        public string ModelCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContactNumber { get; set; }
        public string LeadTypeCode { get; set; }
        public string ServiceType { get; set; }
        public string Comments { get; set; }
    }
}

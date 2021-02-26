using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.DAL.Models
{
    public class UserLeadCountModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int AciveCounts { get; set; }
        public int ClosedCounts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class AdminBrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brandcode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string BrandCreatedBy { get; set; }
        public string BrandUpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}

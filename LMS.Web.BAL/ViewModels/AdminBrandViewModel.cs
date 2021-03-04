using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class AdminBrandViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
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

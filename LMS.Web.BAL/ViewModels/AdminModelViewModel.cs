using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class AdminModelViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int BrandId { get; set; }
        public string Brand { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }

        [Required]
        public string ModelCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}

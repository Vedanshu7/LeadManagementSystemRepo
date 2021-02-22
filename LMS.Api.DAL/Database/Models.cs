//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LMS.Api.DAL.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Models
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Models()
        {
            this.Leads = new HashSet<Leads>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string ModelCode { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual Brands Brands { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Leads> Leads { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LMS.Web.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DealerBrandMappings
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public int BrandId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Brands Brands { get; set; }
        public virtual Dealers Dealers { get; set; }
    }
}

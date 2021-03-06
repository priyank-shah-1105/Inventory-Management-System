//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class VendorMaster
    {
        public VendorMaster()
        {
            this.PurchaseMasters = new HashSet<PurchaseMaster>();
            this.Purchasepays = new HashSet<Purchasepay>();
        }
    
        public int VendorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual UsersMaster UsersMaster { get; set; }
        public virtual ICollection<PurchaseMaster> PurchaseMasters { get; set; }
        public virtual ICollection<Purchasepay> Purchasepays { get; set; }
    }
}

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
    
    public partial class PurchaseItemMaster
    {
        public int PurchaseItemId { get; set; }
        public int PurchaseId { get; set; }
        public int ItemId { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineAmount { get; set; }
    
        public virtual ItemMaster ItemMaster { get; set; }
        public virtual PurchaseMaster PurchaseMaster { get; set; }
    }
}

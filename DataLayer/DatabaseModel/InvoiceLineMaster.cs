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
    
    public partial class InvoiceLineMaster
    {
        public int InvoiceLineId { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    
        public virtual InvoiceMaster InvoiceMaster { get; set; }
        public virtual ItemMaster ItemMaster { get; set; }
    }
}

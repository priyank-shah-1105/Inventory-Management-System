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
    
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Type { get; set; }
        public string InvoiceNr { get; set; }
        public Nullable<int> StockId { get; set; }
        public int PartyId { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual InvoiceMaster InvoiceMaster { get; set; }
        public virtual PartyMaster PartyMaster { get; set; }
        public virtual StockMaster StockMaster { get; set; }
        public virtual UsersMaster UsersMaster { get; set; }
    }
}

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
    
    public partial class usp_GetStocks_Result
    {
        public System.DateTime EntryDate { get; set; }
        public string InvoiceNr { get; set; }
        public decimal Amount { get; set; }
        public int StockId { get; set; }
        public string UserName { get; set; }
        public string Branch { get; set; }
    }
}

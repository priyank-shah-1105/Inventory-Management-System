using System.Collections.Generic;
using System.Linq;
using DataLayer.DatabaseModel;

namespace DataLayer
{
    public static class CustomRepository
    {
        public static bool BranchExists(string name, int branchId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.BranchMasters.Count(item => item.Name == name && item.BranchId != branchId);
                return count > 0;
            }
        }

        public static bool UnitExists(string name, int unitId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.UnitMasters.Count(item => item.Name == name && item.UnitId != unitId);
                return count > 0;
            }
        }

        public static bool PartyExists(string name, int partyId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.PartyMasters.Count(item => item.Name == name && item.PartyId != partyId);
                return count > 0;
            }
        }


        public static bool VendorExists(string name, int vendorId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.VendorMasters.Count(item => item.Name == name && item.VendorId != vendorId);
                return count > 0;
            }
        }

        
        public static bool ItemExists(string name, int itemId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.ItemMasters.Count(item => item.Name == name && item.ItemId != itemId);
                return count > 0;
            }
        }

        public static bool UserExists(string name, int userId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.UsersMasters.Count(item => item.Username == name && item.UserId != userId);
                return count > 0;
            }
        }

        public static List<usp_GetStocks_Result> GetStocks(int? userId = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetStocks(userId).ToList();
            }
        }


        //get purchases

        public static List<usp_GetPurchases_Result> GetPurchases(int? userId = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetPurchases(userId).ToList();
            }
        }
        

        public static List<usp_GetCustomer_Result> GetCustomers(int? userId = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetCustomer(userId).ToList();
            }
        }

        public static List<usp_GetVendor_Result> GetVendors(int? userId = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetVendor(userId).ToList();
            }
        }

        public static List<usp_GetStocksLines_Result> GetStocksLines(int? stockId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetStocksLines(stockId).ToList();
            }
        }

        //get purchase lines
        public static List<usp_GetPurchaseLines_Result> GetPurchasesLines(int? purchaseId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetPurchaseLines(purchaseId).ToList();
            }
        }

        public static bool StockItemExists(int stockItemId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.StockItemMasters.Count(item => item.StockItemId == stockItemId);
                return count > 0;
            }
        }


        public static bool PurchaseItemExists(int purchaseItemId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.PurchaseItemMasters.Count(item => item.PurchaseItemId == purchaseItemId);
                return count > 0;
            }
        }


        public static bool InvoiceLineExists(int invoiceLineId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                int count = context.InvoiceLineMasters.Count(item => item.InvoiceLineId == invoiceLineId);
                return count > 0;
            }
        }


        public static List<usp_DashboardStocks_Result> GetDashboardStocks(int? userId = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_DashboardStocks(userId).ToList();
            }
        }

        public static List<usp_GetItems_Result> GetItems()
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetItems().ToList();
            }
        }

        public static List<usp_GetInvoices_Result> GetInvoices(int? userId = null)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetInvoices(userId).ToList();
            }
        }

        public static List<usp_GetInvoiceLines_Result> GetInvoiceLines(int invoiceId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.usp_GetInvoiceLines(invoiceId).ToList();
            }
        }

        public static List<rptInvoices_Result> rptInvoices(int invoiceId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.rptInvoices(invoiceId).ToList();
            }
        }

        public static List<rptInvoiceLines_Result> rptInvoiceLines(int invoiceId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.rptInvoiceLines(invoiceId).ToList();
            }
        }


        //rpt purchases
        public static List<rptPurchase_Result> rptPurchases(int purchaseId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.rptPurchase(purchaseId).ToList();
            }
        }

        public static List<rptPurchaseItems_Result> rptPurchaseItems(int purchaseId)
        {
            using (StockEntities context = BaseContext.GetDbContext())
            {
                return context.rptPurchaseItems(purchaseId).ToList();
            }
        }

    }

}

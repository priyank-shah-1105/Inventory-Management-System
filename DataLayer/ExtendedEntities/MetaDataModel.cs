using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommonLayer;

namespace DataLayer.DatabaseModel
{
    [MetadataType(typeof(BranchMasterMetadata))]
    public partial class BranchMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.BranchExists(Name, BranchId))
            {
                yield return new ValidationResult("Duplicate branch", fieldName);
            }
        }
    }

    [MetadataType(typeof(UnitMasterMetadata))]
    public partial class UnitMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.UnitExists(Name, UnitId))
            {
                yield return new ValidationResult("Duplicate unit", fieldName);
            }
        }
    }

    /*[MetadataType(typeof(PartyMasterMetadata))]
    public partial class PartyMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.PartyExists(Name, PartyId))
            {
                yield return new ValidationResult("Duplicate party", fieldName);
            }
        }
    }
    */
    /*[MetadataType(typeof(CustomerMetadata))]
    public partial class PartyMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.PartyExists(Name, PartyId))
            {
                yield return new ValidationResult("Duplicate party", fieldName);
            }
        }
    }*/

    [MetadataType(typeof(CustomerMetadata))]
    public partial class usp_GetCustomer_Result :IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.PartyExists(Name, PartyId))
            {
                yield return new ValidationResult("Duplicate party", fieldName);
            }
        }
    }

    [MetadataType(typeof(VendorMetadata))]
    public partial class usp_GetVendor_Result : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.VendorExists(Name, VendorId))
            {
                yield return new ValidationResult("Duplicate vendor", fieldName);
            }
        }
    }

    //vendor
    [MetadataType(typeof(VendorMasterMetadata))]
    public partial class VendorMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.VendorExists(Name, VendorId))
            {
                yield return new ValidationResult("Duplicate Vendor", fieldName);
            }
        }
    }



    [MetadataType(typeof(ItemMasterMetadata))]
    public partial class ItemMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.ItemExists(Name, ItemId))
            {
                yield return new ValidationResult("Duplicate item", fieldName);
            }
        }
    }

    [MetadataType(typeof(UsersMasterMetadata))]
    public partial class UsersMaster : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fieldName = new[] { "Name" };

            if (CustomRepository.UserExists(Username, UserId))
            {
                yield return new ValidationResult("Duplicate username", fieldName);
            }
        }
    }

    [MetadataType(typeof(StockItemMasterMetadata))]
    public partial class StockItemMaster
    {
        public MultipleItemStatus StatusName { get; set; }

    }


    //purchase item meta data

    [MetadataType(typeof(PurchaseItemMasterMetadata))]
    public partial class PurchaseItemMaster
    {
        public MultipleItemStatus StatusName { get; set; }

    }



    [MetadataType(typeof(StockMasterMetadata))]
    public partial class StockMaster
    {
        readonly GenericRepository<StockMaster> _repository = new GenericRepository<StockMaster>();
        readonly GenericRepository<StockItemMaster> _repositoryInvoiceLine = new GenericRepository<StockItemMaster>();

        public StockMaster(int? stockId = null)
        {
            if (stockId != null)
            {
                StockMaster stock = _repository.SelectByID(stockId);
                StockId = stock.StockId;
                EntryDate = stock.EntryDate;
                InvoiceNr = stock.InvoiceNr;
                Amount = stock.Amount;
                PartyId = stock.PartyId;
                CreatedBy = stock.CreatedBy;
                var invoiceList = _repositoryInvoiceLine.GetEntities().Where(p => p.StockId == stockId).ToList();


                StockItemMasters = new List<StockItemMaster>();

                foreach (StockItemMaster stockItem in invoiceList)
                {
                    StockItemMasters.Add(new StockItemMaster
                    {
                        StockItemId = stockItem.StockItemId,
                        StockId = stockItem.StockId,
                        ItemId = stockItem.ItemId,
                        UnitPrice = stockItem.UnitPrice,
                        LineAmount = stockItem.LineAmount,
                        StatusName = MultipleItemStatus.NoChange,
                    });
                }
            }

        }
    }



    //purchasemastermetadata
    [MetadataType(typeof(PurchaseMasterMetadata))]
    public partial class PurchaseMaster
    {
        readonly GenericRepository<PurchaseMaster> _repository1 = new GenericRepository<PurchaseMaster>();
        readonly GenericRepository<PurchaseItemMaster> _repositoryPurchaseLine = new GenericRepository<PurchaseItemMaster>();

        public PurchaseMaster(int? purchaseId = null)
        {
            if (purchaseId != null)
            {
                PurchaseMaster purchase = _repository1.SelectByID(purchaseId);
                PurchaseId = purchase.PurchaseId;
                EntryDate = purchase.EntryDate;
                PurchaseNr = purchase.PurchaseNr;
                Amount = purchase.Amount;
                VendorId = purchase.VendorId;
                CreatedBy = purchase.CreatedBy;
                var purchaseList = _repositoryPurchaseLine.GetEntities().Where(p => p.PurchaseId == purchaseId).ToList();


                PurchaseItemMasters = new List<PurchaseItemMaster>();

                foreach (PurchaseItemMaster purchaseItem in purchaseList)
                {
                    PurchaseItemMasters.Add(new PurchaseItemMaster
                    {
                        PurchaseItemId = purchaseItem.PurchaseItemId,
                        PurchaseId = purchaseItem.PurchaseId,
                        ItemId = purchaseItem.ItemId,
                        UnitPrice = purchaseItem.UnitPrice,
                        LineAmount = purchaseItem.LineAmount,
                        StatusName = MultipleItemStatus.NoChange,
                    });
                }
            }

        }
    }



    [MetadataType(typeof(InvoiceLineMetadata))]
    public partial class InvoiceLineMaster
    {
        public MultipleItemStatus StatusName { get; set; }

    }

    public partial class InvoiceMaster
    {
        readonly GenericRepository<InvoiceMaster> _repositoryInvoice = new GenericRepository<InvoiceMaster>();
        readonly GenericRepository<InvoiceLineMaster> _repositoryInvoiceLine = new GenericRepository<InvoiceLineMaster>();

        public InvoiceMaster(int? invoiceId = null)
        {
            if (invoiceId != null)
            {
                InvoiceMaster invoice = _repositoryInvoice.SelectByID(invoiceId);

                InvoiceId = invoice.InvoiceId;
                PartyId = invoice.PartyId;
                InvoiceDate = invoice.InvoiceDate;
                Amount = invoice.Amount;
                TotalAmount = invoice.TotalAmount;
                IsVat = invoice.IsVat;
                VatAmount = invoice.VatAmount;
                CreatedOn = invoice.CreatedOn;
                CreatedBy = invoice.CreatedBy;
                InvoiceNr = invoice.InvoiceNr;

                var invoiceList = _repositoryInvoiceLine.GetEntities().Where(p => p.InvoiceId == invoiceId).ToList();


                InvoiceLineMasters = new List<InvoiceLineMaster>();

                foreach (InvoiceLineMaster invoiceLine in invoiceList)
                {
                    InvoiceLineMasters.Add(new InvoiceLineMaster
                    {
                        InvoiceLineId = invoiceLine.InvoiceLineId,
                        InvoiceId = invoiceLine.InvoiceId,
                        ItemId = invoiceLine.ItemId,
                        Amount = invoiceLine.Amount,
                        Qty = invoiceLine.Qty,
                        Rate = invoiceLine.Rate,
                        StatusName = MultipleItemStatus.NoChange,
                    });
                }
            }
        }

    }




    public class BranchMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

    }

    public class UnitMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }

  /*  public class PartyMasterMetadata
    {
           [ScaffoldColumn(false)]
            public int PartyId { get; set; }
           
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [UIHint("Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [UIHint("ComboBox")]
        [ScaffoldColumn(false)]
        public int CreatedBy { get; set; }

        //[ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
    }

    */

    public class CustomerMetadata
    {
        [ScaffoldColumn(false)]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [UIHint("Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [UIHint("ComboBox")]
        [ScaffoldColumn(false)]
        public int CreatedBy { get; set; }

        //[ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

      //  [ScaffoldColumn(false)]
        //public string Phone { get; set; }

        [ScaffoldColumn(false)]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        public string Branch { get; set; }

    }


    public class VendorMetadata
    {
        [ScaffoldColumn(false)]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [UIHint("Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [UIHint("ComboBox")]
        [ScaffoldColumn(false)]
        public int CreatedBy { get; set; }

        //[ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        //  [ScaffoldColumn(false)]
        //public string Phone { get; set; }

        [ScaffoldColumn(false)]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        public string Branch { get; set; }

    }

    public class VendorMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [UIHint("Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [UIHint("ComboBox")]
        [ScaffoldColumn(false)]
        public int CreatedBy { get; set; }

        //[ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
    }


    
    public class ItemMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Order = 1)]
        public string Name { get; set; }

        [Display(Name = "Is active", Order = 3)]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [Display(Name = "Unit", Order = 2)]
        [UIHint("ComboBox")]
        public int? UnitId { get; set; }

    }

    public class UsersMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Display(Name = "Branch")]
        [UIHint("ComboBox")]
        [Required(ErrorMessage = "Branch is required")]
        public int BranchId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [UIHint("Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Display(Name = "Mobile #")]
        [Required(ErrorMessage = "Mobile # is required")]
        public string MobileNo { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Is active")]
        public bool IsActive { get; set; }
    }

    public class StockItemMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int StockItemId { get; set; }

        [ScaffoldColumn(false)]
        public int StockId { get; set; }

        [ScaffoldColumn(false)]
        public MultipleItemStatus StatusName { get; set; }

        [Required(ErrorMessage = "Item is required")]
        [Display(Name = "Item")]
        [UIHint("ComboBox")]
        public int? ItemId { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [UIHint("Currency")]
        [Display(Name = "Unit price")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Qty is required")]
        [UIHint("Integer")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        [ScaffoldColumn(false)]
        [UIHint("Currency")]
        [Display(Name = "Line amount")]
        public int LineAmount { get; set; }
    }


    //purchase master metadata


    public class PurchaseItemMasterMetadata
    {
        [ScaffoldColumn(false)]
        public int PurchaseItemId { get; set; }

        [ScaffoldColumn(false)]
        public int PurchaseId { get; set; }

        [ScaffoldColumn(false)]
        public MultipleItemStatus StatusName { get; set; }

        [Required(ErrorMessage = "Item is required")]
        [Display(Name = "Item")]
        [UIHint("ComboBox")]
        public int? ItemId { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [UIHint("Currency")]
        [Display(Name = "Unit price")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Qty is required")]
        [UIHint("Integer")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        [ScaffoldColumn(false)]
        [UIHint("Currency")]
        [Display(Name = "Line amount")]
        public int LineAmount { get; set; }
    }


    public class StockMasterMetadata
    {
        [Required(ErrorMessage = "Party is required")]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Entry date is required")]
        public DateTime EntryDate { get; set; }

        [Required(ErrorMessage = "Invoice # is required")]
        public string InvoiceNr { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

    }



    //purchasemastermetada
    public class PurchaseMasterMetadata
    {
        [Required(ErrorMessage = "Vendor is required")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Entry date is required")]
        public DateTime EntryDate { get; set; }

        [Required(ErrorMessage = "Invoice # is required")]
        public string PurchaseNr { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

    }





    /// <summary>
    /// Category Metadata class
    /// </summary>
    public class InvoiceMetadata
    {
        [Required(ErrorMessage = "Party is required")]
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Invoice date is required")]
        public DateTime InvoiceDate { get; set; }

    }

    /// <summary>
    /// Category Metadata class
    /// </summary>
    public class InvoiceLineMetadata
    {
        [ScaffoldColumn(false)]
        public int InvoiceLineId { get; set; }

        [ScaffoldColumn(false)]
        public int InvoiceId { get; set; }

        [ScaffoldColumn(false)]
        public MultipleItemStatus StatusName { get; set; }

        [Required(ErrorMessage = "Item is required")]
        [Display(Name = "Item")]
        [UIHint("GridForeignKey")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Rate is required")]
        [UIHint("Currency")]
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Qty is required")]
        [UIHint("Integer")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        [ScaffoldColumn(false)]
        [UIHint("Currency")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }


    }
}

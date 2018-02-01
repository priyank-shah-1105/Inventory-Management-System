using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Reporting.WebForms;

namespace BusinessLayer
{
    public class PurchaseController : BaseController
    {
        #region Globel Declaration
        readonly GenericRepository<PurchaseMaster> _repository = new GenericRepository<PurchaseMaster>();
        readonly GenericRepository<PurchaseItemMaster> _repositoryPurchaseLine = new GenericRepository<PurchaseItemMaster>();
        readonly GenericRepository<ItemMaster> _repositoryItem = new GenericRepository<ItemMaster>();
        readonly GenericRepository<Purchasepay> _repositoryPayment = new GenericRepository<Purchasepay>();
        private List<PurchaseItemMaster> _stockLines = new List<PurchaseItemMaster>();
        readonly GenericRepository<VendorMaster> _repositoryVendor = new GenericRepository<VendorMaster>();
        #endregion

        #region Purchase Methods
        public ActionResult Index()
        {
            return View(ViewName.Purchase);
        }

           public ActionResult PurchaseKendoRead(DataSourceRequest request)
           {
               if (SessionHelper.IsSuperAdmin)
               {
                   var jsonResultAdmin = Json(CustomRepository.GetPurchases().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                   jsonResultAdmin.MaxJsonLength = int.MaxValue;
                   return jsonResultAdmin;
               }

               var jsonResult = Json(CustomRepository.GetPurchases(SessionHelper.UserId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
               jsonResult.MaxJsonLength = int.MaxValue;
               return jsonResult;
           }

           public ActionResult PurchaseLinesKendoRead(int purchaseId, DataSourceRequest request)
           {
               var jsonResult = Json(CustomRepository.GetPurchasesLines(purchaseId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
               jsonResult.MaxJsonLength = int.MaxValue;
               return jsonResult;
           }

           public ActionResult Edit(int purchaseId)
           {
               PurchaseMaster purchaseModel = new PurchaseMaster(purchaseId);
               //Get RadiologicalList of same medicalPractionarHistoryId
               TempData["PurchaseItemMasters"] = purchaseModel.PurchaseItemMasters;
               TempData.Keep("PurchasetemMasters");
               ViewBag.ItemList = CustomRepository.GetItems();
               return View(ViewName.PurchaseManagement, purchaseModel);
           }

           public ActionResult Create()
           {
               ViewBag.ItemList = CustomRepository.GetItems();
               TempData["PurchaseItemMasters"] = new List<PurchaseItemMaster>();
               PurchaseMaster purchaseModel = new PurchaseMaster { CreatedBy = SessionHelper.UserId, EntryDate = DateTime.Now };
               return View(ViewName.PurchaseManagement, purchaseModel);
           }

           public ActionResult Save(PurchaseMaster purchaseModel)
           {

               if (!ModelState.IsValid)
               {
                   //Fail
                   ViewBag.openPopup = CommonHelper.ShowAlertMessage("Error");
                   ViewBag.ItemList = _repositoryItem.GetEntities().ToList();
                   return View(ViewName.PurchaseManagement, purchaseModel);
               }

               try
               {
                   using (StockEntities context = BaseContext.GetDbContext())
                   {
                       _stockLines = (List<PurchaseItemMaster>)TempData["PurchaseItemMasters"];

                       if (purchaseModel.PurchaseId == 0)
                       {
                           _repository.Insert(purchaseModel, context);

                           #region Purchasepay
                           Purchasepay objPayment = new Purchasepay
                           {
                               Amount = purchaseModel.Amount,
                               Date = purchaseModel.EntryDate,
                               Type = "Debit",
                               InvoiceNr = purchaseModel.PurchaseNr.ToString(),
                               PurchaseId = purchaseModel.PurchaseId,
                               VendorId = purchaseModel.VendorId,
                               Note = "Payment for purchase nr " + purchaseModel.PurchaseNr,
                               CreatedBy = SessionHelper.UserId,
                               CreatedOn = DateTime.Now

                           };

                           _repositoryPayment.Insert(objPayment, context);

                           #endregion
                       }
                       else
                       {
                           _repository.Update(purchaseModel, context);

                           #region Payment

                           Purchasepay objPayment = context.Purchasepays.FirstOrDefault(x => x.PurchaseId == purchaseModel.PurchaseId);

                           if (objPayment != null)
                           {
                               objPayment.Amount = purchaseModel.Amount;
                               objPayment.Date = purchaseModel.EntryDate;
                               objPayment.InvoiceNr = purchaseModel.PurchaseNr.ToString();
                               objPayment.VendorId = purchaseModel.VendorId;
                               objPayment.Note = "Payment for purchase nr " + purchaseModel.PurchaseNr;

                               _repositoryPayment.Update(objPayment, context);
                           }

                           #endregion
                       }


                       #region Purchase lines


                       if (_stockLines != null && _stockLines.Count > 0)
                       {
                           foreach (PurchaseItemMaster purchaseItem in _stockLines)
                           {
                               purchaseItem.PurchaseId = purchaseModel.PurchaseId;

                               switch (purchaseItem.StatusName)
                               {
                                   case MultipleItemStatus.Add:
                                       _repositoryPurchaseLine.Insert(purchaseItem, context);
                                       break;
                                   case MultipleItemStatus.Edit:

                                       if (CustomRepository.PurchaseItemExists(purchaseItem.PurchaseItemId))
                                       {
                                           _repositoryPurchaseLine.Update(purchaseItem, context);
                                       }
                                       else
                                       {
                                           _repositoryPurchaseLine.Insert(purchaseItem, context);
                                       }
                                    
                                       break;
                                   case MultipleItemStatus.Delete:
                                       _repositoryPurchaseLine.Delete(purchaseItem.PurchaseItemId, context);
                                       break;
                               }
                           }
                       }
                       #endregion

                    

                       // Save changes in database
                       context.SaveChanges();
                       TempData["PurchaseItemMasters"] = new List<PurchaseItemMaster>();
                   }

                   return RedirectToAction(ActionName.Index);
               }
               catch (Exception ex)
               {
                   purchaseModel.PurchaseItemMasters = (List<PurchaseItemMaster>)TempData["PurchaseItemMasters"];
                   ViewBag.ItemList = _repositoryItem.GetEntities().ToList();
                   ViewBag.openPopup = CommonHelper.ShowAlertMessage(CommonHelper.GetErrorMessage(ex, false));
                   return View(ViewName.PurchaseManagement, purchaseModel);
               }
           }

           public string KendoDestroy(int purchaseId)
           {
               if (purchaseId <= 0) return "Error";
               _repository.Delete(purchaseId);
               return string.Empty;
           }

           public JsonResult BindVendor()
           {
               if (SessionHelper.IsSuperAdmin)
               {
                   return Json(_repositoryVendor.GetEntities().Select(c => new { c.VendorId, c.Name }), JsonRequestBehavior.AllowGet);
               }

               return Json(_repositoryVendor.GetEntities().Where(x => x.CreatedBy == SessionHelper.UserId).Select(c => new { c.VendorId, c.Name }), JsonRequestBehavior.AllowGet);
           }

           public FileContentResult ShowPurchase(int purchaseId)
           {
               LocalReport localReport = new LocalReport
               {
                   ReportPath = Server.MapPath("~/ReportLibrary/Report1.rdlc")
               };

               List<rptPurchase_Result> resultSet = CustomRepository.rptPurchases(purchaseId);
               ReportDataSource PurchaseDataset = new ReportDataSource("DataSet1", resultSet);
               localReport.DataSources.Add(PurchaseDataset);

               ReportDataSource PurchaseDetailsDataset = new ReportDataSource("DataSet2", CustomRepository.rptPurchaseItems(purchaseId));
               localReport.DataSources.Add(PurchaseDetailsDataset);

               ReportParameter reportParameters = new ReportParameter("PurchaseId", purchaseId.ToString());
               localReport.SetParameters(reportParameters);

               Warning[] warnings;
               string[] streamids;
               string mimeType;
               string encoding;
               string filenameExtension;

               byte[] bytes = localReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
                   out streamids, out warnings);

               string purchaseName = string.Empty;

               rptPurchase_Result purchaseData = resultSet.FirstOrDefault();
               if (purchaseData != null)
               {
                   purchaseName = purchaseData.PurchaseNr + "-" + purchaseData.VendorName + ".pdf";
               }

               return File(bytes, "application/pdf", purchaseName);
           }


           #endregion

           #region Purchase Lines Method
           /// <summary>
           /// 
           /// </summary>
           /// <param name="request"></param>
           /// <returns></returns>
           public ActionResult PurchaseLinesRead([DataSourceRequest] DataSourceRequest request)
           {
               _stockLines = (List<PurchaseItemMaster>)TempData["PurchaseItemMasters"] ?? new List<PurchaseItemMaster>();
               TempData.Keep("PurchaseItemMasters");
               return Json(_stockLines.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
           }

           /// <summary>
           /// 
           /// </summary>
           /// <param name="request"></param>
           /// <param name="itemModel"></param>
           /// <returns></returns>
           [AcceptVerbs(HttpVerbs.Post)]
           public ActionResult PurchaseLinesCreate([DataSourceRequest] DataSourceRequest request, PurchaseItemMaster itemModel)
           {
               _stockLines = (List<PurchaseItemMaster>)TempData["PurchaseItemMasters"] ?? new List<PurchaseItemMaster>();
               itemModel.PurchaseItemId = _stockLines.Count + 1;
               itemModel.StatusName = MultipleItemStatus.Add;
               itemModel.LineAmount = itemModel.Qty * itemModel.UnitPrice;
               _stockLines.Add(itemModel);

               TempData["PurchaseItemMasters"] = _stockLines;
               TempData.Keep("PurchaseItemMasters");
               return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
           }

           /// <summary>
           /// PropertyItemUpdate
           /// </summary>
           /// <param name="request"></param>
           /// <param name="itemModel"></param>
           /// <returns></returns>
           [AcceptVerbs(HttpVerbs.Post)]
           public ActionResult PurchaseLinesUpdate([DataSourceRequest] DataSourceRequest request, PurchaseItemMaster itemModel)
           {
               _stockLines = (List<PurchaseItemMaster>)TempData["PurchaseItemMasters"] ?? new List<PurchaseItemMaster>();
               if (itemModel != null && ModelState.IsValid)
               {
                   _stockLines.Remove(_stockLines.FirstOrDefault(i => i.PurchaseItemId == itemModel.PurchaseItemId));
                   itemModel.StatusName = itemModel.PurchaseItemId == 0 ? MultipleItemStatus.Add : MultipleItemStatus.Edit;
                   itemModel.LineAmount = itemModel.Qty * itemModel.UnitPrice;
                   _stockLines.Add(itemModel);
               }
               TempData["PurchaseItemMasters"] = _stockLines;
               TempData.Keep("PurchaseItemMasters");
               return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
           }

           /// <summary>
           /// 
           /// </summary>
           /// <param name="request"></param>
           /// <param name="itemModel"> </param>
           /// <returns></returns>
           [AcceptVerbs(HttpVerbs.Post)]
           public ActionResult PurchaseLinesDestroy([DataSourceRequest] DataSourceRequest request, PurchaseItemMaster itemModel)
           {
               _stockLines = (List<PurchaseItemMaster>)TempData["PurchaseItemMasters"] ?? new List<PurchaseItemMaster>();
               var radiologicalExaminationModel = _stockLines.FirstOrDefault(u => u.PurchaseItemId == itemModel.PurchaseItemId);
               if (radiologicalExaminationModel != null)
                   if (radiologicalExaminationModel.PurchaseItemId == 0)
                       _stockLines.Remove(radiologicalExaminationModel);
                   else
                       radiologicalExaminationModel.StatusName = MultipleItemStatus.Delete;

               TempData["PurchaseItemMasters"] = _stockLines;
               TempData.Keep("PurchaseItemMasters");
               return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
           }
           #endregion
       }
    
}
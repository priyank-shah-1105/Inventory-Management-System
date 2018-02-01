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

namespace BusinessLayer
{
    public class StockController : BaseController
    {
        #region Globel Declaration
        readonly GenericRepository<StockMaster> _repository = new GenericRepository<StockMaster>();
        readonly GenericRepository<StockItemMaster> _repositoryInvoiceLine = new GenericRepository<StockItemMaster>();
        readonly GenericRepository<ItemMaster> _repositoryItem = new GenericRepository<ItemMaster>();
        readonly GenericRepository<Payment> _repositoryPayment = new GenericRepository<Payment>();
        private List<StockItemMaster> _stockLines = new List<StockItemMaster>();
        readonly GenericRepository<PartyMaster> _repositoryParty = new GenericRepository<PartyMaster>();
        #endregion

        #region Invoice Methods
        public ActionResult Index()
        {
            return View(ViewName.Stock);
        }

        public ActionResult InvoiceKendoRead(DataSourceRequest request)
        {
            if (SessionHelper.IsSuperAdmin)
            {
                var jsonResultAdmin = Json(CustomRepository.GetStocks().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResultAdmin.MaxJsonLength = int.MaxValue;
                return jsonResultAdmin;
            }

            var jsonResult = Json(CustomRepository.GetStocks(SessionHelper.UserId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InvoiceLinesKendoRead(int stockId, DataSourceRequest request)
        {
            var jsonResult = Json(CustomRepository.GetStocksLines(stockId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Edit(int stockId)
        {
            StockMaster stockModel = new StockMaster(stockId);
            //Get RadiologicalList of same medicalPractionarHistoryId
            TempData["StockItemMasters"] = stockModel.StockItemMasters;
            TempData.Keep("StockItemMasters");
            ViewBag.ItemList = CustomRepository.GetItems();
            return View(ViewName.StockManagement, stockModel);
        }

        public ActionResult Create()
        {
            ViewBag.ItemList = CustomRepository.GetItems();
            TempData["StockItemMasters"] = new List<StockItemMaster>();
            StockMaster stockModel = new StockMaster { CreatedBy = SessionHelper.UserId, EntryDate = DateTime.Now };
            return View(ViewName.StockManagement, stockModel);
        }

        public ActionResult Save(StockMaster stockModel)
        {

            if (!ModelState.IsValid)
            {
                //Fail
                ViewBag.openPopup = CommonHelper.ShowAlertMessage("Error");
                ViewBag.ItemList = _repositoryItem.GetEntities().ToList();
                return View(ViewName.StockManagement, stockModel);
            }

            try
            {
                using (StockEntities context = BaseContext.GetDbContext())
                {
                    _stockLines = (List<StockItemMaster>)TempData["StockItemMasters"];

                    if (stockModel.StockId == 0)
                    {
                        _repository.Insert(stockModel, context);

                        #region Payment
                        Payment objPayment = new Payment
                        {
                            Amount = stockModel.Amount,
                            Date = stockModel.EntryDate,
                            Type = "Debit",
                            InvoiceNr = stockModel.InvoiceNr,
                            StockId = stockModel.StockId,
                            PartyId = stockModel.PartyId,
                            Note = "Payment for invoice nr " + stockModel.InvoiceNr,
                            CreatedBy = SessionHelper.UserId,
                            CreatedOn = DateTime.Now

                        };

                        _repositoryPayment.Insert(objPayment, context);

                        #endregion
                    }
                    else
                    {
                        _repository.Update(stockModel, context);

                        #region Payment

                        Payment objPayment = context.Payment.FirstOrDefault(x => x.StockId == stockModel.StockId);

                        if (objPayment != null)
                        {
                            objPayment.Amount = stockModel.Amount;
                            objPayment.Date = stockModel.EntryDate;
                            objPayment.InvoiceNr = stockModel.InvoiceNr;
                            objPayment.PartyId = stockModel.PartyId;
                            objPayment.Note = "Payment for invoice nr " + stockModel.InvoiceNr;

                            _repositoryPayment.Update(objPayment, context);
                        }

                        #endregion
                    }


                    #region Stock lines


                    if (_stockLines != null && _stockLines.Count > 0)
                    {
                        foreach (StockItemMaster stockItem in _stockLines)
                        {
                            stockItem.StockId = stockModel.StockId;

                            switch (stockItem.StatusName)
                            {
                                case MultipleItemStatus.Add:
                                    _repositoryInvoiceLine.Insert(stockItem, context);
                                    break;
                                case MultipleItemStatus.Edit:

                                    if (CustomRepository.StockItemExists(stockItem.StockItemId))
                                    {
                                        _repositoryInvoiceLine.Update(stockItem, context);
                                    }
                                    else
                                    {
                                        _repositoryInvoiceLine.Insert(stockItem, context);
                                    }
                                    
                                    break;
                                case MultipleItemStatus.Delete:
                                    _repositoryInvoiceLine.Delete(stockItem.StockItemId, context);
                                    break;
                            }
                        }
                    }
                    #endregion

                    

                    // Save changes in database
                    context.SaveChanges();
                    TempData["StockItemMasters"] = new List<StockItemMaster>();
                }

                return RedirectToAction(ActionName.Index);
            }
            catch (Exception ex)
            {
                stockModel.StockItemMasters = (List<StockItemMaster>)TempData["StockItemMasters"];
                ViewBag.ItemList = _repositoryItem.GetEntities().ToList();
                ViewBag.openPopup = CommonHelper.ShowAlertMessage(CommonHelper.GetErrorMessage(ex, false));
                return View(ViewName.StockManagement, stockModel);
            }
        }

        public string KendoDestroy(int stockId)
        {
            if (stockId <= 0) return "Error";
            _repository.Delete(stockId);
            return string.Empty;
        }

        public JsonResult BindParty()
        {
            if (SessionHelper.IsSuperAdmin)
            {
                return Json(_repositoryParty.GetEntities().Select(c => new { c.PartyId, c.Name }), JsonRequestBehavior.AllowGet);
            }

            return Json(_repositoryParty.GetEntities().Where(x => x.CreatedBy == SessionHelper.UserId).Select(c => new { c.PartyId, c.Name }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Invoice Lines Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult InvoiceLinesRead([DataSourceRequest] DataSourceRequest request)
        {
            _stockLines = (List<StockItemMaster>)TempData["StockItemMasters"] ?? new List<StockItemMaster>();
            TempData.Keep("StockItemMasters");
            return Json(_stockLines.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InvoiceLinesCreate([DataSourceRequest] DataSourceRequest request, StockItemMaster itemModel)
        {
            _stockLines = (List<StockItemMaster>)TempData["StockItemMasters"] ?? new List<StockItemMaster>();
            itemModel.StockItemId = _stockLines.Count + 1;
            itemModel.StatusName = MultipleItemStatus.Add;
            itemModel.LineAmount = itemModel.Qty * itemModel.UnitPrice;
            _stockLines.Add(itemModel);

            TempData["StockItemMasters"] = _stockLines;
            TempData.Keep("StockItemMasters");
            return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// PropertyItemUpdate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InvoiceLinesUpdate([DataSourceRequest] DataSourceRequest request, StockItemMaster itemModel)
        {
            _stockLines = (List<StockItemMaster>)TempData["StockItemMasters"] ?? new List<StockItemMaster>();
            if (itemModel != null && ModelState.IsValid)
            {
                _stockLines.Remove(_stockLines.FirstOrDefault(i => i.StockItemId == itemModel.StockItemId));
                itemModel.StatusName = itemModel.StockItemId == 0 ? MultipleItemStatus.Add : MultipleItemStatus.Edit;
                itemModel.LineAmount = itemModel.Qty * itemModel.UnitPrice;
                _stockLines.Add(itemModel);
            }
            TempData["StockItemMasters"] = _stockLines;
            TempData.Keep("StockItemMasters");
            return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemModel"> </param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InvoiceLinesDestroy([DataSourceRequest] DataSourceRequest request, StockItemMaster itemModel)
        {
            _stockLines = (List<StockItemMaster>)TempData["StockItemMasters"] ?? new List<StockItemMaster>();
            var radiologicalExaminationModel = _stockLines.FirstOrDefault(u => u.StockItemId == itemModel.StockItemId);
            if (radiologicalExaminationModel != null)
                if (radiologicalExaminationModel.StockItemId == 0)
                    _stockLines.Remove(radiologicalExaminationModel);
                else
                    radiologicalExaminationModel.StatusName = MultipleItemStatus.Delete;

            TempData["StockItemMasters"] = _stockLines;
            TempData.Keep("StockItemMasters");
            return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}

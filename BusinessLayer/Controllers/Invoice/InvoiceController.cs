using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Reporting.WebForms;

namespace NationalTenders.BusinessLayer
{
    public class InvoiceController : Controller
    {
        #region Globel Declaration
        readonly GenericRepository<InvoiceMaster> _repository = new GenericRepository<InvoiceMaster>();
        readonly GenericRepository<InvoiceLineMaster> _repositoryInvoiceLine = new GenericRepository<InvoiceLineMaster>();
        readonly GenericRepository<PartyMaster> _repositoryParty = new GenericRepository<PartyMaster>();
        private List<InvoiceLineMaster> _InvoiceLineList = new List<InvoiceLineMaster>();
        readonly GenericRepository<Payment> _repositoryPayment = new GenericRepository<Payment>();
        #endregion

        #region Invoice Methods
        public ActionResult Index()
        {
            return View(ViewName.InvoiceList);
        }

        public ActionResult InvoiceKendoRead(DataSourceRequest request)
        {
            if (SessionHelper.IsSuperAdmin)
            {
                var jsonResultAdmin = Json(CustomRepository.GetInvoices().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResultAdmin.MaxJsonLength = int.MaxValue;
                return jsonResultAdmin;
            }

            var jsonResult = Json(CustomRepository.GetInvoices(SessionHelper.UserId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InvoiceLinesKendoRead(int invoiceId, DataSourceRequest request)
        {
            var jsonResult = Json(CustomRepository.GetInvoiceLines(invoiceId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Edit(int invoiceId)
        {
            InvoiceMaster invoiceModel = new InvoiceMaster(invoiceId);
            //Get RadiologicalList of same medicalPractionarHistoryId
            TempData["InvoiceLines"] = invoiceModel.InvoiceLineMasters;
            TempData.Keep("InvoiceLines");
            ViewBag.ItemList = CustomRepository.GetItems();
            return View(ViewName.InvoiceManagement, invoiceModel);
        }

        public ActionResult Create()
        {
            ViewBag.ItemList = CustomRepository.GetItems();
            TempData["InvoiceLines"] = new List<InvoiceLineMaster>();
            InvoiceMaster invoiceModel = new InvoiceMaster { CreatedBy = SessionHelper.UserId, CreatedOn = DateTime.Now, InvoiceDate = DateTime.Now };
            return View(ViewName.InvoiceManagement, invoiceModel);
        }

        public ActionResult Save(InvoiceMaster invoice)
        {

            if (!ModelState.IsValid)
            {
                //Fail
                ViewBag.ItemList = CustomRepository.GetItems();
                ViewBag.openPopup = CommonHelper.ShowAlertMessage("Error");
                return View(ViewName.InvoiceManagement, invoice);
            }

            try
            {
                using (StockEntities context = BaseContext.GetDbContext())
                {
                    _InvoiceLineList = (List<InvoiceLineMaster>)TempData["InvoiceLines"];

                    invoice.Amount = _InvoiceLineList.Where(x => x.StatusName != MultipleItemStatus.Delete).Sum(x => x.Amount);
                   
                    if (invoice.IsVat)
                    {
                        invoice.VatAmount = invoice.Amount * (decimal)0.126;
                    }

                    invoice.TotalAmount = invoice.Amount + invoice.VatAmount;

                    if (invoice.InvoiceId == 0)
                    {
                        _repository.Insert(invoice, context);

                        #region Payment
                        Payment objPayment = new Payment
                        {
                            Amount = invoice.Amount,
                            Date = invoice.InvoiceDate,
                            Type = "Credit",
                            InvoiceNr = invoice.InvoiceNr.ToString(),
                            InvoiceId = invoice.InvoiceId,
                            PartyId = invoice.PartyId,
                            Note = "Payment for invoice nr " + invoice.InvoiceNr,
                            CreatedBy = SessionHelper.UserId,
                            CreatedOn = DateTime.Now

                        };

                        _repositoryPayment.Insert(objPayment, context);

                        #endregion
                    }
                    else
                    {
                        _repository.Update(invoice, context);

                        #region Payment

                        Payment objPayment = context.Payment.FirstOrDefault(x => x.InvoiceId == invoice.InvoiceId);

                        if (objPayment != null)
                        {
                            objPayment.Amount = invoice.Amount;
                            objPayment.Date = invoice.InvoiceDate;
                            objPayment.InvoiceNr = invoice.InvoiceNr.ToString();
                            objPayment.PartyId = invoice.PartyId;
                            objPayment.Note = "Payment for invoice nr " + invoice.InvoiceNr;

                            _repositoryPayment.Update(objPayment, context);
                        }

                        #endregion
                    }


                    #region RadioLogical Examination


                    if (_InvoiceLineList != null && _InvoiceLineList.Count > 0)
                    {
                        foreach (var radioLogical in _InvoiceLineList)
                        {
                            radioLogical.InvoiceId = invoice.InvoiceId;

                            switch (radioLogical.StatusName)
                            {
                                case MultipleItemStatus.Add:
                                    _repositoryInvoiceLine.Insert(radioLogical, context);
                                    break;
                                case MultipleItemStatus.Edit:

                                    if (CustomRepository.InvoiceLineExists(radioLogical.InvoiceLineId))
                                    {
                                        _repositoryInvoiceLine.Update(radioLogical, context);
                                    }
                                    else
                                    {
                                        _repositoryInvoiceLine.Insert(radioLogical, context);
                                    }

                                    break;
                                case MultipleItemStatus.Delete:
                                    _repositoryInvoiceLine.Delete(radioLogical.InvoiceLineId, context);
                                    break;
                            }
                        }
                    }
                    #endregion

                    // Save changes in database
                    context.SaveChanges();
                    TempData["InvoiceLines"] = new List<InvoiceLineMaster>();
                }

                return RedirectToAction(ActionName.Index);
            }
            catch (Exception ex)
            {
                invoice.InvoiceLineMasters = (List<InvoiceLineMaster>)TempData["InvoiceLines"];
                ViewBag.ItemList = CustomRepository.GetItems();
                ViewBag.openPopup = CommonHelper.ShowAlertMessage(CommonHelper.GetErrorMessage(ex, false));
                return View(ViewName.InvoiceManagement, invoice);
            }
        }

        public string KendoDestroy(int invoiceId)
        {
            if (invoiceId <= 0) return "Error";
            _repository.Delete(invoiceId);
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


        public FileContentResult ShowInvoice(int invoiceId)
        {
            LocalReport localReport = new LocalReport
            {
                ReportPath = Server.MapPath("~/ReportLibrary/Report_Inv.rdlc")
            };

            List<rptInvoices_Result> resultSet = CustomRepository.rptInvoices(invoiceId);
            ReportDataSource InvoiceDataset = new ReportDataSource("DataSet1", resultSet);
            localReport.DataSources.Add(InvoiceDataset);

            ReportDataSource InvoiceDetailsDataset = new ReportDataSource("DataSet2", CustomRepository.rptInvoiceLines(invoiceId));
            localReport.DataSources.Add(InvoiceDetailsDataset);

            ReportParameter reportParameters = new ReportParameter("InvoiceId", invoiceId.ToString());
            localReport.SetParameters(reportParameters);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = localReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            string invoiceName = string.Empty;

            rptInvoices_Result invoiceData = resultSet.FirstOrDefault();
            if (invoiceData != null)
            {
                invoiceName = invoiceData.InvoiceNr + "-" + invoiceData.PartyName + ".pdf";
            }

            return File(bytes, "application/pdf", invoiceName);
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
            _InvoiceLineList = (List<InvoiceLineMaster>)TempData["InvoiceLines"] ?? new List<InvoiceLineMaster>();
            TempData.Keep("InvoiceLines");
            return Json(_InvoiceLineList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InvoiceLinesCreate([DataSourceRequest] DataSourceRequest request, InvoiceLineMaster itemModel)
        {
            _InvoiceLineList = (List<InvoiceLineMaster>)TempData["InvoiceLines"] ?? new List<InvoiceLineMaster>();
            itemModel.InvoiceLineId = _InvoiceLineList.Count + 1;
            itemModel.StatusName = MultipleItemStatus.Add;
            itemModel.Amount = itemModel.Qty * itemModel.Rate;
            _InvoiceLineList.Add(itemModel);

            TempData["InvoiceLines"] = _InvoiceLineList;
            TempData.Keep("InvoiceLines");
            return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// PropertyItemUpdate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InvoiceLinesUpdate([DataSourceRequest] DataSourceRequest request, InvoiceLineMaster itemModel)
        {
            _InvoiceLineList = (List<InvoiceLineMaster>)TempData["InvoiceLines"] ?? new List<InvoiceLineMaster>();
            if (itemModel != null && ModelState.IsValid)
            {
                _InvoiceLineList.Remove(_InvoiceLineList.FirstOrDefault(i => i.InvoiceLineId == itemModel.InvoiceLineId));
                itemModel.StatusName = itemModel.InvoiceLineId == 0 ? MultipleItemStatus.Add : MultipleItemStatus.Edit;
                itemModel.Amount = itemModel.Qty * itemModel.Rate;
                _InvoiceLineList.Add(itemModel);
            }
            TempData["InvoiceLines"] = _InvoiceLineList;
            TempData.Keep("InvoiceLines");
            return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemModel"> </param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InvoiceLinesDestroy([DataSourceRequest] DataSourceRequest request, InvoiceLineMaster itemModel)
        {
            _InvoiceLineList = (List<InvoiceLineMaster>)TempData["InvoiceLines"] ?? new List<InvoiceLineMaster>();
            var radiologicalExaminationModel = _InvoiceLineList.FirstOrDefault(u => u.InvoiceLineId == itemModel.InvoiceLineId);
            if (radiologicalExaminationModel != null)
                if (radiologicalExaminationModel.InvoiceLineId == 0)
                    _InvoiceLineList.Remove(radiologicalExaminationModel);
                else
                    radiologicalExaminationModel.StatusName = MultipleItemStatus.Delete;

            TempData["InvoiceLines"] = _InvoiceLineList;
            TempData.Keep("InvoiceLines");
            return Json(new[] { itemModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}

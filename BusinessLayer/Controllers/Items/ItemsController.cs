using System.Collections.Generic;
using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;

namespace BusinessLayer
{
    public class ItemsController : BaseController
    {
        readonly GenericRepository<ItemMaster> _repository = new GenericRepository<ItemMaster>();
        readonly GenericRepository<UnitMaster> _repositoryUnit = new GenericRepository<UnitMaster>();

        public ActionResult Index()
        {
            ViewBag.UnitList = _repositoryUnit.GetEntities().ToList();
            return View(ViewName.Items);
        }

        public ActionResult KendoRead(DataSourceRequest request)
        {
            return Json(_repository.GetEntities().OrderBy(x => x.Name).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KendoSave(DataSourceRequest request, ItemMaster model)
        {
            if (model == null || !ModelState.IsValid)
                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

            if (model.ItemId > 0)
            {
                _repository.Update(model);
            }
            else
            {
                _repository.Insert(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KendoDestroy(DataSourceRequest request, ItemMaster model)
        {
            string deleteMessage = _repository.Delete(model.ItemId);
            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
                ModelState.AddModelError(deleteMessage, deleteMessage);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool ChangeStatus(int id)
        {
            ItemMaster category = _repository.SelectByID(id);
            if (category == null) return false;
            category.IsActive = !category.IsActive;
            _repository.Update(category);
            return true;
        }
    }
}

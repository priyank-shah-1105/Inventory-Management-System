using System.Linq;
using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BusinessLayer
{
    public class UnitController : BaseController
    {
        readonly GenericRepository<UnitMaster> _repository = new GenericRepository<UnitMaster>();

        public ActionResult Index()
        {
            return View(ViewName.Unit);
        }

        public ActionResult KendoRead(DataSourceRequest request)
        {
            return Json(_repository.GetEntities().OrderBy(x => x.Name).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KendoSave(DataSourceRequest request, UnitMaster model)
        {
            if (model == null || !ModelState.IsValid)
                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

            if (model.UnitId > 0)
            {
                _repository.Update(model);
            }
            else
            {

                _repository.Insert(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KendoDestroy(DataSourceRequest request, BranchMaster model)
        {
            string deleteMessage = _repository.Delete(model.BranchId);
            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
                ModelState.AddModelError(deleteMessage, deleteMessage);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
    }
}

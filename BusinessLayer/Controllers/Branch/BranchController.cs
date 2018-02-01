using System.Linq;
using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BusinessLayer
{
    public class BranchController : BaseController
    {
        readonly GenericRepository<BranchMaster> _repository = new GenericRepository<BranchMaster>();

        public ActionResult Index()
        {
            return View(ViewName.Branch);
        }

        public ActionResult KendoRead(DataSourceRequest request)
        {
            return Json(_repository.GetEntities().OrderBy(x => x.Name).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KendoSave(DataSourceRequest request, BranchMaster model)
        {
            if (model == null || !ModelState.IsValid)
                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

            if (model.BranchId > 0)
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

        public bool ChangeStatus(int id)
        {
            BranchMaster category = _repository.SelectByID(id);
            if (category == null) return false;
            category.IsActive = !category.IsActive;
            _repository.Update(category);
            return true;
        }
    }
}

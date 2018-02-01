using System.Linq;
using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BusinessLayer
{
    public class VendorController : BaseController
    {
        readonly GenericRepository<VendorMaster> _repository = new GenericRepository<VendorMaster>();

        public ActionResult Index()
        {
            ViewBag.Users = SelectionList.UserList();
            return View(ViewName.Vendor);
        }

        public ActionResult KendoRead(DataSourceRequest request)
        {
            //return Json(_repository.GetEntities().OrderBy(x => x.Name).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            if (SessionHelper.IsSuperAdmin)
            {
                var jsonResultAdmin = Json(CustomRepository.GetVendors().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResultAdmin.MaxJsonLength = int.MaxValue;
                return jsonResultAdmin;
            }

            var jsonResult = Json(CustomRepository.GetVendors(SessionHelper.UserId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult KendoSave(DataSourceRequest request, VendorMaster model)
        {
           // if (model == null || !ModelState.IsValid)
            if (model == null)
                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

            if (model.VendorId > 0)
            {
                _repository.Update(model);
            }
            else
            {

                _repository.Insert(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KendoDestroy(DataSourceRequest request, VendorMaster model)
        {
            string deleteMessage = _repository.Delete(model.VendorId);
            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
                ModelState.AddModelError(deleteMessage, deleteMessage);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool ChangeStatus(int id)
        {
            VendorMaster category = _repository.SelectByID(id);
            if (category == null) return false;
            category.IsActive = !category.IsActive;
            _repository.Update(category);
            return true;
        }
    }
}

using System.Linq;
using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BusinessLayer
{
    public class PartyController : BaseController
    {
       // readonly GenericRepository<usp_GetCustomer_Result> _repository1 = new GenericRepository<usp_GetCustomer_Result>();
        readonly GenericRepository<PartyMaster> _repository = new GenericRepository<PartyMaster>();
        public ActionResult Index()
        {
            ViewBag.Users = SelectionList.UserList();
            return View(ViewName.Party);
        }

        public ActionResult KendoRead(DataSourceRequest request)
        {
            if (SessionHelper.IsSuperAdmin)
            {
                var jsonResultAdmin = Json(CustomRepository.GetCustomers().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResultAdmin.MaxJsonLength = int.MaxValue;
                return jsonResultAdmin;
            }

            var jsonResult = Json(CustomRepository.GetCustomers(SessionHelper.UserId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /*public ActionResult KendoRead(DataSourceRequest request)
        {
            return Json(_repository.GetEntities().OrderBy(x => x.Name).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            
        }
        */
        public ActionResult KendoSave(DataSourceRequest request,PartyMaster model)
        {
           // if (model == null || !ModelState.IsValid)
            if (model == null)
                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

            if (model.PartyId > 0)
            {
                _repository.Update(model);
            }
            else
            {

                _repository.Insert(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

           
        }

        public ActionResult KendoDestroy(DataSourceRequest request, PartyMaster model)
        {
            string deleteMessage = _repository.Delete(model.PartyId);
            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
                ModelState.AddModelError(deleteMessage, deleteMessage);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool ChangeStatus(int id)
        {
            PartyMaster category = _repository.SelectByID(id);
            if (category == null) return false;
            category.IsActive = !category.IsActive;
            _repository.Update(category);
            return true;
        }
    }
}

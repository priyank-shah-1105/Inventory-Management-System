using System.Web.Mvc;
using CommonLayer;
using DataLayer;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace BusinessLayer
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(ViewName.Home);
        }

        public ActionResult InvoiceKendoRead(DataSourceRequest request)
        {
            if (SessionHelper.IsSuperAdmin)
            {
                var jsonResultAdmin = Json(CustomRepository.GetDashboardStocks().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResultAdmin.MaxJsonLength = int.MaxValue;
                return jsonResultAdmin;
            }

            var jsonResult = Json(CustomRepository.GetDashboardStocks(SessionHelper.UserId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}

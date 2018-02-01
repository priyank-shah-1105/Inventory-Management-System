using System.Web.Mvc;
using CommonLayer;


namespace BusinessLayer
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View(ViewName.Error);
        }

    }
}

using System.Web.Mvc;
using CommonLayer;


namespace BusinessLayer
{
    public class NoPageFoundController : Controller
    {
        public ActionResult Index()
        {
            return View(ViewName.Error);
        }
    }
}

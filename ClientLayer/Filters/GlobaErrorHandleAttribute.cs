using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CommonLayer;

namespace Client
{
    public class GlobaErrorHandleAttribute : HandleErrorAttribute
    {
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                return;
            string errorMessege = CommonHelper.GetErrorMessage(filterContext.Exception);

            #region Log Exception
            //Log Exception
            string logDirectory = HttpContext.Current.Server.MapPath("~");
            logDirectory = Path.Combine(logDirectory, "Log");
            CommonHelper.WriteToLogText(errorMessege, "TLF MVC Architecture", logDirectory);
            #endregion

            #region Send Error Mail
            StringBuilder mailContent = CommonHelper.GetMailTemplate(MailTemplate.Error);
            if (mailContent != null)
            {
                mailContent.Replace("_error_", errorMessege);
                //CommonHelper.SendErrorMail(mailContent.ToString(), "", "", "", "");
            }
            #endregion

            #region Handle Error
            // if the request is AJAX return JSON else view.,
            if (IsAjax(filterContext))
            {
                /*
                 * IF any error occured in kendo create/update/delete then we do not to handle it with below code
                 * By default kenod ajax call post the form so we have checked RequestType != "POST"
                 */
                if (filterContext.HttpContext.Request.RequestType != "POST")
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = filterContext.Exception.Message,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                }
            }
            else
            {
                base.OnException(filterContext);
            }
            #endregion
        }
    }
}
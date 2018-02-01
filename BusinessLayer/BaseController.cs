using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;


namespace BusinessLayer
{
    public class BaseController : Controller
    {
        #region Overridden Methods
        /// <summary>
        /// On Result Executing
        /// </summary>
        /// <param name="filterContext">ResultExecutingContext</param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="filterContext">ActionExecutingContext</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Request.IsAuthenticated)
            {

                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                if (!controllerName.Contains("error") && !controllerName.Contains("nopagefound"))
                {
                    #region Handle Session Time Out
                    if (SessionHelper.UserId == 0)
                    {
                        SetSessionValues(filterContext);
                    }
                    #endregion
                }
                else
                {
                    RedirectToLoginPage(filterContext);
                }
            }
            else
            {
                RedirectToLoginPage(filterContext);
            }
        }

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="filterContext">AuthorizationContext</param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                var newTicket = FormsAuthentication.RenewTicketIfOld(ticket);
                if (newTicket != null && newTicket.Expiration != ticket.Expiration)
                {
                    var encryptedTicket = FormsAuthentication.Encrypt(newTicket);

                    cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Path = FormsAuthentication.FormsCookiePath };
                    Response.Cookies.Add(cookie);
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Redirect To Login Page
        /// </summary>
        /// <param name="filterContext">ActionExecutingContext</param>
        private void RedirectToLoginPage(ActionExecutingContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Action(ActionName.UserLogin, ControllerName.Login);
            if (loginUrl != null) filterContext.HttpContext.Response.Redirect(loginUrl, true);
        }


        /// <summary>
        /// Set Session Values
        /// </summary>
        /// <param name="filterContext"></param>
        private void SetSessionValues(ActionExecutingContext filterContext)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                RedirectToLoginPage(filterContext);
            }
            else
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null || authTicket.Expired)
                {
                    RedirectToLoginPage(filterContext);
                }
                else
                {

                    string userName = authTicket.Name;
                    GenericRepository<UsersMaster> repository = new GenericRepository<UsersMaster>();
                    UsersMaster logedInUser = repository.GetEntities().SingleOrDefault(u => u.Username == userName);

                    if (logedInUser == null)
                    {
                        RedirectToLoginPage(filterContext);
                    }
                    else if (logedInUser.IsActive == false)
                    {
                        RedirectToLoginPage(filterContext);
                    }
                    else
                    {
                        SessionHelper.UserId = logedInUser.UserId;
                        SessionHelper.WelcomeUser = logedInUser.Name;
                        SessionHelper.Username = logedInUser.Username;
                        SessionHelper.IsSuperAdmin = logedInUser.IsSuperAdmin;
                    }
                }
            }
        }
        #endregion
    }
}

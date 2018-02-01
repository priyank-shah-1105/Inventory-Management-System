using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using CommonLayer;
using DataLayer;
using DataLayer.DatabaseModel;


namespace BusinessLayer
{
    public class LoginController : Controller
    {
        private IFormsAuthenticationService FormsService { get; set; }

        readonly GenericRepository<UsersMaster> _repository = new GenericRepository<UsersMaster>();

        [OutputCache(Duration = int.MaxValue, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult UserLogin()
        {
            return View(ViewName.Login, new LoginIndexModel());
           
        }

        [HttpPost]
        public ActionResult ValidateUser(LoginIndexModel model)
        {
            UsersMaster logedInUser = _repository.GetEntities().SingleOrDefault(u => u.Username == model.UserName && u.Password == model.Password);

            if (logedInUser == null)
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return View(ViewName.Login, model);
            }

            if (logedInUser.IsActive == false)
            {
                ModelState.AddModelError("Username", string.Format("User '{0}' is no longer active.", logedInUser.Username));
                return View(ViewName.Login, model);
            }

            FormsService.SignIn(model.UserName, model.RememberMe);
            SessionHelper.UserId = logedInUser.UserId;
            SessionHelper.IsSuperAdmin = logedInUser.IsSuperAdmin;
            SessionHelper.WelcomeUser = logedInUser.Name;
            return RedirectToAction(ActionName.Index, ControllerName.Home);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult LogOut()
        {
            System.Web.HttpContext.Current.Session.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            FormsService.SignOut();

            return RedirectToAction(ActionName.UserLogin);
        }

        [OutputCache(Duration = int.MaxValue, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult ForgotPasswordIndex()
        {
            return View(ViewName.ForgotPasswordIndex, new LoginIndexModel());
        }

        [HttpPost]
        public ActionResult ForgotPassword(LoginIndexModel model)
        {
            try
            {
                UsersMaster userDetail = _repository.GetEntities().SingleOrDefault(u => u.Email == model.Email);

                if (userDetail == null)
                    return RedirectToAction(ActionName.ForgotPasswordIndex);

                StringBuilder mailContent = CommonHelper.GetMailTemplate(MailTemplate.ForgotPassword);
                if (mailContent != null)
                {
                    mailContent.Replace("_displayname_", userDetail.Name);
                    mailContent.Replace("_username_", userDetail.Username);
                    mailContent.Replace("_password_", Security.Decrypt(userDetail.Password));
                    //CommonHelper.SendMail(userDetail.Email, "Password Details", mailContent.ToString());
                }
            }
            catch (Exception)
            {
                return RedirectToAction(ActionName.UserLogin);
            }

            // set message after successful validation of password
            return RedirectToAction(ActionName.UserLogin);
        }

        #region Overriden Methods

        /// <summary>
        ///     Intialize Method
        /// </summary>
        /// <param name="requestContext">RequestContext</param>
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            base.Initialize(requestContext);
        }


        #endregion
    }

    #region Form Authentication

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(string.Empty, "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);

        void SignOut();
    }

    #endregion
}

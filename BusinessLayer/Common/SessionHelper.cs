using System.Web;


namespace BusinessLayer
{
    public class SessionHelper
    {
        public static SessionHelper Current
        {
            get
            {
                return (SessionHelper)HttpContext.Current.Session["SessionHelper"];
            }
            set
            {
                HttpContext.Current.Session["SessionHelper"] = value;
            }
        }

        public static int UserId
        {
            get
            {
                return HttpContext.Current.Session["UserId"] == null ? 0 : (int)HttpContext.Current.Session["UserId"];
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }

        public static int RoleId
        {
            get
            {
                return HttpContext.Current.Session["RoleId"] == null ? 0 : (int)HttpContext.Current.Session["RoleId"];
            }
            set
            {
                HttpContext.Current.Session["RoleId"] = value;
            }
        }

        public static string WelcomeUser
        {
            get
            {
                return HttpContext.Current.Session["WelcomeUser"] == null
                           ? "Guest"
                           : (string)HttpContext.Current.Session["WelcomeUser"];
            }
            set
            {
                HttpContext.Current.Session["WelcomeUser"] = value;
            }
        }

        public static string Username
        {
            get
            {
                return HttpContext.Current.Session["Username"] == null
                           ? "Guest"
                           : (string)HttpContext.Current.Session["Username"];
            }
            set
            {
                HttpContext.Current.Session["Email"] = value;
            }
        }
        
        public static bool IsSuperAdmin
        {
            get
            {
                return HttpContext.Current.Session["IsSuperAdmin"] != null && (bool)HttpContext.Current.Session["IsSuperAdmin"];
            }
            set
            {
                HttpContext.Current.Session["IsSuperAdmin"] = value;
            }
        }
    }
}

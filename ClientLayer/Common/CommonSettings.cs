using System.Collections.Generic;
using System.Web;

namespace Client
{
    public static class CommonSettings
    {
        public static string SiteRootPathUrl
        {
            get
            {
                string msRootUrl;
                if (HttpContext.Current.Request.ApplicationPath != null && string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath.Split('/')[1]))
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +
                                ":" +
                                HttpContext.Current.Request.Url.Port;
                }
                else
                {
                    msRootUrl = HttpContext.Current.Request.ApplicationPath;
                }

                return msRootUrl;
            }
        }

        public static readonly Dictionary<string, object> ActionCenterColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center" }, { "width", "60px" }, { "valign", "middle" } };
        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanEdit = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "80px" } };
        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanEditAssign = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "120px" } };
        public static readonly Dictionary<string, object> StatusColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "40px" } };
    }
}
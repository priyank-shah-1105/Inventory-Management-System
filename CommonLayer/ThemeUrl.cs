using System;
using System.Web;

namespace CommonLayer
{
    public class ThemeUrl
    {
        private string AbsoluteThemeUrl { get; set; }
        private string RelativeThemeUrl { get; set; }
        private static ThemeUrl _singleton;

        private ThemeUrl(string relativeThemeUrl)
        {
            AbsoluteThemeUrl = VirtualPathUtility.AppendTrailingSlash(Paths.GetVirtualUrl(relativeThemeUrl).TrimStart('/'));
            RelativeThemeUrl = VirtualPathUtility.AppendTrailingSlash(relativeThemeUrl);
        }

        public static ThemeUrl ImageUrl
        {
           get { return _singleton ?? (_singleton = new ThemeUrl("Content")); }                                    
        }

        public string GetClientSideFile(string fileName)
        {
            return "/" + AbsoluteThemeUrl + fileName.TrimStart('\\');
        }

        public string GetServerSideFile(string fileName)
        {
            return "~/" + RelativeThemeUrl + fileName.TrimStart('\\');
        }
    }

    public class Paths
    {
        public static string GetVirtualUrl(string relativeUrl)
        {
            //string i = "/priyank";
            //return VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath) + relativeUrl.TrimStart(new[] { '\\', '/' });
            return VirtualPathUtility.AppendTrailingSlash(relativeUrl.TrimStart(new[] { '\\', '/' }));
        }

        public static string GetUrlPath()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
        }
    }
}

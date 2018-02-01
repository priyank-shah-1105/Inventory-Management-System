using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Client
{
    public static class LinkExtensions
    {
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return htmlHelper.SecureActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary(), true);
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, int menuId)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = (string)htmlHelper.ViewContext.RouteData.Values["controller"];
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool isEmptyRequired)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = (string)htmlHelper.ViewContext.RouteData.Values["controller"];
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }
    }
}
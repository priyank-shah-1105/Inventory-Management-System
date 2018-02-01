using System.Web.Mvc;




namespace Client
{
    public static class HtmlExtension
    {
        private static MvcHtmlString CreateAddLink(HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            return htmlHelper.SecureActionLink("Create", actionName, controllerName);
        }

        private static MvcHtmlString CreateAddLink(HtmlHelper htmlHelper, string actionName, string controllerName, int menuId)
        {
            return htmlHelper.SecureActionLink("Create", actionName, controllerName, menuId);
        }

        private static MvcHtmlString GenerateMvcHtmlString(this HtmlHelper htmlHelper, string normalString)
        {
            return MvcHtmlString.Create(normalString);
        }

        public static MvcHtmlString LabelWithColon(this HtmlHelper html, string expression)
        {
            string strTemp = "<label for=\"" + expression.Replace(" ", "_") + "\" style=\"display:inline !important\">" + expression + "</label> : ";
            return GenerateMvcHtmlString(html, strTemp);
        }

        public static string SetStatusClientTemplate(this HtmlHelper helper, string isActive, string controllerName, string action, string parameter, string id, string gridId, string entityName)
        {

            string deactivteMessage = "Are you sure you want to deactivate this " + entityName;


            string activteMessage = "Are you sure you want to activate this " + entityName;

            var deactiveAttributes = " onclick='changeStatus(" + @"""" + controllerName + @"""" + ", " + @"""" +
                                    action + @"""" + ", " + @"""""" + ", "
                                    + @"""" + parameter + @"""" + ", " + @"""" + deactivteMessage + @"""" + ", " + id
                                    + ", " + @"""" + gridId + @"""" + @")'";

            var activeAttributes = " onclick='changeStatus(" + @"""" + controllerName + @"""" + ", " + @"""" +
                                   action + @"""" + ", " + @"""""" + ", "
                                   + @"""" + parameter + @"""" + ", " + @"""" + activteMessage + @"""" + ", " + id
                                   + ", " + @"""" + gridId + @"""" + @")'";


            return "# if (" + isActive + ")    {#" +
                   @"<a class='k-button' " + deactiveAttributes + @"><span class='k-icon k-i-tick'></span></a>" +
                   "#}else { #" +
                   @"<a class='k-button' " + activeAttributes + @"><span class='k-icon k-i-close'></span></a>"
                   + "#}#";
        }

        public static string SetStatusClientTemplateRequest(this HtmlHelper helper, string isActive, string controllerName, string action, string parameter, string id, string gridId, string entityName)
        {

            string deactivteMessage = "Are you sure you want to deactivate this " + entityName;


            string activteMessage = "Are you sure you want to activate this " + entityName;

            var deactiveAttributes = " onclick='changeStatus(" + @"""" + controllerName + @"""" + ", " + @"""" +
                                    action + @"""" + ", " + @"""""" + ", "
                                    + @"""" + parameter + @"""" + ", " + @"""" + deactivteMessage + @"""" + ", " + id
                                    + ", " + @"""" + gridId + @"""" + @")'";

            var activeAttributes = " onclick='changeStatus(" + @"""" + controllerName + @"""" + ", " + @"""" +
                                   action + @"""" + ", " + @"""""" + ", "
                                   + @"""" + parameter + @"""" + ", " + @"""" + activteMessage + @"""" + ", " + id
                                   + ", " + @"""" + gridId + @"""" + @")'";


            return "# if (" + isActive + ")    {#" +
                   @"<a class='k-button' " + deactiveAttributes + @"><span class='k-icon k-i-tick'></span></a>" +
                   "#}else { #" +
                   @"<a class='k-button' " + activeAttributes + @"><span class='k-icon k-i-tick'></span></a>"
                   + "#}#";
        }
        
    }
}
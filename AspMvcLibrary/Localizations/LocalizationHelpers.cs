using System;
using System.Globalization;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace AspMvcLibrary.Localizations
{
    public static class LocalizationHelpers
    {
        public static MvcHtmlString Resource(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var virtualPath = GetVirtualPath(htmlhelper);

            return MvcHtmlString.Create(GetResourceString(htmlhelper.ViewContext.HttpContext, expression, virtualPath, args));
        }

// ReSharper disable once UnusedMember.Global
        public static string Resource(this Controller controller, string expression, params object[] args)
        {
            return GetResourceString(controller.HttpContext, expression, "~/", args);
        }

        private static string GetResourceString(HttpContextBase httpContext, string expression, string virtualPath, object[] args)
        {
            var context = new ExpressionBuilderContext(virtualPath);
            var builder = new ResourceExpressionBuilder();
            var fields = (ResourceExpressionFields)builder.ParseExpression("LanguageResource, " + expression, typeof(string), context);

            string text;

            if (!string.IsNullOrEmpty(fields.ClassKey))
            {
                text = (string)httpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture);
            }
            else
            {
                text = (string)httpContext.GetLocalResourceObject(virtualPath, fields.ResourceKey, CultureInfo.CurrentUICulture);
            }

            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            return String.Format(text, args);
        }

        private static string GetVirtualPath(HtmlHelper htmlhelper)
        {
            var view = htmlhelper.ViewContext.View as BuildManagerCompiledView;

            if (view != null)
            {
                return view.ViewPath;
            }

            return null;
        }
    }
}

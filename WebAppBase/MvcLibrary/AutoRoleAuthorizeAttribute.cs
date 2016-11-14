using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebAppBase.MvcLibrary
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AutoRoleAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public ApplicationRoleMenuManager RoleManager(HttpContextBase httpContext)
        {
            return httpContext.GetOwinContext().Get<ApplicationRoleMenuManager>();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {         
            var controller= filterContext.Controller as Controller;
            if (!controller.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                return;
            }
            var rmMng = RoleManager(filterContext.HttpContext);
            var roles= rmMng.GetAllowRolesByControllNameActionName(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
            if (roles == null || roles.Count == 0)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                return;
            }
            var allow=false;
            foreach (var r in roles)
            {
                if (controller.User.IsInRole(r.Name))
                {
                    allow = true;
                    break;
                }
            }
            if (!allow)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
    }
}
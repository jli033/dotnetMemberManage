using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBase.Configs;
using WebAppBase.Models.SystemMenus;
using WebAppBase.Sessions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace WebAppBase.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }


        public HomeController(ApplicationUserManager userManager,
            ApplicationRoleMenuManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleMenuManager _roleManager;
        public ApplicationRoleMenuManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleMenuManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index()
        {
            var at = "OrganizationAdmin";            
            var menus=RoleManager.GetMenusByRoleName(at);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            try
            {
                if (Request.UserHostAddress != null)
                {
                    var ex = (Exception)HttpContext.Application[Request.UserHostAddress];

                    if (ex != null)
                    {
                        var loginInfoSession = SessionLoginInfo.GetInstance(Session);

                        SystemLogManager.GetInstance().SetSystemErrorLog(SystemConfig.SystemTitle, loginInfoSession.OrganizationID, loginInfoSession.LoginID, loginInfoSession.UserName, ex.Message, ex.StackTrace);
                    }
                }
            }
            catch (Exception exx)
            {
                Debug.WriteLine(exx.Message);
            }

            return View("Error");
        }

        public ActionResult Http404()
        {
            return View("Error");
        }

        public ActionResult TimeoutRedirect(string OrganizationID)
        {
            ViewBag.OrganizationID = string.Format("{0}", OrganizationID);
            return View();
        }

        public ActionResult RolloutRedirect(string OrganizationID)
        {
            ViewBag.OrganizationID = string.Format("{0}", OrganizationID);
            return View();
        }

        public ActionResult Live()
        {
            return new EmptyResult();
        }

        public ActionResult GetMenuHTML()
        {
            var at = "OrganizationAdmin";
            if (User.Identity.IsAuthenticated)
            {
                var roles = UserManager.GetRoles(User.Identity.GetUserId());
                if (roles.Count > 0)
                {
                    at = roles[0];
                }
            }
            var menus =  RoleManager.GetMenusByRoleName(at);
            var menuHtml = SystemMenuListModel.GetInstance().GetHtml(menus, Url);
            return Content(menuHtml);
        }

    }
}
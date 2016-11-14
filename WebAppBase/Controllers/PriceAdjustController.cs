using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppBase.MvcLibrary;

namespace WebAppBase.Controllers
{
    [AutoRoleAuthorize]
    public class PriceAdjustController : Controller
    {
        // GET: PriceAdjust
        public ActionResult Index()
        {
            return new ContentResult() { Content = "Index" };
        }

        public ActionResult Edit()
        {
            return new ContentResult() { Content = "Edit" };
        }

        public ActionResult Test()
        {
            return new ContentResult() { Content = "Test" };
        }

    }
}
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AspMvcLibrary.Attributes;
using WebAppBase.Configs;
using WebAppBase.Models.Base;
using WebAppBase.Models.Sorts;

namespace WebAppBase.Controllers
{
    [Authorize]
    public class SortsController : Controller
    {
        public ActionResult SortList()
        {
            var sortTargetModel = Session[SessionKeyConfig.SortTargetModel] as SortTargetModel;

            if (sortTargetModel == null)
            {
                throw new Exception("指定されたSession変数はISortModel型にCast出来ません。");
            }

            var model = new SortListModel(sortTargetModel);
            return View(model);
        }

        [HttpPost]
        [ActionName("SortList")]
        [SubmitCommand("back")]
        public ActionResult Back()
        {
            var model = _getTarget();
            return RedirectToAction(model.RedirectAction, model.RedirectController,model.RouteValues);
        }

        [HttpPost]
        public ActionResult Update(List<SortModel> items)
        {
            var model = _getTarget();
            if (items != null && items.Count > 0)
            {
                SortListModel.GetInstance().Update(model, items);
            }
            return Json(new { redirectTo = Url.Action(model.RedirectAction, model.RedirectController, model.RouteValues) });
        }

        private SortTargetModel _getTarget()
        {
            var model = Session[SessionKeyConfig.SortTargetModel] as SortTargetModel;

            if (model == null)
            {
                throw new Exception("SortTargetが指定されていません。");
            }

            return model;
        }
    }
}

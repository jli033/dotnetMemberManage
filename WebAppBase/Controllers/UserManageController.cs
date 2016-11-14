using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAppBase.Models;

namespace WebAppBase.Controllers
{
    public class UserManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: UserManager
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: UserManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user=db.Users.Find(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            if (userEditViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userEditViewModel);
        }

        // GET: UserManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserManager/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email")] UserEditViewModel userEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userEditViewModel.Email, Email = userEditViewModel.Email };
                var result = await UserManager.CreateAsync(user,"123456");
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(userEditViewModel);
        }

        // GET: UserManager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            
            if (userEditViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userEditViewModel);
        }

        // POST: UserManager/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email")] UserEditViewModel userEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(userEditViewModel.Id);
                user.Email = userEditViewModel.Email;
                user.UserName = userEditViewModel.Email;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userEditViewModel);
        }

        // GET: UserManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            var userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            
            if (userEditViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userEditViewModel);
        }

        // POST: UserManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

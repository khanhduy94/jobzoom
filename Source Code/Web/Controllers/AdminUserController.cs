using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{ 
    public class AdminUserController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /AdminUser/

        public ViewResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /AdminUser/Details/5

        public ViewResult Details(string id)
        {
            User user = db.Users.Single(u => u.UserId == id);
            return View(user);
        }

        //
        // GET: /AdminUser/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /AdminUser/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.AddObject(user);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(user);
        }
        
        //
        // GET: /AdminUser/Edit/5
 
        public ActionResult Edit(string id)
        {
            User user = db.Users.Single(u => u.UserId == id);
            return View(user);
        }

        //
        // POST: /AdminUser/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /AdminUser/Delete/5
 
        public ActionResult Delete(string id)
        {
            User user = db.Users.Single(u => u.UserId == id);
            return View(user);
        }

        //
        // POST: /AdminUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            User user = db.Users.Single(u => u.UserId == id);
            db.Users.DeleteObject(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
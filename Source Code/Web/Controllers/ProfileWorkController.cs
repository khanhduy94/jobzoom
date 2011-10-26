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
    public class ProfileWorkController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /ProfileWork/

        public ViewResult Index()
        {
            var profile_work = db.Profile_Work.Include("User");
            return View(profile_work.ToList());
        }

        //
        // GET: /ProfileWork/Details/5

        public ViewResult Details(Guid id)
        {
            Profile_Work profile_work = db.Profile_Work.Single(p => p.ProfileWorkId == id);
            return View(profile_work);
        }

        //
        // GET: /ProfileWork/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        } 

        //
        // POST: /ProfileWork/Create

        [HttpPost]
        public ActionResult Create(Profile_Work profile_work)
        {
            if (ModelState.IsValid)
            {
                profile_work.ProfileWorkId = Guid.NewGuid();
                db.Profile_Work.AddObject(profile_work);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_work.UserId);
            return View(profile_work);
        }
        
        //
        // GET: /ProfileWork/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Profile_Work profile_work = db.Profile_Work.Single(p => p.ProfileWorkId == id);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_work.UserId);
            return View(profile_work);
        }

        //
        // POST: /ProfileWork/Edit/5

        [HttpPost]
        public ActionResult Edit(Profile_Work profile_work)
        {
            if (ModelState.IsValid)
            {
                db.Profile_Work.Attach(profile_work);
                db.ObjectStateManager.ChangeObjectState(profile_work, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_work.UserId);
            return View(profile_work);
        }

        //
        // GET: /ProfileWork/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Profile_Work profile_work = db.Profile_Work.Single(p => p.ProfileWorkId == id);
            return View(profile_work);
        }

        //
        // POST: /ProfileWork/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Profile_Work profile_work = db.Profile_Work.Single(p => p.ProfileWorkId == id);
            db.Profile_Work.DeleteObject(profile_work);
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
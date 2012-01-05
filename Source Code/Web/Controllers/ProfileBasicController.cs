using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;
using JobZoom.Core.FlexibleAttributes;

namespace JobZoom.Web.Controllers
{ 
    public class ProfileBasicController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /ProfileBasic/

        public ViewResult Index()
        {
            var profile_basic = db.Profile_Basic.Include("User");
            return View(profile_basic.ToList());
        }

        //
        // GET: /ProfileBasic/Details/5

        public ViewResult Details(Guid id)
        {
            Profile_Basic profile_basic = db.Profile_Basic.Single(p => p.ProfileBasicId == id);
            return View(profile_basic);
        }

        //
        // GET: /ProfileBasic/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        } 

        //
        // POST: /ProfileBasic/Create

        [HttpPost]
        public ActionResult Create(Profile_Basic profile_basic)
        {
            if (ModelState.IsValid)
            {
                profile_basic.ProfileBasicId = Guid.NewGuid();
                //db.Profile_Basic.AddObject(profile_basic);
                db.Profile_Basic.Add(profile_basic);
                db.SaveChanges();

                //Mapping
                Guid objectId = profile_basic.ProfileBasicId;
                TagAttributeMappingManager mapping = new TagAttributeMappingManager();
                mapping.AddRootAttribute(objectId, profile_basic.UserId, "JobSeekerProfile");
                mapping.AddSecondLevelAttribute(objectId, "Basic", "JobSeekerProfile");
                mapping.AddThirdLevelAttribute(profile_basic, objectId, "JobSeekerProfile", "Profile.Basic");

                return RedirectToAction("Index");  
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_basic.UserId);
            return View(profile_basic);
        }
        
        //
        // GET: /ProfileBasic/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Profile_Basic profile_basic = db.Profile_Basic.Single(p => p.ProfileBasicId == id);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_basic.UserId);
            return View(profile_basic);
        }

        //
        // POST: /ProfileBasic/Edit/5

        [HttpPost]
        public ActionResult Edit(Profile_Basic profile_basic)
        {
            if (ModelState.IsValid)
            {
                db.Profile_Basic.Attach(profile_basic);
                //db.ObjectStateManager.ChangeObjectState(profile_basic, EntityState.Modified);
                db.Entry(profile_basic).State = EntityState.Modified;
                db.SaveChanges();

                //Mapping
                Guid objectId = profile_basic.ProfileBasicId;
                TagAttributeMappingManager mapping = new TagAttributeMappingManager();
                mapping.AddRootAttribute(objectId, profile_basic.UserId, "JobSeekerProfile");
                mapping.AddSecondLevelAttribute(objectId, "Basic", "JobSeekerProfile");
                mapping.AddThirdLevelAttribute(profile_basic, objectId, "JobSeekerProfile", "Profile.Basic");

                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_basic.UserId);
            return View(profile_basic);
        }

        //
        // GET: /ProfileBasic/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Profile_Basic profile_basic = db.Profile_Basic.Single(p => p.ProfileBasicId == id);
            return View(profile_basic);
        }

        //
        // POST: /ProfileBasic/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Profile_Basic profile_basic = db.Profile_Basic.Single(p => p.ProfileBasicId == id);
            //db.Profile_Basic.DeleteObject(profile_basic);
            db.Profile_Basic.Remove(profile_basic);
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
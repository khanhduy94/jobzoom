using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;
using System.Data;
using JobZoom.Web.Models;

namespace JobZoom.Web.Controllers
{
    public class ProfileEditController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        private string userId;
        //
        // GET: /ProfileEdit/

        [Authorize]
        public ActionResult Index()
        {   
            return View();
        }

        //
        // GET: /Profile/Edit/Basic
        [Authorize]
        [HttpGet]
        public ActionResult Basic()
        {
            string userId = User.Identity.Name;
            Profile_Basic profile_basic = db.Profile_Basic.Single(p => p.UserId == userId);
            ViewBag.City = new SelectList(new City { }.GetCities, profile_basic.City);
            ViewBag.Country = new SelectList(new Countries { }.GetCountries, profile_basic.Country);
            ViewBag.Gender = new SelectList(new Genders { }.GetGenders, profile_basic.Gender);
            return View(profile_basic);
        }

        // POST: /Profile/Edit/Basic
        [Authorize]
        [HttpPost]
        public ActionResult Basic(Profile_Basic profile_basic)
        {
            if (ModelState.IsValid)
            {
                db.Profile_Basic.Attach(profile_basic);
                db.ObjectStateManager.ChangeObjectState(profile_basic, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Basic");
            }
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", profile_basic.UserId);
            return View(profile_basic);
        }

        [Authorize]
        public ActionResult Education()
        {
            return View();
        }

        [Authorize]
        public ActionResult Work()
        {            
            return View();
        }

        [Authorize]
        public ActionResult ListWorks()
        {
            userId = User.Identity.Name;
            var profile_work = db.Profile_Work.Where(x => x.UserId == userId);

            return PartialView("ListProfileWorkView", profile_work);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateWork()
        {
            Profile_Work profile_work = new Profile_Work();
            return PartialView("EditProfileWorkView", profile_work);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditWork(Guid id)
        {
            Profile_Work profile_work = db.Profile_Work.FirstOrDefault(x => x.ProfileWorkId == id);

            return PartialView("EditProfileWorkView", profile_work);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveWork(Profile_Work profile_work)
        {
            userId = User.Identity.Name;
            profile_work.UserId = userId;

            if (ModelState.IsValid)
            {
                if (profile_work.ProfileWorkId == Guid.Empty)
                {                    
                    profile_work.ProfileWorkId = Guid.NewGuid();
                    db.Profile_Work.AddObject(profile_work);
                    db.SaveChanges();
                }
                else
                {
                    db.Profile_Work.Attach(profile_work);
                    db.ObjectStateManager.ChangeObjectState(profile_work, EntityState.Modified);
                    db.SaveChanges();
                }

                             
            }                        
            var listProfileWork = db.Profile_Work.Where(x => x.UserId == userId);
            return PartialView("ListProfileWorkView", listProfileWork);
        }
    }
}

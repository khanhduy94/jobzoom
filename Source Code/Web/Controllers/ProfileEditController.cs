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
        [OutputCache(Duration = 0)]
        public ActionResult Education()
        {
            return View();
        }

        [OutputCache(Duration = 0)]
        public ActionResult ListEducations()
        {
            userId = User.Identity.Name;
            var profile_educations = db.Profile_Education.Where(x => x.UserId == userId);

            return PartialView("ListProfileEducationView", profile_educations);
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult CreateEducation()
        {
            Profile_Education profile_educations = new Profile_Education();
            return PartialView("EditProfileEducationView", profile_educations);
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult EditEducation(Guid id)
        {
            Profile_Education profile_educations = db.Profile_Education.FirstOrDefault(x => x.ProfileEducationId == id);
            return PartialView("EditProfileEducationView", profile_educations);
        }

        [HttpPost]
        public ActionResult SaveEducation(Profile_Education profile_education)
        {
            userId = User.Identity.Name;
            profile_education.UserId = userId;

            if (ModelState.IsValid)
            {
                if (profile_education.ProfileEducationId == Guid.Empty)
                {
                    profile_education.ProfileEducationId = Guid.NewGuid();
                    db.Profile_Education.AddObject(profile_education);
                    db.SaveChanges();
                }
                else
                {
                    db.Profile_Education.Attach(profile_education);
                    db.ObjectStateManager.ChangeObjectState(profile_education, EntityState.Modified);
                    db.SaveChanges();
                }
            }
            var listProfileEducation = db.Profile_Education.Where(x => x.UserId == userId);
            return PartialView("ListProfileEducationView", listProfileEducation);
        }

        public ActionResult DeleteWork()
        {
            return View();
        }


        [Authorize]
        [OutputCache(Duration = 0)]
        public ActionResult Work()
        {            
            return View();
        }        

        [Authorize]
        [OutputCache(Duration = 0)]
        public ActionResult ListWorks()
        {
            userId = User.Identity.Name;
            var profile_work = db.Profile_Work.Where(x => x.UserId == userId);

            return PartialView("ListProfileWorkView", profile_work);
        }

        [Authorize]
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult CreateWork()
        {
            Profile_Work profile_work = new Profile_Work();
            return PartialView("EditProfileWorkView", profile_work);
        }

        [Authorize]
        [HttpGet]
        [OutputCache(Duration = 0)]
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

        public ActionResult DeleteWork(Guid id)
        {
            Profile_Work profile_work = db.Profile_Work.Single(p => p.ProfileWorkId == id);
            return View(profile_work);
        }        
    }
}

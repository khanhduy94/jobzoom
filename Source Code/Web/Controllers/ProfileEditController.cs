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
        //
        // GET: /ProfileEdit/

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


        public ActionResult Education()
        {
            return View();
        }

        public ActionResult Work()
        {
            string userId = User.Identity.Name;
            var profile_work = db.Profile_Work.Where(x => x.UserId == userId);
            ProfileWorkViewModel profile_Work_ViewModel = new ProfileWorkViewModel();
            profile_Work_ViewModel.ProfileWorks = profile_work;
            return View(profile_Work_ViewModel);
        }
    }
}

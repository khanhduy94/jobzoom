using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.JobZoomServiceReference;
using JobZoom.Business.Entites;
using JobZoom.Web.Models;

namespace JobZoom.Web.Controllers
{
    public class ProfileEditController : Controller
    {
        public ActionResult Index()
        {
            //Display personal information - education - skill - experience
            string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
            return View(new ProfileViewModel(userID));
        }

        //
        // GET 
        public ActionResult Basic()
        {
            string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
            JobZoomServiceClient client = new JobZoomServiceClient();
            var profile = client.GetJobseeker(userID);

            var genders = new List<SelectListItem>();
            genders.Add(new SelectListItem { Text = "Male", Value = "M" });
            genders.Add(new SelectListItem { Text = "Female", Value = "F" });
            ViewData["Genders"] = genders;         

            ViewData["ListCity"] = client.GetAllCities().ToSelectList(c => c.ID, c => c.Name);
            ViewData["ListCountry"] = client.GetAllCountries().ToSelectList(c => c.ID, c => c.Name);                        
            return View(profile);
        }

        [HttpPost]
        public ActionResult Basic(Jobseeker model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                JobZoomServiceClient client = new JobZoomServiceClient();
                client.SavePersonalInfo(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET
        public ActionResult AddEducation()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult AddEducation(Jobseeker_Experience model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
                JobZoomServiceClient client = new JobZoomServiceClient();
                model.ID = System.Guid.NewGuid().ToString();
                model.UserID = userID;               
                client.AddEducation(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UpdateExperience(string id)
        {            
            JobZoomServiceClient client = new JobZoomServiceClient();
            var education = client.GetExperience(id);
            return View(education);
        }

        [HttpPost]
        public ActionResult UpdateEducation(Jobseeker_Experience model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                JobZoomServiceClient client = new JobZoomServiceClient();
                client.SaveExperience(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteEducation(string id)
        {
            try
            {
                JobZoomServiceClient client = new JobZoomServiceClient();
                client.DeleteExperience(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        //GET
        public ActionResult WorkExperience()
        {
            return View();
        }
    }
}

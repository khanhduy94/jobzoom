using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.JobZoomServiceReference;
using JobZoom.Business.Entites;

namespace JobZoom.Web.Controllers
{
    public class ProfileEditController : Controller
    {
        public ActionResult Index()
        {
            //Display personal information - education - skill - experience
            string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
            JobZoomServiceClient client = new JobZoomServiceClient();
            var profile = client.GetJobseeker(userID);
            return View(profile);
        }

        //
        // GET 
        public ActionResult Basic()
        {
            string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
            JobZoomServiceClient client = new JobZoomServiceClient();
            var profile = client.GetJobseeker(userID);
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
                return RedirectToAction("Edit");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET
        public ActionResult Education()
        {
            return View();
        }

        //
        //GET
        public ActionResult WorkExperience()
        {
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.JobZoomServiceReference;
using JobZoom.Business.Entites;

namespace JobZoom.Web.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/View/5

        public ActionResult View(string id)
        {
            return View();
        }
     
        //
        // GET: /Profile/Edit/5
 
        public ActionResult Edit()
        {
            //Display personal information - education - skill - experience
            string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
            JobZoomServiceClient client = new JobZoomServiceClient();
            var profile = client.GetJobseeker(userID);            
            return View(profile);
        }

        public ActionResult EditPersonalInfo()
        {
            string userID = "1a7d990c-d629-4e8b-bb60-c5f22e498e5d";
            JobZoomServiceClient client = new JobZoomServiceClient();
            var profile = client.GetJobseeker(userID);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Swimming",
                Value = "1"
            });
            ViewBag.item = items;

            return View(profile);
        }

        //
        // POST: /Profile/Edit/5
        
        [HttpPost]
        public ActionResult EditPersonalInfo(Jobseeker model)
        {
            if (ModelState.IsValid)
            {
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
            return View(model);
        }        
    }
}

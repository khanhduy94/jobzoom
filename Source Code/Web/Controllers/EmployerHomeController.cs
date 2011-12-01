using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class EmployerHomeController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        private string userId;
        //
        // GET: /EmployerHome/

        public ActionResult Home()
        {
            userId = User.Identity.Name;
            return View(db.Job_Posting.Where(x=>x.UserId == userId).ToList());
        }
    }
}
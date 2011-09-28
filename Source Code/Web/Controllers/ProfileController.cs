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

        public ActionResult Details(string id)
        {
            return View();
        }
            
    }
}

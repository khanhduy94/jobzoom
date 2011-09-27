using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobZoom.Web.Controllers
{
    public class ProfileEditController : Controller
    {
        //
        // GET 
        public ActionResult Basic()
        {
            return View();
        }
        
        //
        // GET
        public ActionResult EducationExperience()
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

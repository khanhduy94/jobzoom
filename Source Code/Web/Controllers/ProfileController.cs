using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Core;

namespace JobZoom.Web.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        
        public ActionResult Home()
        {
            return View();
        }

    }
}

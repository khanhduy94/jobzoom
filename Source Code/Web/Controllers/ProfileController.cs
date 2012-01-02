using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class ProfileController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /Profile/

        [Authorize]
        public ActionResult Home()
        {
            Profile_Basic profile_basic = db.Profile_Basic.Where(x=>x.UserId == User.Identity.Name).SingleOrDefault();
            return View(profile_basic);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobZoom.Web.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        [Authorize]
        public ActionResult Home()
        {
            return View();
        }

    }
}

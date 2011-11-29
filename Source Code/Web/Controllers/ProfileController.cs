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
            string profileID = "b121bddf-7a43-4de4-9048-7ff1c90ead9b";
            string jobID = "da00077a-adfe-4a98-bd62-b9e1e6491928";

            JobZoomMatching match = new JobZoomMatching(profileID, jobID);
            match.Process();
            ViewBag.RequirePoint = match.RequirePoint;
            ViewBag.Point = match.MatchingPoint;

            return View();
        }

    }
}

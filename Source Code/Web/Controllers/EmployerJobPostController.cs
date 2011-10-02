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
    public class EmployerJobPostController : Controller
    {
        //
        // GET: /EmployerJobPost/

        public ActionResult Basic()
        {
            //JobZoomServiceClient client = new JobZoomServiceClient();
            //ViewData["ListCountry"] = client.GetAllCountries().ToSelectList(c => c.ID, c => c.Name);
            //ViewData["ListCity"] = client.GetAllCities().ToSelectList(c => c.ID, c => c.Name);
            return View();
        }

        //
        // GET: /EmployerJobPost/Details/5

        public ActionResult Details()
        {
            return View();
        }

        //
        // GET: /EmployerJobPost/Create

        public ActionResult CandidatesRecommendation()
        {
            return View();
        }     
    }
}

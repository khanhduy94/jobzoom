using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;
using JobZoom.Web.Models;

namespace JobZoom.Web.Controllers
{
    public class EmployerHomeController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        private string userId;
        //
        // GET: /EmployerHome/

        [Authorize]
        public ActionResult Home()
        {
            return View();
        }               
    }
}
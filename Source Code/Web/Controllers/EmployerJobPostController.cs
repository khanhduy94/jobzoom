using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class EmployerJobPostController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        //
        // GET: /EmployerJobPost/

        public ActionResult Basic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Basic(Job_Posting job_posting)
        {
            if (ModelState.IsValid)
            {
                job_posting.JobPostingId = Guid.NewGuid();
                //db.Job_Posting.AddObject(job_posting);
                db.Job_Posting.Add(job_posting);
                db.SaveChanges();
                return RedirectToAction("Basic", "EmployerEditJob", new { id = job_posting.JobPostingId});
            }

            return View(job_posting);
        }

        

        public ActionResult Education()
        {
            return View();
        }

        public ActionResult Work()
        {
            return View();
        }

        
    }
}

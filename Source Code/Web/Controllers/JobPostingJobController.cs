using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{ 
    public class JobPostingJobController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /JobPostingJob/

        public ViewResult Index()
        {
            return View(db.Job_Posting.ToList());
        }

        //
        // GET: /JobPostingJob/Details/5

        public ViewResult Details(string id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            return View(job_posting);
        }

        //
        // GET: /JobPostingJob/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /JobPostingJob/Create

        [HttpPost]
        public ActionResult Create(Job_Posting job_posting)
        {
            if (ModelState.IsValid)
            {
                job_posting.JobPostingId = Guid.NewGuid().ToString();
                db.Job_Posting.AddObject(job_posting);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(job_posting);
        }
        
        //
        // GET: /JobPostingJob/Edit/5

        public ActionResult Edit(string id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            return View(job_posting);
        }

        //
        // POST: /JobPostingJob/Edit/5

        [HttpPost]
        public ActionResult Edit(Job_Posting job_posting)
        {
            if (ModelState.IsValid)
            {
                db.Job_Posting.Attach(job_posting);
                db.ObjectStateManager.ChangeObjectState(job_posting, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job_posting);
        }

        //
        // GET: /JobPostingJob/Delete/5

        public ActionResult Delete(string id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            return View(job_posting);
        }

        //
        // POST: /JobPostingJob/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            db.Job_Posting.DeleteObject(job_posting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
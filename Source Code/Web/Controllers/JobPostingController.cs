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
    public class JobPostingController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /JobPosting/

        public ViewResult Index()
        {
            return View(db.Job_Posting.ToList());
        }

        //
        // GET: /JobPosting/Details/5

        public ViewResult Details(Guid id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            return View(job_posting);
        }

        //
        // GET: /JobPosting/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /JobPosting/Create

        [HttpPost]
        public ActionResult Create(Job_Posting job_posting)
        {
            if (ModelState.IsValid)
            {
                job_posting.JobPostingId = Guid.NewGuid();
                db.Job_Posting.AddObject(job_posting);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(job_posting);
        }
        
        //
        // GET: /JobPosting/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            return View(job_posting);
        }

        //
        // POST: /JobPosting/Edit/5

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
        // GET: /JobPosting/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(j => j.JobPostingId == id);
            return View(job_posting);
        }

        //
        // POST: /JobPosting/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
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
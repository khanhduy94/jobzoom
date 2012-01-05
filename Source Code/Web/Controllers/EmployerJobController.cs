﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.Models;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class EmployerJobController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        private string userId;
        //
        // GET: /EmployerJob/

        public ActionResult Index()
        {
            userId = User.Identity.Name;
            return View(db.Job_Posting.Where(x => x.UserId == userId).ToList());
        }

        [Authorize]
        public ActionResult Details(Guid id)
        {
            JobViewModel model = new JobViewModel();
            model.Job_Posting = db.Job_Posting.FirstOrDefault(x => x.JobPostingId == id);
            Guid[] arrayProfile = db.Job_Approval.Where(x => x.JobPostingId == id && x.IsApplied == true).Select(x => x.ProfileID).ToArray();
            model.ApplyList = db.Profile_Basic.Where(x => arrayProfile.Contains(x.ProfileBasicId));
            return View(model);
        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var job = db.Job_Posting.FirstOrDefault(x => x.JobPostingId == id);
            //db.DeleteObject(job);
            db.Job_Posting.Remove(job);  
            db.SaveChanges();
            return RedirectToAction("Home");
        }

        [Authorize]
        public ActionResult Candidate(Guid id)
        {
            return View();
        }
    }
}
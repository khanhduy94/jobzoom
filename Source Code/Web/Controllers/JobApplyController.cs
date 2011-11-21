using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.Models;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class JobApplyController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();        
        //
        // GET: /JobApply/

        [OutputCache(Duration = 0)]
        public ActionResult ApplyConfirmView(Guid id)
        {
            return PartialView("ApplyConfirmView");
        }

        [HttpPost]
        public ActionResult Apply(JobApplyConfirmViewModel model)
        {
            model.UserId = User.Identity.Name;
            Guid profileId = db.Profile_Basic.SingleOrDefault(x=>x.UserId == User.Identity.Name).ProfileBasicId;
            Job_Approval job_approval = new Job_Approval
            {
                JobApprovalId = Guid.NewGuid(),
                ProfileID = profileId,
                JobPostingId = model.JobId,
                IsApplied = true,
                IsApproved = false
            };

            //db.Job_Approval.AddObject(job_approval);
            //db.SaveChanges();

            return RedirectToAction("AppliedJobs");
        }

        public ActionResult AppliedJobs()
        {
            return View();
        }
    }
}

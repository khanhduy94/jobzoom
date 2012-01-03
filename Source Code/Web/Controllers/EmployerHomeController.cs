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
            db.DeleteObject(job);
            db.SaveChanges();
            return RedirectToAction("Home");
        }
    }
}
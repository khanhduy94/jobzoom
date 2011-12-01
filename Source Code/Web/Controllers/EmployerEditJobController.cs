using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Business.Entities;
using System.Data;

namespace JobZoom.Web.Controllers
{
    public class EmployerEditJobController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        private string userId;
        private Guid jobPosingId;
        //
        // GET: /EmployerEditJob/

        public ActionResult Basic(Guid id)
        {
            Job_Posting job_posting = db.Job_Posting.Single(x => x.JobPostingId == id);
            return View(job_posting);
        }

        [HttpPost]
        public ActionResult Basic(Job_Posting job_posting)
        {
            if (ModelState.IsValid)
            {
                db.Job_Posting.Attach(job_posting);
                db.ObjectStateManager.ChangeObjectState(job_posting, EntityState.Modified);
                db.SaveChanges();                
            }
            return View(job_posting);
        }

        public ActionResult Education(Guid id)
        {
            var test = Url.RequestContext.RouteData.Values["id"];
            List<Job_EducationExpRequirement> model = db.Job_EducationExpRequirement.Where(x => x.JobPostingId == id).ToList();            
            return View(model);
        }

        public ActionResult Work(Guid id)
        {
            jobPosingId = id;
            return View();
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult ListWorks(Guid id)
        {
            var listWorkExp = db.Job_WorkExpRequirement.Where(x => x.JobPostingId == id);
            return PartialView("ListWorkExpView", listWorkExp);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult CreateWork()
        {
            Job_WorkExpRequirement workExp = new Job_WorkExpRequirement();
            return PartialView("EditWorkExpView", workExp);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult EditWork(Guid id)
        {
            Job_WorkExpRequirement workExp = db.Job_WorkExpRequirement.FirstOrDefault(x=>x.JobWorkExpRequirementId == id);
            return PartialView("EditWorkExpView", workExp);

        }
        
        [HttpPost]
        public ActionResult SaveWork(Job_WorkExpRequirement workExp)
        {
            if(ModelState.IsValid)
            {
                if(workExp.JobWorkExpRequirementId == Guid.Empty)
                {
                    workExp.JobWorkExpRequirementId = Guid.NewGuid();                    
                    db.Job_WorkExpRequirement.AddObject(workExp);

                    //Tag tag = new Tag{ 
                    //    ID= Guid.NewGuid(), 
                    //    ObjectID = workExp.JobWorkExpRequirementId, 
                    //    TableName="Job.WorkExpRequirement", 
                    //    TagName = workExp.JobAttributeValue,
                        
                    //    IsUpToDate = true,
                    
                    db.SaveChanges();
                }
                else
                {
                    db.Job_WorkExpRequirement.Attach(workExp);
                    db.ObjectStateManager.ChangeObjectState(workExp, EntityState.Modified);
                    db.SaveChanges();
                }
            }
            var listWorkExp = db.Job_WorkExpRequirement.Where(x => x.JobPostingId == workExp.JobPostingId);
            return PartialView("ListWorkExpView", listWorkExp);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult CreateEducation()
        {
            Job_EducationExpRequirement educationExp = new Job_EducationExpRequirement();
            return PartialView("EditEducationExpView", educationExp);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult EditEducation(Guid id)
        {
            Job_EducationExpRequirement educationExp = db.Job_EducationExpRequirement.FirstOrDefault(x => x.JobEducationExpRequirementId == id);
            return PartialView("EditEducationExpView", educationExp);
        }

        [HttpPost]
        public ActionResult SaveEducation(Job_EducationExpRequirement educationExp)
        {
            if (ModelState.IsValid)
            {
                if (educationExp.JobEducationExpRequirementId == Guid.Empty)
                {
                    educationExp.JobEducationExpRequirementId = Guid.NewGuid();
                    db.Job_EducationExpRequirement.AddObject(educationExp);
                    db.SaveChanges();
                }
                else
                {
                    db.Job_EducationExpRequirement.Attach(educationExp);
                    db.ObjectStateManager.ChangeObjectState(educationExp, EntityState.Modified);
                    db.SaveChanges();
                }
            }
            var listEducationExp = db.Job_EducationExpRequirement.Where(x => x.JobPostingId == educationExp.JobPostingId);
            return PartialView("ListEducationExpView", listEducationExp);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult ListEducations(Guid id)
        {
            List<Job_EducationExpRequirement> listEducationExp = db.Job_EducationExpRequirement.Where(x => x.JobPostingId == id).ToList();
            return PartialView("ListEducationExpView", listEducationExp);
        }


    }
}

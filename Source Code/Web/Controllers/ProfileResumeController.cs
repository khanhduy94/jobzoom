using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.Models;
using JobZoom.Core.Framework.DataMining;
using JobZoom.Business.Entities;
using JobZoom.Core.Taxonomy;
using JobZoom.Core.Entities;
using JobZoom.Core.DataMining;

namespace JobZoom.Web.Controllers
{
    public class ProfileResumeController : Controller
    {
        //
        // GET: /ProfileResume/

        public ActionResult Index()
        {
            ResumeViewModel model = new ResumeViewModel();
            JobZoomEntities entities = new JobZoomEntities();
            JobZoomCoreEntities coreEntities = new JobZoomCoreEntities();
            Guid[] JobIDs = entities.Job_Approval.Where(a => a.UserId == User.Identity.Name && a.IsApproved == false).Select(a => a.JobPostingId).ToArray();
            model.JobTitles = entities.Job_Posting.Where(j => JobIDs.Contains(j.JobPostingId)).Select(j => j.JobTitle).ToArray();
            model.AppliedJobs = entities.Job_Posting.Where(j => JobIDs.Contains(j.JobPostingId)).ToList();
            model.TagAttributeDics = new List<TagAttributeDic>();
            foreach (var jobId in JobIDs)
            {
                List<TagAttribute> listAttributes = coreEntities.TagAttributes.Where(x => x.ObjectId == jobId && x.ObjectDeepLevel == 3).ToList();

                TagAttributeDic tagAttributeDic = new TagAttributeDic
                {
                    JobId = jobId,
                    TagAttributes = listAttributes
                };
                model.TagAttributeDics.Add(tagAttributeDic);
            }
            

            List<DecisionTreeAnalysisResult> results = new List<DecisionTreeAnalysisResult>();
            

            foreach (string JobTitle in model.JobTitles)
            {
                try
                {
                    results = DecisionTreeAnalysis.getAnalysisResults(DecisionTreeAnalysis.convertJobTitleNameToModelName(JobTitle, model.Prefix), model.TypedAttributes, model.ExceptAttributes, model.CompareType, model.Probability);
                    if (results != null)
                    {
                        RequiredTagName RequiredTag = new RequiredTagName(JobTitle, results.First().getNodeCaptionsWithValue(true).Select(nc => nc.Name).ToList());
                        model.RequiredTagNames.Add(RequiredTag);
                    }
                }
                catch (Exception) { }
            }
            return View(model);
        }

        [OutputCache(Duration = 0)]
        [HttpGet]
        public ActionResult CreateDecisionTreeGraph(Guid id)
        {
            JobZoomEntities db = new JobZoomEntities();
            string jobTitle = db.Job_Posting.Where(x => x.JobPostingId == id).Single().JobTitle;

            string modelName = DecisionTreeAnalysis.convertJobTitleNameToModelName(jobTitle, "PF");
            
            JobGraphViewModel model = new JobGraphViewModel(modelName);
            return PartialView("DecisionTreeGraphView", model);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.Models;
using JobZoom.Core.Framework.DataMining;
using JobZoom.Business.Entities;

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
            Guid[] JobIDs = entities.Job_Approval.Where(a => a.UserId == User.Identity.Name && a.IsApproved == false).Select(a => a.JobPostingId).ToArray();
            model.JobTitles = entities.Job_Posting.Where(j => JobIDs.Contains(j.JobPostingId)).Select(j => j.JobTitle).ToArray();

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

        //
        // GET: /ProfileResume/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ProfileResume/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ProfileResume/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /ProfileResume/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ProfileResume/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProfileResume/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ProfileResume/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

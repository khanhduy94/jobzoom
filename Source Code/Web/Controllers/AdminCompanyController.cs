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
    public class AdminCompanyController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();

        //
        // GET: /AdminCompany/

        public ViewResult Index()
        {
            return View(db.Companies.ToList());
        }

        //
        // GET: /AdminCompany/Details/5

        public ViewResult Details(Guid id)
        {
            Company company = db.Companies.Single(c => c.CompanyId == id);
            return View(company);
        }

        //
        // GET: /AdminCompany/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /AdminCompany/Create

        [HttpPost]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                company.CompanyId = Guid.NewGuid();
                //db.Companies.AddObject(company);
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(company);
        }
        
        //
        // GET: /AdminCompany/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Company company = db.Companies.Single(c => c.CompanyId == id);
            return View(company);
        }

        //
        // POST: /AdminCompany/Edit/5

        [HttpPost]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Attach(company);
                //db.ObjectStateManager.ChangeObjectState(company, EntityState.Modified);
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        //
        // GET: /AdminCompany/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Company company = db.Companies.Single(c => c.CompanyId == id);
            return View(company);
        }

        //
        // POST: /AdminCompany/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            Company company = db.Companies.Single(c => c.CompanyId == id);
            //db.Companies.DeleteObject(company);
            db.Companies.Remove(company);
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
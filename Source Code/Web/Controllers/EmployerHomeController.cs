using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobZoom.Web.Controllers
{
    public class EmployerHomeController : Controller
    {
        //
        // GET: /EmployerHome/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /EmployerHome/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /EmployerHome/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /EmployerHome/Create

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
        // GET: /EmployerHome/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /EmployerHome/Edit/5

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
        // GET: /EmployerHome/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /EmployerHome/Delete/5

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

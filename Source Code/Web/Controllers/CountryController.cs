﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Web.JobZoomServiceReference;

namespace JobZoom.Web.Controllers
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/

        public ActionResult Index()
        {
            JobZoomServiceClient dataContext = new JobZoomServiceClient();
            
            List<Country> allCountries = dataContext.GetAllCountries();
            return View();
        }

        //
        // GET: /Country/Details/5

        public ActionResult Details(string id)
        {
            //using (var context = new JobZoomEntities())
            //{
            //    var countries = context.Countries.Where(c => c.ID == id).First();
            //    return View(countries);
            //}      
            return View();
        }

        //
        // GET: /Country/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Country/Create

        [HttpPost]
        public ActionResult Create(Country country)
        {
            //try
            //{
            //    country.ID = System.Guid.NewGuid().ToString();
            //    using (var context = new JobZoomEntities())
            //    {
            //        context.Countries.AddObject(country);
            //        context.SaveChanges();
            //    }
            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
            return null;
        }
        
        //
        // GET: /Country/Edit/5
 
        public ActionResult Edit(string id)
        {
            //using (var context = new JobZoomEntities())
            //{
            //    var country = context.Countries.Where(c => c.ID == id).First();
            //    return View(country);
            //}            

            return View();
        }

        //
        // POST: /Country/Edit/5

        [HttpPost]
        public ActionResult Edit(Country collection)
        {
            //try
            //{
            //    using (var context = new JobZoomEntities())
            //    {
            //        var country = context.Countries.Where(c => c.ID == collection.ID).First();
            //        UpdateModel(country);
            //        context.SaveChanges();
            //    }
 
            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}

            return View();
        }

        //
        // GET: /Country/Delete/5
 
        public ActionResult Delete(string id)
        {
            //using (var context = new JobZoomEntities())
            //{
            //    var country = context.Countries.Where(c => c.ID == id).First();
            //    return View(country);
            //}      

            return View();
        }

        //
        // POST: /Country/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            //try
            //{
            //    using (var context = new JobZoomEntities())
            //    {
            //        var country = context.Countries.Where(c => c.ID == id).First();
            //        context.Countries.DeleteObject(country);
            //        context.SaveChanges();
            //    }
 
            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}

            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobZoom.Core;
using JobZoom.Web.Models;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class ProfileController : Controller
    {
        private JobZoomEntities db = new JobZoomEntities();
        private string userId;

        //
        // GET: /Profile/
        [CustomAuthorize(Roles="Jobseeker")]        
        public ActionResult Home()
        {
            userId = User.Identity.Name;
            ProfileViewModel model = new ProfileViewModel();
            model.Basic = db.Profile_Basic.FirstOrDefault(p => p.UserId == userId);
            model.Educations = db.Profile_Education.Where(p => p.UserId == userId);
            model.Works = db.Profile_Work.Where(p => p.UserId == userId);
            model.Skills = db.AttributeTags.Where(p => p.ObjectId == model.Basic.ProfileBasicId.ToString() && p.ParentId == "Skill/Qualifications");
            return View(model);
        }
    }
}

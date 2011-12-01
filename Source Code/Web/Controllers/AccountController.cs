using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using JobZoom.Web.Models;
using JobZoom.Business.Entities;

namespace JobZoom.Web.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            ViewBag.City = new SelectList(new City { }.GetCities);
            ViewBag.Country = new SelectList(new Countries { }.GetCountries);
            ViewBag.Gender = new SelectList(new Genders { }.GetGenders);
            ViewBag.MaritalStatus = new SelectList(new MaritalStatus { }.GetMaritalStatus);
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    // Save basic information
                    JobZoomEntities db = new JobZoomEntities();
                    Profile_Basic profile_basic = new Profile_Basic();
                    profile_basic.ProfileBasicId = Guid.NewGuid().ToString();
                    profile_basic.UserId = model.UserName;
                    profile_basic.FirstName = model.FirstName;
                    profile_basic.LastName = model.LastName;
                    profile_basic.Birthdate = model.Birthdate;
                    profile_basic.Gender = model.Gender;
                    profile_basic.MaritalStatus = model.MaritalStatus;
                    profile_basic.Country = model.Country;
                    profile_basic.City = model.City;
                    db.Profile_Basic.AddObject(profile_basic);

                    // Add user to Jobseeker role
                    var user = db.Users.FirstOrDefault(u=> u.UserId == model.UserName);
                    var role = db.Roles.FirstOrDefault(r => r.RoleName == "Jobseeker");
                    role.Users.Add(user);   
                    db.SaveChanges();
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            ViewBag.Gender = new SelectList(new Genders { }.GetGenders, model.Gender);
            ViewBag.City = new SelectList(new City { }.GetCities, model.City);
            ViewBag.Country = new SelectList(new Countries { }.GetCountries, model.Country);
            ViewBag.MaritalStatus = new SelectList(new MaritalStatus { }.GetMaritalStatus, model.MaritalStatus);

            // If we got this far, something failed, redisplay form
            return View(model);
        }                

        //
        // GET: /Account/EmployerRegister

        public ActionResult EmployerRegister()
        {
            ViewBag.Industry = new SelectList(new Industry { }.GetIndustry);
            ViewBag.CompanySize = new SelectList(new CompanySize().GetCompanySizes);
            ViewBag.City = new SelectList(new City { }.GetCities);
            ViewBag.Country = new SelectList(new Countries { }.GetCountries);
            return View();
        }

        //
        // POST: /Account/EmployerRegister

        [HttpPost]
        public ActionResult EmployerRegister(EmployerRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    // Save basic information
                    JobZoomEntities db = new JobZoomEntities();
                    Company company = new Company();
                    company.CompanyId = Guid.NewGuid().ToString();
                    company.UserId = model.UserName;
                    company.Name = model.CompanyName;
                    company.Industry = model.Industry;
                    company.CompanySize = model.CompanySize;                    
                    company.Country = model.Country;
                    company.State = model.State;
                    company.City = model.City;
                    company.Website = model.Website;
                    db.Companies.AddObject(company);

                    // Add user to Jobseeker role
                    var user = db.Users.FirstOrDefault(u => u.UserId == model.UserName);
                    var role = db.Roles.FirstOrDefault(r => r.RoleName == "Employer");
                    role.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            ViewBag.Industry = new SelectList(new Industry { }.GetIndustry);
            ViewBag.CompanySize = new SelectList(new CompanySize().GetCompanySizes);
            ViewBag.City = new SelectList(new City { }.GetCities);
            ViewBag.Country = new SelectList(new Countries { }.GetCountries);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Activate(string username, string key)
        {
            CustomMembershipProvider membershipProvider = new CustomMembershipProvider();
            if (membershipProvider.ActivateUser(username, key))
                FormsAuthentication.SetAuthCookie(username, false);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResendActivatedEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResendActivatedEmail(ActivatedEmailModel Model)
        {
            if (ModelState.IsValid)
            {
                // ResendActivatedEmail will throw an exception rather
                // than return false in certain failure scenarios.
                bool ResendActivatedEmailSucceeded;
                try
                {
                    CustomMembershipProvider _user = new CustomMembershipProvider();
                    ResendActivatedEmailSucceeded = _user.resendActivatedEmail(Model.Email);
                }
                catch (Exception)
                {
                    ResendActivatedEmailSucceeded = false;
                } 

                if (ResendActivatedEmailSucceeded)
                {
                    return RedirectToAction("ResendActivatedEmailSucceeded");
                }
                else
                {
                    ModelState.AddModelError("", "Your email doesn't exists!");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(Model);
        }

        public ActionResult ResendActivatedEmailSucceeded()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}

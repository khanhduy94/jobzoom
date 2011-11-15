using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobZoom.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //EMPLOYER ROUTINGS
            routes.MapRoute(
                "EmployerDefault",//Route name
                "Employer/",
                new { controller = "EmployerHome", action = "Home", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "EmployerHome",//Route name
                "Employer/Home/{action}/{id}",
                new { controller = "EmployerHome", action = "Home", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "EmployerJobPost",
                "Employer/Job/Post/{action}/{id}",
                new { controller = "EmployerJobPost", action = "Basic", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "EmployerEditJob",
                "Employer/Job/Edit/{action}/{id}",
                new { controller = "EmployerEditJob", action = "Basic", id = UrlParameter.Optional }
                );

            


            routes.MapRoute(
                "ProfileDefault",
                "Profile/",
                new { controller = "Profile", action = "Home", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "ProfileHome",
                "Profile/Home/{action}/{id}",
                new { controller = "Profile", action = "Home", id = UrlParameter.Optional }
                );

            //JOB SEEKER ROUTINGS            
            routes.MapRoute(
                "ProfileEditDefault",
                "Profile/Edit/",
                new { controller = "ProfileEdit", action = "Basic", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "ProfileEdit",
                "Profile/Edit/{action}/{id}",
                new { controller = "ProfileEdit", action = "Basic", id = UrlParameter.Optional }
                );            

            //Adminstrator Page
            routes.MapRoute(
                "AdminHome",
                "Admin/",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "AdminCompany",
                "Admin/Company/{action}/{id}",
                new { controller = "AdminCompany", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "AdminUser",
                "Admin/User/{action}/{id}",
                new { controller = "AdminUser", action = "Index", id = UrlParameter.Optional }
                );



            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
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

            routes.MapRoute(
                "EmployerDefault",//Route name
                "Employer/",
                new { controller = "EmployerHome", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "EmployerHome",//Route name
                "Employer/Home/{action}/{id}",
                new { controller = "EmployerHome", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "ProfileDefault",
                "Profile/",
                new { controller = "ProfileHome", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "ProfileHome",
                "Profile/Home/{action}/{id}",
                new { controller = "ProfileHome", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "ProfileEditDefault",
                "Profile/Edit/",
                new { controller = "ProfileEdit", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                "ProfileEdit",
                "Profile/Edit/{action}/{id}",
                new { controller = "ProfileEdit", action = "Index", id = UrlParameter.Optional }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            //filterContext.Result = new HttpStatusCodeResult(403);
            throw new HttpException(403, "You can't access this page!");
        else
            filterContext.Result = new HttpUnauthorizedResult();
    }
   
}



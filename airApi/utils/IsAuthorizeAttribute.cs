using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace airApi.utils
{
    public class IsAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Login");
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = false;
            if (httpContext.Session["username"] != null)
            {
                if (!HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower().Contains("admin")
                    || httpContext.Session["username"].ToString() == "admin")
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using LMS.Common;
using LMS.Common.Enums;

namespace LMS.Web.Attributes
{
    public class Authorization : AuthorizeAttribute
    {
        private readonly RolesEnum[] allowedRoles;

        public Authorization(params RolesEnum[] roles)
        {
            this.allowedRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["role"] != null)
            {
                RolesEnum userRole = (RolesEnum)httpContext.Session["role"];

                //If userRole is Dealer, then check if DealerId is set in Session
                if (userRole == RolesEnum.DealerManager)
                    if (httpContext.Session["dealerId"] == null)
                        return false;

                //check if valid role
                if (allowedRoles.Contains(userRole))
                    return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Authentication" },
                    { "action", "Unauthorized" }
                });
        }
    }
}
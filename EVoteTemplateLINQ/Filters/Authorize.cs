using System;
using System.Security.Policy;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Routing;

namespace EVote.Filters
{
    public class VerifyUserAttribute : ActionFilterAttribute
    {
        //public new RedirectToRouteResult RedirectToAction(string action, string controller)
        //{
        //    return base.RedirectToAction(action, controller);
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.LogError = "A general error occured while processing this request.";
            var user = filterContext.HttpContext.Session["UserID"];
            if (user == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Login");
            }
        }
    }

    public class VerifySuperUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.LogError = "Unauthorized User";
            var role = Convert.ToInt32(filterContext.HttpContext.Session["RoleID"]);
            if (role < 2)            
                filterContext.Result = new RedirectResult("~/Home/Login");
            
        }
    }

    public class VerifyAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.LogError = "Unauthorized User";
            var role = Convert.ToInt32(filterContext.HttpContext.Session["RoleID"]);
            if (role != 3)            
                filterContext.Result = new RedirectResult("~/Home/Login");
            
        }
    }

    // Dont know why the string.Format function is here ...
    //public class VerifyAdminAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var role = Convert.ToInt32(filterContext.HttpContext.Session["RoleID"]);
    //        if (role != 3)
    //            filterContext.Result = new RedirectResult(string.Format("~/Home/Login", filterContext.HttpContext.Request.Url.AbsolutePath));

    //    }
    //}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WMS.Models;

namespace WMS.Controllers.Filters
{
    public class CustomActionAttribute: FilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.OnActionExecuted = "IActionFilter.OnActionExecuted filter called";
        }
        
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            User _User = session["LoggedUser"] as User;
                bool HavePermission = false;
                try
                {
                    switch (filterContext.ActionDescriptor.ActionName)
                    {
                        case "Create":
                            if (CheckCreateActionPermission(_User))
                                HavePermission = true;
                            break;
                        case "Edit":
                            if (CheckEditActionPermission(_User))
                                HavePermission = true;
                            break;
                        case "Details":
                            if (CheckDetailActionPermission(_User))
                                HavePermission = true;
                            break;
                        case "Delete":
                            if (CheckDeleteActionPermission(_User))
                                HavePermission = true;
                            break;
                    }
                    if (HavePermission == false)
                    {
                        //filterContext.Result = new HttpUnauthorizedResult();
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, action = "Index" }));
                        filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                        
                    }
                }
                catch (Exception ex)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
        }

        private bool CheckDeleteActionPermission(User _User)
        {
            if (_User.CanDelete == true)
                return true;
            else
                return false;
        }

        private bool CheckDetailActionPermission(User _User)
        {
            if (_User.CanView == true)
                return true;
            else
                return false;
        }

        private bool CheckEditActionPermission(User _User)
        {
            if (_User.CanEdit == true)
                return true;
            else
                return false;
        }

        private bool CheckCreateActionPermission(User _User)
        {
            if (_User.CanAdd == true)
                return true;
            else
                return false;
        }
    }
}
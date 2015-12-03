using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WMS.Models;

namespace WMS.Controllers.Filters
{
    public class CustomControllerAttributes : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            bool HavePermission = false;
            try
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                User _User = session["LoggedUser"] as User;
                switch (controllerName)
                {
                    case "Category":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "City":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Crew":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Dept":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Designation":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Division":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Emp":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "EmpType":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Grade":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Holiday":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Home":

                        break;
                    case "JobTitle":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Location":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "LvApp":
                        if (CheckLeavePermision(_User))
                            HavePermission = true;
                        break;
                    case "LvQuota":
                        if (CheckLeavePermision(_User))
                            HavePermission = true;
                        break;
                    case "LvShort":
                        if (CheckLeavePermision(_User))
                            HavePermission = true;
                        break;
                    case "Reader":
                        if (CheckDevicePermision(_User))
                            HavePermission = true;
                        break;
                    case "ReaderType":
                        if (CheckDevicePermision(_User))
                            HavePermission = true;
                        break;
                    case "Region":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Section":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Shift":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "Site":
                        if (CheckHRPermision(_User))
                            HavePermission = true;
                        break;
                    case "User":
                        if (CheckUserPermision(_User))
                            HavePermission = true;
                        break;
                    case "Vendor":
                        if (CheckDevicePermision(_User))
                            HavePermission = true;
                        break;
                    case "EditAttendance":
                        if (CheckAttEditPermision(_User))
                            HavePermission = true;
                        break;
                }
                if (HavePermission == false)
                {
                    //filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                } 
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
            
        }

        private bool CheckAttEditPermision(User _User)
        {
            try
            {
                if (_User.MEditAtt == true)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CheckUserPermision(User _User)
        {
            try
            {
                if (_User.MUser == true)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CheckDevicePermision(User _User)
        {
            try
            {
                if (_User.MDevice == true)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private bool CheckLeavePermision(User _User)
        {
            try
            {
                if (_User.MLeave == true)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private bool CheckHRPermision(User _User)
        {
            try
            {
                if (_User.MHR == true)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        } 
    }
}
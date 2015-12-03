using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services;
using WMS.CustomClass;
using WMS.Models;

namespace WMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            // Initialize Session["FiltersModel"] -- Move to First Page
            Session["FiltersModel"] = new WMSLibrary.FiltersModel();
            Session["LoginCount"] = null;
            LoadSessionValues();
            //LoadSession();
        }
        private void LoadSession()
        {
            using (TAS2013Entities dc = new TAS2013Entities())
            {
               var v = dc.Users.Where(aa => aa.UserName == "wms.ffl").FirstOrDefault();
                if (v != null)
                {
                    Session["MDevice"] = "0";
                    Session["MHR"] = "0";
                    Session["MDevice"] = "0";
                    Session["MLeave"] = "0";
                    Session["MEditAtt"] = "0";
                    Session["MUser"] = "0";
                    Session["LogedUserFullname"] = "";
                    Session["UserCompany"] = "";
                    Session["MRDailyAtt"] = "0";
                    Session["MRLeave"] = "0";
                    Session["MRMonthly"] = "0";
                    Session["MRAudit"] = "0";
                    Session["MRManualEditAtt"] = "0";
                    Session["MREmployee"] = "0";
                    Session["MRDetail"] = "0";
                    Session["MRSummary"] = "0";
                    Session["LogedUserID"] = v.UserID.ToString();
                    Session["LogedUserFullname"] = v.UserName;
                    Session["LoggedUser"] = v;
                    if (v.MHR == true)
                        Session["MHR"] = "1";
                    if (v.MDevice == true)
                        Session["MDevice"] = "1";
                    if (v.MLeave == true)
                        Session["MLeave"] = "1";
                    if (v.MEditAtt == true)
                        Session["MEditAtt"] = "1";
                    if (v.MUser == true)
                        Session["MUser"] = "1";
                    if (v.MRDailyAtt == true)
                        Session["MRDailyAtt"] = "1";
                    if (v.MRLeave == true)
                        Session["MRLeave"] = "1";
                    if (v.MRMonthly == true)
                        Session["MRMonthly"] = "1";
                    if (v.MRAudit == true)
                        Session["MRAudit"] = "1";
                    if (v.MRManualEditAtt == true)
                        Session["MRManualEditAtt"] = "1";
                    if (v.MREmployee == true)
                        Session["MREmployee"] = "1";
                    if (v.MRDetail == true)
                        Session["MRDetail"] = "1";
                    if (v.MRSummary == true)
                        Session["MRSummary"] = "1";
                    if (v.MRoster == true)
                        Session["MRoster"] = "1";

                }
            }
        }

        private void LoadSessionValues()
        {
            Session["ReportSession"] = new List<string>();
            List<string> list = Session["ReportSession"] as List<string>;
            list.Add(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
            list.Add(DateTime.Today.ToString("yyyy-MM-dd"));
            list.Add("EmpView");
            Session["ReportSession"] = list;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session["FiltersModel"] = null;
            Session["LogedUserID"] = null;
            Session["LoggedUser"] = null;
        }

        //void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    if (Request == null || Request.Cookies == null)
        //    {
        //        return;
        //    }
        //    if (Request.Cookies.Count < 10)
        //    {
        //        return;
        //    }
        //    for (int i = 0; i < Request.Cookies.Count; ++i)
        //    {
        //        if (StringComparer.OrdinalIgnoreCase.Equals(Request.Cookies[i].Name, System.Web.Security.FormsAuthentication.FormsCookieName))
        //        {
        //            continue;
        //        }
        //        if (!Request.Cookies[i].Name.EndsWith("_SKA", System.StringComparison.OrdinalIgnoreCase))
        //        {
        //            continue;
        //        }
        //        if (i > 10)
        //        {
        //            break;
        //        }

        //        System.Web.HttpCookie c = new System.Web.HttpCookie(Request.Cookies[i].Name);
        //        c.Expires = DateTime.Now.AddDays(-1);
        //        c.Path = "/";
        //        c.Secure = false;
        //        c.HttpOnly = true;
        //        Response.Cookies.Set(c);
        //    }
        //}

        protected void Session_End()
        {
            Session["FiltersModel"] = null;
            Session["LogedUserID"] = null;
            Session["LoggedUser"] = null;
        }


    }



}

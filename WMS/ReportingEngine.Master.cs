using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS.Models;
using WMS.Reports;

namespace WMS
{
    public partial class ReportingEngine : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["LogedUserID"].ToString() != "")
            {
                //Deployment Type =false : Local Deployment
                //Deployment Type =true: Server Deployment
                //GlobalVariables.DeploymentType = false;
            }
            else
                Response.Redirect("~/Home");
        }

        private void LoadSession()
        {
            using (TAS2013Entities dc = new TAS2013Entities())
            {
                var v = dc.Users.Where(aa=>aa.UserName=="wms.ffl").FirstOrDefault();
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS.Reports.Filters
{
    public partial class StepSixFilter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            //SaveDateSession();
            //SaveSectionIDs();
            //SaveLocationIDs();
            Response.Redirect(path);
        }
        protected void btnStepOne_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepOneFilter.aspx");
        }

        protected void btnStepTwo_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepTwoFilter.aspx");
        }

        protected void btnStepThree_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepThreeFilter.aspx");
        }

        protected void btnStepFour_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepFourFilter.aspx");
        }

        protected void btnStepFive_Click(object sender, EventArgs e)
        {
            NavigationCommonCalls("~/Reports/Filters/StepFiveFilter.aspx");
        }

        //protected void btnStepSix_Click(object sender, EventArgs e)
        //{
        //    //SaveDateSession();
        //    //SaveSectionIDs();
        //    //SaveLocationIDs();
        //    //FiltersModel fm = Session["FiltersModel"] as FiltersModel;
        //    //if (MyHelper.UserHasValuesInSession(fm))
        //    //{
        //        Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
        //    }
        //}


        #endregion

    }
}
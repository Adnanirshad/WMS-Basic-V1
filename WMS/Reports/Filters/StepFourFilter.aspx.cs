using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS.CustomClass;
using WMS.HelperClass;
using WMS.Models;
using WMSLibrary;

namespace WMS.Reports.Filters
{
    public partial class StepFourFilter : System.Web.UI.Page
    {
        private TAS2013Entities da = new TAS2013Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewType("");
                BindGridViewShift("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
            }
            else
            {
                SaveTypeIDs();
                SaveShiftIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
            }
        }
        protected void ButtonSearchType_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveTypeIDs();
            BindGridViewType(tbSearch_Type.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
        }
        protected void ButtonSearchShift_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveShiftIDs();
            BindGridViewShift(tbSearch_Shift.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
        }
        protected void GridViewType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveTypeIDs();

            //change page index
            GridViewType.PageIndex = e.NewPageIndex;
            BindGridViewType(tbSearch_Type.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
        }
        #region --DeleteAll Filters--
        protected void ButtonDeleteAll_Click(object sender, EventArgs e)
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            list[1] = DateTime.Today.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
            dateFrom.Value = list[0];
            dateTo.Value = list[1];
            WMSLibrary.Filters filtersHelper = new WMSLibrary.Filters();
            Session["FiltersModel"] = filtersHelper.DeleteAllFilters(Session["FiltersModel"] as FiltersModel);

            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Employee");


        }
        #endregion 
        protected void GridViewShift_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveShiftIDs();

            //change page index
            GridViewShift.PageIndex = e.NewPageIndex;
            BindGridViewShift("");
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
        }

        private void SaveTypeIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            Session["FiltersModel"] = FM;
        }
        private void SaveShiftIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewType(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<EmpType> _View = new List<EmpType>();
            QueryBuilder qb = new QueryBuilder();
            DataTable dt = qb.GetValuesfromDB("select * from EmpType ");
            _View = dt.ToList<EmpType>();
            GridViewType.DataSource = _View.Where(aa => aa.TypeName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewType.DataBind();
        }

        private void BindGridViewShift(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<Shift> _View = new List<Shift>();
            QueryBuilder qb = new QueryBuilder();
            DataTable dt = qb.GetValuesfromDB("select * from Shift ");
            _View = dt.ToList<Shift>();
            GridViewShift.DataSource = _View.Where(aa => aa.ShiftName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewShift.DataBind();
        }


        protected void GridViewType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewType.PageIndex + 1) + " of " + GridViewType.PageCount;
            }
        }

        protected void GridViewShift_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewShift.PageIndex + 1) + " of " + GridViewShift.PageCount;
            }
        }
        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }
        public DateTime DateFrom
        {
            get
            {
                if (dateFrom.Value == "")
                    return DateTime.Today.Date.AddDays(-1);
                else
                    return DateTime.Parse(dateFrom.Value);
            }
        }
        public DateTime DateTo
        {
            get
            {
                if (dateTo.Value == "")
                    return DateTime.Today.Date.AddDays(-1);
                else
                    return DateTime.Parse(dateTo.Value);
            }
        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveShiftIDs();
            SaveTypeIDs();
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

        protected void btnStepSix_Click(object sender, EventArgs e)
        {
            SaveDateSession();
            SaveTypeIDs();
            SaveShiftIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
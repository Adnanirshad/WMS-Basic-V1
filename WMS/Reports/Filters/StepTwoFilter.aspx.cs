using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS.CustomClass;
using WMS.HelperClass;
using System.Linq.Dynamic;
using WMS.Models;
using WMSLibrary;

namespace WMS.Reports.Filters
{
    public partial class StepTwoFilter : System.Web.UI.Page
    {
        private TAS2013Entities da = new TAS2013Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewDivision("");
                BindGridViewShift("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
            }
            else
            {
                SaveDivisionIDs();
                SaveShiftIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
            }
        }
        protected void ButtonSearchShift_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveShiftIDs();
            BindGridViewShift(tbSearch_Shift.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
        }
        protected void ButtonSearchDivision_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDivisionIDs();
            BindGridViewDivision(TextBoxSearchDivision.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
        }
        protected void GridViewDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDivisionIDs();

            //change page index
            GridViewDivision.PageIndex = e.NewPageIndex;
            BindGridViewDivision(TextBoxSearchDivision.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
        }

        protected void GridViewShift_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveShiftIDs();

            //change page index
            GridViewShift.PageIndex = e.NewPageIndex;
            BindGridViewShift(tbSearch_Shift.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
        }

        private void SaveShiftIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewShift, Session["FiltersModel"] as FiltersModel, "Shift");
            Session["FiltersModel"] = FM;
        }
        private void SaveDivisionIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
            Session["FiltersModel"] = FM;
        }
        #region --DeleteAll Filters--
        protected void ButtonDeleteAll_Click(object sender, EventArgs e)
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            list[1] = DateTime.Today.ToString("yyyy-MM-dd");
            dateFrom.Value = list[0];
            dateTo.Value = list[1];
            Session["ReportSession"] = list;
            WMSLibrary.Filters filtersHelper = new WMSLibrary.Filters();
            Session["FiltersModel"] = filtersHelper.DeleteAllFilters(Session["FiltersModel"] as FiltersModel);

            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Employee");


        }
        #endregion 
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
        private void BindGridViewDivision(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewDivision> _View = new List<ViewDivision>();
            List<ViewDivision> _TempView = new List<ViewDivision>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewLinq(LoggedInUser);
           
            _View = da.ViewDivisions.Where(query).AsQueryable().OrderBy("DivisionName ASC").ToList();
            if (fm.CompanyFilter.Count > 0)
            {
                foreach (var comp in fm.CompanyFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.CompID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewDivision.DataSource = _View.Where(aa => aa.DivisionName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewDivision.DataBind();
        }

        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }

        private void BindGridViewShift(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<Shift> _View = new List<Shift>();
            List<Shift> _TempView = new List<Shift>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForShiftForLinq(LoggedInUser);
           // DataTable dt = qb.GetValuesfromDB("select * from Shift " + query);
            _View = da.Shifts.Where(query).AsQueryable().OrderBy("ShiftName ASC").ToList();
            if (fm.LocationFilter.Count > 0)
            {
                foreach (var loc in fm.LocationFilter)
                {
                    short _locID = Convert.ToInt16(loc.ID);
                    _TempView.AddRange(_View.Where(aa => aa.LocationID == _locID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewShift.DataSource = _View.Where(aa => aa.ShiftName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewShift.DataBind();
        }


        protected void GridViewDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDivision.PageIndex + 1) + " of " + GridViewDivision.PageCount;
            }
        }

        protected void GridViewShift_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewShift.PageIndex + 1) + " of " + GridViewShift.PageCount;
            }
        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveDivisionIDs();
            SaveShiftIDs();
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
            SaveShiftIDs();
            SaveDivisionIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
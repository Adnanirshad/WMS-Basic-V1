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
    public partial class StepThreeFilter : System.Web.UI.Page
    {
        private TAS2013Entities da = new TAS2013Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewSection("");
                BindGridViewLocation("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
            }
            else
            {
                SaveSectionIDs();
                SaveLocationIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            }
        }
        protected void ButtonSearchSection_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            BindGridViewSection(tbSearch_Section.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
        }
        protected void ButtonSearchLocation_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveLocationIDs();
            BindGridViewLocation(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
        }
        protected void GridViewSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();

            //change page index
            GridViewSection.PageIndex = e.NewPageIndex;
            BindGridViewSection(tbSearch_Section.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
        }

        protected void GridViewLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveLocationIDs();

            //change page index
            GridViewLocation.PageIndex = e.NewPageIndex;
            BindGridViewLocation(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
        }

        private void SaveSectionIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
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

            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Employee");


        }
        #endregion 
        private void SaveLocationIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewSection(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewSection> _View = new List<ViewSection>();
            List<ViewSection> _TempView = new List<ViewSection>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyView(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewSection " + query + " ORDER BY SectionName ASC");
            _View = dt.ToList<ViewSection>();
            if (fm.DivisionFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var div in fm.DivisionFilter)
                {
                    short _divID = Convert.ToInt16(div.ID);
                    _TempView.AddRange(_View.Where(aa => aa.DivID == _divID).ToList());
                }
                _View = _TempView.ToList();
            }
            if (fm.DepartmentFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var dept in fm.DepartmentFilter)
                {
                    short _deptID = Convert.ToInt16(dept.ID);
                    _TempView.AddRange(_View.Where(aa => aa.DeptID == _deptID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewSection.DataSource = _View.Where(aa => aa.DeptName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewSection.DataBind();
        }

        private void BindGridViewLocation(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewLocation> _View = new List<ViewLocation>();
            List<ViewLocation> _TempView = new List<ViewLocation>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForLocReport(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewLocation " + query +" order by LocName asc");
            _View = dt.ToList<ViewLocation>();
            if (fm.RegionFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var reg in fm.RegionFilter)
                {
                    short _regID = Convert.ToInt16(reg.ID);
                    _TempView.AddRange(_View.Where(aa => aa.RegionID == _regID).ToList());
                }
                _View = _TempView.ToList();
            }
            if (fm.CityFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var city in fm.CityFilter)
                {
                    short _cityID = Convert.ToInt16(city.ID);
                    _TempView.AddRange(_View.Where(aa => aa.CityID == _cityID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewLocation.DataSource = _View.Where(aa => aa.LocName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewLocation.DataBind();
        }

        #region Navigation Buttons
        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            SaveDateSession();
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            SaveLocationIDs();
            // Go to the next page
            string url = "~/Reports/Filters/StepFourFilter.aspx";
            Response.Redirect(url);
        }
        protected void ButtonSkip_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            // Go to the next page
            string url = "~/Filters/DeptFilter.aspx";
            Response.Redirect(url);
        }
        protected void ButtonFinish_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();
            SaveLocationIDs();
            // Go to the next page
            string url = "~/Reports/ReportContainer.aspx";
            Response.Redirect(url);
        }
        #endregion

        protected void GridViewSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewSection.PageIndex + 1) + " of " + GridViewSection.PageCount;
            }
        }

        protected void GridViewLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewLocation.PageIndex + 1) + " of " + GridViewLocation.PageCount;
            }
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
        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveSectionIDs();
            SaveLocationIDs();
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
            SaveSectionIDs();
            SaveLocationIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
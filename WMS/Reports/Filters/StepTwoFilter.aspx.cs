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
                BindGridViewDepartment("");
                BindGridViewCity("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
            }
            else
            {
                SaveDepartmentIDs();
                SaveCityIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewCity, Session["FiltersModel"] as FiltersModel, "City");
            }
        }
        protected void ButtonSearchCity_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveCityIDs();
            BindGridViewCity(tbSearch_City.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewCity, Session["FiltersModel"] as FiltersModel, "City");
        }
        protected void ButtonSearchDepartment_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();
            BindGridViewDepartment(TextBoxSearchDepartment.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
        }
        protected void GridViewDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();

            //change page index
            GridViewDepartment.PageIndex = e.NewPageIndex;
            BindGridViewDepartment(TextBoxSearchDepartment.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
        }

        protected void GridViewCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveCityIDs();

            //change page index
            GridViewCity.PageIndex = e.NewPageIndex;
            BindGridViewCity(tbSearch_City.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewCity, Session["FiltersModel"] as FiltersModel, "City");
        }

        private void SaveCityIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewCity, Session["FiltersModel"] as FiltersModel, "City");
            Session["FiltersModel"] = FM;
        }
        private void SaveDepartmentIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
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

            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "City");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Employee");


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
        private void BindGridViewDepartment(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewDepartment> _View = new List<ViewDepartment>();
            List<ViewDepartment> _TempView = new List<ViewDepartment>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForReportsDepartment(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewDepartment " + query + " order by DeptName asc");
            _View = dt.ToList<ViewDepartment>();
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
            GridViewDepartment.DataSource = _View.Where(aa => aa.DeptName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewDepartment.DataBind();
        }

        private void SaveDateSession()
        {
            List<string> list = Session["ReportSession"] as List<string>;
            list[0] = DateFrom.ToString("yyyy-MM-dd");
            list[1] = DateTo.ToString("yyyy-MM-dd");
            Session["ReportSession"] = list;
        }

        private void BindGridViewCity(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<City> _View = new List<City>();
            List<City> _TempView = new List<City>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForReportsCity(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from City " + query + " order by CityName asc");
            _View = dt.ToList<City>();
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
            GridViewCity.DataSource = _View.Where(aa => aa.CityName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewCity.DataBind();
        }


        protected void GridViewDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDepartment.PageIndex + 1) + " of " + GridViewDepartment.PageCount;
            }
        }

        protected void GridViewCity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewCity.PageIndex + 1) + " of " + GridViewCity.PageCount;
            }
        }
        #region Navigation Buttons
        private void NavigationCommonCalls(string path)
        {
            SaveDateSession();
            SaveDepartmentIDs();
            SaveCityIDs();
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
            SaveCityIDs();
            SaveDepartmentIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
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
                BindGridViewSection("");
                BindGridViewCrew("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
            }
            else
            {
                SaveSectionIDs();
                SaveCrewIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewCrew, Session["FiltersModel"] as FiltersModel, "Crew");
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
        protected void ButtonSearchCrew_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveCrewIDs();
            BindGridViewCrew(tbSearch_Crew.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewCrew, Session["FiltersModel"] as FiltersModel, "Crew");
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

            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Employee");


        }
        #endregion 
        protected void GridViewCrew_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveCrewIDs();

            //change page index
            GridViewCrew.PageIndex = e.NewPageIndex;
            BindGridViewCrew("");
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewCrew, Session["FiltersModel"] as FiltersModel, "Crew");
        }

        private void SaveSectionIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            Session["FiltersModel"] = FM;
        }
        private void SaveCrewIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewCrew, Session["FiltersModel"] as FiltersModel, "Crew");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewSection(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewSection> _View = new List<ViewSection>();
            List<ViewSection> _TempView = new List<ViewSection>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewLinq(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewSection where " + query);
            _View = dt.ToList<ViewSection>().AsQueryable().SortBy("SectionName").ToList();
            if (fm.CompanyFilter.Count > 0)
            {
                foreach (var comp in fm.CompanyFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.CompID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            if (fm.DivisionFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var comp in fm.DivisionFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.DivID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            if (fm.DepartmentFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var comp in fm.DepartmentFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.DeptID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewSection.DataSource = _View.Where(aa => aa.SectionName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewSection.DataBind();
        }

        private void BindGridViewCrew(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewCrew> _View = new List<ViewCrew>();
            List<ViewCrew> _TempView = new List<ViewCrew>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyFilters(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewCrew " + query);
            _View = dt.ToList<ViewCrew>().AsQueryable().SortBy("CrewName").ToList();
            if (fm.CompanyFilter.Count > 0)
            {
                foreach (var comp in fm.CompanyFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.CompanyID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewCrew.DataSource = _View.Where(aa => aa.CrewName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewCrew.DataBind();
        }


        protected void GridViewSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewSection.PageIndex + 1) + " of " + GridViewSection.PageCount;
            }
        }

        protected void GridViewCrew_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewCrew.PageIndex + 1) + " of " + GridViewCrew.PageCount;
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
            SaveCrewIDs();
            SaveSectionIDs();
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
            SaveCrewIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
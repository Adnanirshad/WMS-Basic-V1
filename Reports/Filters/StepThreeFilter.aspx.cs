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
                BindGridViewDepartment("");
                BindGridViewType("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
            }
            else
            {
                SaveDepartmentIDs();
                SaveTypeIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            }
        }
        protected void ButtonSearchDepartment_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();
            BindGridViewDepartment(tbSearch_Department.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
        }
        protected void ButtonSearchType_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveTypeIDs();
            BindGridViewType(tbSearch_Type.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
        }
        protected void GridViewDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();

            //change page index
            GridViewDepartment.PageIndex = e.NewPageIndex;
            BindGridViewDepartment(tbSearch_Department.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
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
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDepartment, Session["FiltersModel"] as FiltersModel, "Employee");


        }
        #endregion 
        private void SaveTypeIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewType, Session["FiltersModel"] as FiltersModel, "Type");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewDepartment(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewDepartment> _View = new List<ViewDepartment>();
            List<ViewDepartment> _TempView = new List<ViewDepartment>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyView(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewDepartment " + query + " ORDER BY DeptName ASC");
            _View = dt.ToList<ViewDepartment>();
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

        private void BindGridViewType(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewEmpType> _View = new List<ViewEmpType>();
            List<ViewEmpType> _TempView = new List<ViewEmpType>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewForLinq(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewEmpType where " + query);
            _View = dt.ToList<ViewEmpType>().AsQueryable().SortBy("TypeName").ToList();
            if (fm.CompanyFilter.Count > 0)
            {
                foreach (var comp in fm.CompanyFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.CompanyID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewType.DataSource = _View.Where(aa => aa.TypeName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewType.DataBind();
        }

        #region Navigation Buttons
        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            SaveDateSession();
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();
            SaveTypeIDs();
            // Go to the next page
            string url = "~/Reports/Filters/StepFourFilter.aspx";
            Response.Redirect(url);
        }
        protected void ButtonSkip_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();
            // Go to the next page
            string url = "~/Filters/DeptFilter.aspx";
            Response.Redirect(url);
        }
        protected void ButtonFinish_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDepartmentIDs();
            SaveTypeIDs();
            // Go to the next page
            string url = "~/Reports/ReportContainer.aspx";
            Response.Redirect(url);
        }
        #endregion

        protected void GridViewDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDepartment.PageIndex + 1) + " of " + GridViewDepartment.PageCount;
            }
        }

        protected void GridViewType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewType.PageIndex + 1) + " of " + GridViewType.PageCount;
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
            SaveDepartmentIDs();
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
            SaveDepartmentIDs();
            SaveTypeIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
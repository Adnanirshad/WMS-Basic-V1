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
    public partial class StepFiveFilter : System.Web.UI.Page
    {
        private TAS2013Entities da = new TAS2013Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridViewEmployee("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
                //dateFrom.Value = "2015-08-09";
            }
            else
            {
                SaveEmployeeIDs();
            }
            if (Session["FiltersModel"] != null)
            {
                // Check and Uncheck Items in grid view according to Session Filters Model
                WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
            }
        }
        protected void ButtonSearchEmployee_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveEmployeeIDs();
            BindGridViewEmployee(tbSearch_Employee.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
        }
        protected void GridViewEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveEmployeeIDs();

            //change page index
            GridViewEmployee.PageIndex = e.NewPageIndex;
            BindGridViewEmployee(tbSearch_Employee.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
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

            //WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Location");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Section");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");


        }
        #endregion
        private void SaveEmployeeIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewEmployee, Session["FiltersModel"] as FiltersModel, "Employee");
            Session["FiltersModel"] = FM;
        }

        private void BindGridViewEmployee(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<EmpView> _View = new List<EmpView>();
            List<EmpView> _TempView = new List<EmpView>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.MakeCustomizeQueryForEmpView(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from EmpView " + query);
            _View = dt.ToList<EmpView>().AsQueryable().SortBy("EmpNo").ToList();
            _View = _View.Where(aa => aa.Status == true).ToList();
            //if (fm.DivisionFilter.Count > 0)
            //{
            //    _TempView.Clear();
            //    foreach (var comp in fm.DivisionFilter)
            //    {
            //        short _compID = Convert.ToInt16(comp.ID);
            //        _TempView.AddRange(_View.Where(aa => aa.DivID == _compID).ToList());
            //    }
            //    _View = _TempView.ToList();
            //}
            //if (fm.DepartmentFilter.Count > 0)
            //{
            //    _TempView.Clear();
            //    foreach (var comp in fm.DepartmentFilter)
            //    {
            //        short _compID = Convert.ToInt16(comp.ID);
            //        _TempView.AddRange(_View.Where(aa => aa.DeptID == _compID).ToList());
            //    }
            //    _View = _TempView.ToList();
            //}
            if (fm.SectionFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var comp in fm.SectionFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.SecID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            if (fm.TypeFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var comp in fm.TypeFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.TypeID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            //if (fm.RegionFilter.Count > 0)
            //{
            //    _TempView.Clear();
            //    foreach (var comp in fm.RegionFilter)
            //    {
            //        short _compID = Convert.ToInt16(comp.ID);
            //        _TempView.AddRange(_View.Where(aa => aa.RegionID == _compID).ToList());
            //    }
            //    _View = _TempView.ToList();
            //}
            //if (fm.CityFilter.Count > 0)
            //{
            //    _TempView.Clear();
            //    foreach (var comp in fm.CityFilter)
            //    {
            //        short _compID = Convert.ToInt16(comp.ID);
            //        _TempView.AddRange(_View.Where(aa => aa.CityID == _compID).ToList());
            //    }
            //    _View = _TempView.ToList();
            //}
            if (fm.LocationFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var comp in fm.LocationFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.LocID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            if (fm.ShiftFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var comp in fm.ShiftFilter)
                {
                    short _compID = Convert.ToInt16(comp.ID);
                    _TempView.AddRange(_View.Where(aa => aa.ShiftID == _compID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewEmployee.DataSource = _View.Where(aa => aa.EmpName.ToUpper().Contains(search.ToUpper()) || aa.EmpNo.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewEmployee.DataBind();
        }


        protected void GridViewEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewEmployee.PageIndex + 1) + " of " + GridViewEmployee.PageCount;
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
            SaveEmployeeIDs();
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
            SaveEmployeeIDs();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion
    }
}
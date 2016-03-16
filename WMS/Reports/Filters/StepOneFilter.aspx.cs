using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using WMS.CustomClass;
using WMS.HelperClass;
using WMS.Models;
using WMSLibrary;
using System.Web.Services;

namespace WMS.Reports.Filters
{
    public partial class StepOneFilter : System.Web.UI.Page
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
            BindGridViewSection(TextBoxSearch.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            //fml = Session["FiltersModel"] as FiltersModel;
        }
         private void SaveSectionIDs()
        {
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            Session["FiltersModel"] = FM;
        }

        protected void GridViewSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveSectionIDs();

            //change page index
            GridViewSection.PageIndex = e.NewPageIndex;
            BindGridView(TextBoxSearch.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
        }
         protected void GridViewSection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewSection.PageIndex + 1) + " of " + GridViewSection.PageCount;
            }
        }

         private void BindGridViewSection(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewSection> _View = new List<ViewSection>();
            List<ViewSection> _TempView = new List<ViewSection>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewLinq(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewSection ");
            _View = dt.ToList<ViewSection>().AsQueryable().SortBy("SectionName").ToList();
            if (fm.SectionFilter.Count > 0)
            {
                foreach (var Sec in fm.SectionFilter)
                {
                    short _SecID = Convert.ToInt16(Sec.ID);
                    _TempView.AddRange(_View.Where(aa => aa.SectionID == _SecID).ToList());
                }
                _View = _TempView.ToList();
            }
            
            if (fm.DepartmentFilter.Count > 0)
            {
                _TempView.Clear();
                foreach (var Sec in fm.DepartmentFilter)
                {
                    short _SecID = Convert.ToInt16(Sec.ID);
                    _TempView.AddRange(_View.Where(aa => aa.SectionID == _SecID).ToList());
                }
                _View = _TempView.ToList();
            }
            GridViewSection.DataSource = _View.Where(aa => aa.SectionName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewSection.DataBind();
        }

        private void SaveSectionsIDs()
        {
            WMSLibrary.Filters filtersHelper = new WMSLibrary.Filters();
            FiltersModel modelTemp = Session["FiltersModel"] as FiltersModel;
            WMSLibrary.FiltersModel FM = filtersHelper.SyncGridViewIDs(GridViewSection, modelTemp, "Section");
            Session["FiltersModel"] = FM;
            //fml = Session["FiltersModel"] as FiltersModel;
        }

        private void BindGridView(string search)
        {
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewLinq(LoggedInUser);
            //List<Company> _View = da.Companies.Where(query).OrderBy(s => s.CompName).ToList();
            //GridViewCompany.DataSource = _View.Where(aa => aa.CompName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewSection.DataBind();
            
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

            //WMSLibrary.Filters.SetGridViewCheckState(GridViewCompany, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Location");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Section");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewSection, Session["FiltersModel"] as FiltersModel, "Employee");
      

        }
        #endregion 
        protected void ButtonSearchLoc_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveLocationIDs();
            BindGridViewLocation(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            //fml = Session["FiltersModel"] as FiltersModel;
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

        protected void GridViewLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewLocation.PageIndex + 1) + " of " + GridViewLocation.PageCount;
            }
        }

        private void SaveLocationIDs()
        {
            //Session["FiltersModel"] = fml;
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            Session["FiltersModel"] = FM;
            //fml = Session["FiltersModel"] as FiltersModel;
        }

        private void BindGridViewLocation(string search)
        {
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewLinq(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from Location ");
            List<Location> _View = new List<Location>();
            _View = dt.ToList<Location>().AsQueryable().SortBy("LocName").ToList();
            GridViewLocation.DataSource = _View.Where(aa => aa.LocName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewLocation.DataBind();
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
        [WebMethod(EnableSession = true)]
        public static string DeleteSingleFilter(string id, string parentid)
        {
           FiltersModel fml = new FiltersModel();
           fml = HttpContext.Current.Session["FiltersModel"] as FiltersModel;
           fml = WMSLibrary.Filters.DeleteSingleFilter(fml, id, parentid); 
           return DateTime.Now.ToString();
        }

        //[WebMethod(EnableSession = true)]
        //public static string ClearSession()
        //{
        //    HttpContext.Current.Session["FiltersModel"] = new FiltersModel();
        //    return "";
        //}

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
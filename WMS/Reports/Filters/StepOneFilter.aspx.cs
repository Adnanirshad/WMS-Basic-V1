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
            if (!Page.IsPostBack)
            {
                // Bind Grid View According to Filters
                BindGridView("");

                BindGridViewDivision("");
                List<string> list = Session["ReportSession"] as List<string>;
                dateFrom.Value = list[0];
                dateTo.Value = list[1];
            }
            else
            {

                SaveRegionIDs();
                SaveDivisionIDs();

            }
            if (Session["FiltersModel"] != null)
            {
                //1.Write a function that checks the checkbox state 
                //2.Save the unchecked checkboxes to the session



                // Check and Uncheck Items in grid view according to Session Filters Model

                WMSLibrary.Filters.SetGridViewCheckState(GridViewRegion, Session["FiltersModel"] as FiltersModel, "Region");
                WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");

            }
        }

        #region --GridView Company--
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string date = dateFrom.Value.ToString();
            // Save selected Company ID and Name in Session

                           SaveRegionIDs();

            BindGridView(TextBoxSearch.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewRegion, Session["FiltersModel"] as FiltersModel, "Region");
            //fml = Session["FiltersModel"] as FiltersModel;
        }

        protected void GridViewRegion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveRegionIDs();

            //change page index
           GridViewRegion.PageIndex = e.NewPageIndex;
            BindGridView(TextBoxSearch.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewRegion, Session["FiltersModel"] as FiltersModel, "Region");
        }

        protected void GridViewRegion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewRegion.PageIndex + 1) + " of " + GridViewRegion.PageCount;
            }
        }

        private void SaveRegionIDs()
        {
            WMSLibrary.Filters filtersHelper = new WMSLibrary.Filters();
            FiltersModel modelTemp = Session["FiltersModel"] as FiltersModel;
            WMSLibrary.FiltersModel FM = filtersHelper.SyncGridViewIDs(GridViewRegion, modelTemp, "");

            Session["FiltersModel"] = FM;
            //fml = Session["FiltersModel"] as FiltersModel;
        }

        private void BindGridView(string search)
        {   
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewLinq(LoggedInUser);
            

         GridViewRegion.DataBind();

        }

        #endregion
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

<<<<<<< HEAD
            WMSLibrary.Filters.SetGridViewCheckState(GridViewRegion, Session["FiltersModel"] as FiltersModel, "Region");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Division");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Shift");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Type");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Department");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Type");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Section");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Crew");
            //WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Employee");
=======
            WMSLibrary.Filters.SetGridViewCheckState(GridViewCompany, Session["FiltersModel"] as FiltersModel, "Company");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Location");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Division");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Shift");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Department");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Type");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Section");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Crew");
            WMSLibrary.Filters.SetGridViewCheckState(GridViewLocation, Session["FiltersModel"] as FiltersModel, "Employee");
>>>>>>> 92c8efc907499465a40b438e8fab917f49c589c4
      

        }
        #endregion 
        #region --GridView Location--
        protected void ButtonSearchLoc_Click(object sender, EventArgs e)
        {
            // Save selected Company ID and Name in Session
<<<<<<< HEAD
            SaveDivisionIDs();
            BindGridViewDivision(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
            //fml = Session["FiltersModel"] as FiltersModel;
        }

        protected void GridViewDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Save selected Company ID and Name in Session
            SaveDivisionIDs();

            //change page index
           GridViewDivision.PageIndex = e.NewPageIndex;
            BindGridViewDivision(tbSearch_Location.Text.Trim());
            // Check and set Check box state
            WMSLibrary.Filters.SetGridViewCheckState(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
        }

        protected void GridViewDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Page " + (GridViewDivision.PageIndex + 1) + " of " +GridViewDivision.PageCount;
            }
        }

        private void SaveDivisionIDs()
        {
            //Session["FiltersModel"] = fml;
            WMSLibrary.Filters filterHelper = new WMSLibrary.Filters();
            WMSLibrary.FiltersModel FM = filterHelper.SyncGridViewIDs(GridViewDivision, Session["FiltersModel"] as FiltersModel, "Division");
=======
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
>>>>>>> 92c8efc907499465a40b438e8fab917f49c589c4
            Session["FiltersModel"] = FM;
            //fml = Session["FiltersModel"] as FiltersModel;
        }

<<<<<<< HEAD
        private void BindGridViewDivision(string search)
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<ViewDivision> _View = new List<ViewDivision>();
            List<ViewDivision> _TempView = new List<ViewDivision>();
            User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
           // string query = qb.QueryForCompanyViewLinq(LoggedInUser);

           _View = da.ViewDivisions.ToList();
            //if (fm.CompanyFilter.Count > 0)
            //{
            //    foreach (var comp in fm.CompanyFilter)
            //    {
            //        short _compID = Convert.ToInt16(comp.ID);
            //        _TempView.AddRange(_View.Where(aa => aa.CompID == _compID).ToList());
            //    }
            //    _View = _TempView.ToList();
            //}
            GridViewDivision.DataSource = _View.Where(aa => aa.DivisionName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewDivision.DataBind();
=======
        private void BindGridViewLocation(string search)
        {
            //User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
            //QueryBuilder qb = new QueryBuilder();
            //string query = qb.QueryForLocationTableSegerationForLinq(LoggedInUser);
            //List<Location> _View = da.Locations.Where(query).OrderBy(s=>s.LocName).ToList();
            GridViewLocation.DataSource = da.Locations.Where(aa => aa.LocName.ToUpper().Contains(search.ToUpper())).ToList();
            GridViewLocation.DataBind();
>>>>>>> 92c8efc907499465a40b438e8fab917f49c589c4
        }

        #endregion

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
<<<<<<< HEAD
           SaveRegionIDs();
            SaveDivisionIDs();
=======
            SaveCompanyIDs();
            SaveLocationIDs();
>>>>>>> 92c8efc907499465a40b438e8fab917f49c589c4
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
<<<<<<< HEAD
            SaveRegionIDs();
            SaveDivisionIDs();
=======
            SaveCompanyIDs();
            SaveLocationIDs();
>>>>>>> 92c8efc907499465a40b438e8fab917f49c589c4
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            if (MyHelper.UserHasValuesInSession(fm))
            {
                Response.Redirect("~/Reports/Filters/StepSixFilter.aspx");
            }
        }


        #endregion





    }
}
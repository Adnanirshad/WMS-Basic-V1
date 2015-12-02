using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using WMS.CustomClass;
using WMS.Models;

namespace WMS.Reports
{
    public partial class YLvConsumed : System.Web.UI.Page
    {
        string PathString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DivGridSection.Visible = false;
                DivGridCrew.Visible = false;
                DivGridDept.Visible = false;
                DivGridEmp.Visible = false;
                DivShiftGrid.Visible = false;
                DivLocGrid.Visible = false;
                DivTypeGrid.Visible = false;
                ReportViewer1.Visible = true;
                ReportViewer1.Width = 1050;
                ReportViewer1.Height = 700;
                SelectedTypes.Clear();
                SelectedCrews.Clear();
                SelectedDepts.Clear();
                SelectedEmps.Clear();
                SelectedLocs.Clear();
                SelectedSections.Clear();
                SelectedShifts.Clear();
                RefreshLabels();
                LoadGridViews();
                if (GlobalVariables.DeploymentType == false)
                {
                    PathString = "/Reports/RDLC/YLvConsumed.rdlc";
                }
                else
                    PathString = "/WMS/Reports/RDLC/YLvConsumed.rdlc";
                User LoggedInUser = HttpContext.Current.Session["LoggedUser"] as User;
                QueryBuilder qb = new QueryBuilder();
                string query = qb.MakeCustomizeQuery(LoggedInUser);
                DataTable dt = qb.GetValuesfromDB("select * from ViewLvConsumed " + query);
                List<ViewLvConsumed> _ViewList = dt.ToList<ViewLvConsumed>();
                LoadReport(PathString, _ViewList);
            }
        }
        protected void grid_Employee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gridView = (GridView)sender;

            if (gridView.SortExpression.Length > 0)
            {
                int cellIndex = -1;
                //  find the column index for the sorresponding sort expression
                foreach (DataControlField field in gridView.Columns)
                {
                    if (field.SortExpression == gridView.SortExpression)
                    {
                        cellIndex = gridView.Columns.IndexOf(field);
                        break;
                    }
                }

                if (cellIndex > -1)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        //  this is a header row,
                        //  set the sort style
                        e.Row.Cells[cellIndex].CssClass +=
                            (gridView.SortDirection == SortDirection.Ascending
                            ? " sortascheader" : " sortdescheader");
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //  this is a data row
                        e.Row.Cells[cellIndex].CssClass +=
                            (e.Row.RowIndex % 2 == 0
                            ? " sortaltrow" : "sortrow");
                    }
                }
            }
        }
        #region --Load GridViews --
        private void LoadGridViews()
        {
            User _loggedUser = HttpContext.Current.Session["LoggedUser"] as User;
            LoadEmpView(_loggedUser);
            LoadShiftView(_loggedUser);
            LoadLocationView(_loggedUser);
            LoadEmpTypeView(_loggedUser);
        }

        private void LoadEmpTypeView(User _loggedUser)
        {
            //string connectionString = WebConfigurationManager.ConnectionStrings["TAS2013ConnectionString"].ConnectionString;
            //string selectSQL = "";
            //string _Query = "";
            List<EmpType> _empType = new List<EmpType>();
            if (_loggedUser.ViewContractual == true && _loggedUser.ViewPermanentMgm == false && _loggedUser.ViewPermanentStaff == false)
            {
                _empType = context.EmpTypes.Where(aa => aa.CatID == 3).ToList();
            }
            else if (_loggedUser.ViewContractual == false && _loggedUser.ViewPermanentMgm == true && _loggedUser.ViewPermanentStaff == false)
            {
                _empType = context.EmpTypes.Where(aa => aa.CatID == 2).ToList();
            }
            else if (_loggedUser.ViewContractual == false && _loggedUser.ViewPermanentMgm == false && _loggedUser.ViewPermanentStaff == true)
            {
                _empType = context.EmpTypes.Where(aa => aa.CatID == 2).ToList();
            }
            else
            {
                _empType = context.EmpTypes.ToList();
            }
            //_Query = "SELECT * FROM TAS2013.dbo.EmpType where " + selectSQL;
            //grid_EmpType.DataSource = GetValuesFromDatabase(_Query, "EmpType");
            //grid_EmpType.DataBind();
            grid_EmpType.DataSource = _empType;
            grid_EmpType.DataBind();
        }

        private void LoadLocationView(User _loggedUser)
        {

        }

        private void LoadShiftView(User _loggedUser)
        {

        }
        private void LoadEmpView(User _loggedUser)
        {
            QueryBuilder qb = new QueryBuilder();
            string query = qb.MakeCustomizeQuery(_loggedUser);
            DataTable dt = qb.GetValuesfromDB("select * from EmpView " + query);
            List<EmpView> _View = dt.ToList<EmpView>();
            grid_Employee.DataSource = _View;
            grid_Employee.DataBind();


        }

        //private DataSet GetValuesFromDatabase(string _query, string _tableName)
        //{
        //    string connectionString = WebConfigurationManager.ConnectionStrings["TAS2013ConnectionString"].ConnectionString;
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand(_query, con);
        //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();

        //    adapter.Fill(ds, _tableName);
        //    return ds;
        //}
        #endregion
        #region --Filters Button--
        protected void btnEmployeeGrid_Click(object sender, EventArgs e)
        {
            DivGridSection.Visible = false;
            DivGridCrew.Visible = false;
            DivGridDept.Visible = false;
            DivShiftGrid.Visible = false;
            DivLocGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivGridEmp.Visible = true;
            RefreshLabels();
        }

        protected void btnSectionGrid_Click(object sender, EventArgs e)
        {
            DivGridCrew.Visible = false;
            DivGridDept.Visible = false;
            DivGridEmp.Visible = false;
            DivShiftGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivLocGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivGridSection.Visible = true;
            RefreshLabels();
        }

        protected void btnDepartmentGrid_Click(object sender, EventArgs e)
        {
            DivGridSection.Visible = false;
            DivGridCrew.Visible = false;
            DivGridEmp.Visible = false;
            DivShiftGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivLocGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivGridDept.Visible = true;
            RefreshLabels();
        }

        protected void btnCrewGrid_Click(object sender, EventArgs e)
        {
            DivGridSection.Visible = false;
            DivGridDept.Visible = false;
            DivGridEmp.Visible = false;
            DivShiftGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivLocGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivGridCrew.Visible = true;
            RefreshLabels();
        }

        protected void btnShiftGrid_Click(object sender, EventArgs e)
        {
            DivGridSection.Visible = false;
            DivGridCrew.Visible = false;
            DivGridDept.Visible = false;
            DivGridEmp.Visible = false;
            ReportViewer1.Visible = false;
            DivLocGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivShiftGrid.Visible = true;
            RefreshLabels();
        }
        protected void btnLoc_Click(object sender, EventArgs e)
        {
            DivGridSection.Visible = false;
            DivGridCrew.Visible = false;
            DivGridDept.Visible = false;
            DivGridEmp.Visible = false;
            ReportViewer1.Visible = false;
            DivShiftGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivLocGrid.Visible = true;
            RefreshLabels();
        }
        protected void btnEmpType_Click(object sender, EventArgs e)
        {
            DivGridSection.Visible = false;
            DivGridCrew.Visible = false;
            DivGridDept.Visible = false;
            DivGridEmp.Visible = false;
            ReportViewer1.Visible = false;
            DivShiftGrid.Visible = false;
            DivLocGrid.Visible = false;
            ReportViewer1.Visible = false;
            DivTypeGrid.Visible = true;
            RefreshLabels();
        }
        #endregion

        #region --ViewStates--

        protected List<Section> SelectedSections
        {
            get
            {
                if (this.Session["SelectedSection"] == null)
                    this.Session["SelectedSection"] = new List<Section>();
                return (List<Section>)this.Session["SelectedSection"];
            }
            set
            {
                Session["SelectedSection"] = value;
            }
        }

        protected List<Department> SelectedDepts
        {
            get
            {
                if (this.Session["SelectedDept"] == null)
                    this.Session["SelectedDept"] = new List<Department>();
                return (List<Department>)this.Session["SelectedDept"];
            }
            set
            {
                Session["SelectedDept"] = value;
            }
        }

        protected List<Shift> SelectedShifts
        {
            get
            {
                if (this.Session["SelectedShifts"] == null)
                    this.Session["SelectedShifts"] = new List<Shift>();
                return (List<Shift>)this.Session["SelectedShifts"];
            }
            set
            {
                Session["SelectedSection"] = value;
            }
        }

        protected List<Crew> SelectedCrews
        {
            get
            {
                if (this.Session["SelectedCrew"] == null)
                    this.Session["SelectedCrew"] = new List<Crew>();
                return (List<Crew>)this.Session["SelectedCrew"];
            }
            set
            {
                Session["SelectedCrew"] = value;
            }
        }

        protected List<EmpView> SelectedEmps
        {
            get
            {
                if (this.Session["SelectedEmp"] == null)
                    this.Session["SelectedEmp"] = new List<EmpView>();
                return (List<EmpView>)this.Session["SelectedEmp"];
            }
            set
            {
                Session["SelectedEmp"] = value;
            }
        }

        protected List<Location> SelectedLocs
        {
            get
            {
                if (this.Session["SelectedLoc"] == null)
                    this.Session["SelectedLoc"] = new List<Location>();
                return (List<Location>)this.Session["SelectedLoc"];
            }
            set
            {
                Session["SelectedLoc"] = value;
            }
        }
        protected List<EmpType> SelectedTypes
        {
            get
            {
                if (this.Session["SelectedCat"] == null)
                    this.Session["SelectedCat"] = new List<EmpType>();
                return (List<EmpType>)this.Session["SelectedCat"];
            }
            set
            {
                Session["SelectedCat"] = value;
            }
        }
        #endregion

        private void RefreshLabels()
        {
            lbSectionCount.Text = "Selected Sections : " + SelectedSections.Count.ToString();
            lbEmpCount.Text = "Selected Employees : " + SelectedEmps.Count.ToString();
            lbDeptCount.Text = "Selected Departments : " + SelectedDepts.Count.ToString();
            lbCrewCount.Text = "Selected Crews : " + SelectedCrews.Count.ToString();
            lbShiftCount.Text = "Selected Shifts : " + SelectedShifts.Count.ToString();
            lbLocCount.Text = "Selected Locations : " + SelectedLocs.Count.ToString();
            lbCatCount.Text = "Selected Types : " + SelectedTypes.Count.ToString();
        }

        #region --CheckBoxes Changed Events --
        //for sections
        protected void chkCtrlSection_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_Section.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlSection"));
                Section _sec = new Section();
                _sec.SectionID = Convert.ToInt16(row.Cells[1].Text);
                _sec.SectionName = row.Cells[2].Text;
                if (ck.Checked)
                {
                    if (SelectedSections.Where(sec => sec.SectionID == _sec.SectionID).Count() == 0)
                        SelectedSections.Add(_sec);
                }
                else
                {
                    if (SelectedSections.Where(sec => sec.SectionID == _sec.SectionID).Count() > 0)
                    {
                        var _section = SelectedSections.Where(sec => sec.SectionID == _sec.SectionID).First();
                        SelectedSections.Remove(_section);
                    }
                }
            }
        }

        //for departments
        protected void chkCtrlDept_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_Dept.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlDept"));
                Department _dept = new Department();
                _dept.DeptID = Convert.ToInt16(row.Cells[1].Text);
                _dept.DeptName = row.Cells[2].Text;
                if (ck.Checked)
                {
                    if (SelectedDepts.Where(aa => aa.DeptID == _dept.DeptID).Count() == 0)
                        SelectedDepts.Add(_dept);
                }
                else
                {
                    if (SelectedDepts.Where(aa => aa.DeptID == _dept.DeptID).Count() > 0)
                    {
                        var dept = SelectedDepts.Where(aa => aa.DeptID == _dept.DeptID).First();
                        SelectedDepts.Remove(dept);
                    }
                }
            }
        }

        //for shift
        protected void chkCtrlShift_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_Shift.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlShift"));
                Shift _Shift = new Shift();
                _Shift.ShiftID = Convert.ToByte(row.Cells[1].Text);
                _Shift.ShiftName = row.Cells[2].Text;
                if (ck.Checked)
                {
                    if (SelectedShifts.Where(aa => aa.ShiftID == _Shift.ShiftID).Count() == 0)
                        SelectedShifts.Add(_Shift);
                }
                else
                {
                    if (SelectedShifts.Where(aa => aa.ShiftID == _Shift.ShiftID).Count() > 0)
                    {
                        var shift = SelectedShifts.Where(aa => aa.ShiftID == _Shift.ShiftID).First();
                        SelectedShifts.Remove(shift);
                    }
                }
            }
        }
        //For employees
        protected void chkCtrlEmp_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_Employee.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlEmp"));
                EmpView _EmpView = new EmpView();
                _EmpView.EmpID = Convert.ToInt32(row.Cells[1].Text);
                _EmpView.EmpName = row.Cells[3].Text;
                if (ck.Checked)
                {
                    if (SelectedEmps.Where(aa => aa.EmpID == _EmpView.EmpID).Count() == 0)
                        SelectedEmps.Add(_EmpView);
                }
                else
                {
                    if (SelectedEmps.Where(aa => aa.EmpID == _EmpView.EmpID).Count() > 0)
                    {
                        var _emp = SelectedEmps.Where(aa => aa.EmpID == _EmpView.EmpID).First();
                        SelectedEmps.Remove(_emp);
                    }
                }
            }
        }
        //For crew
        protected void chkCtrlCrew_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_Crew.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlCrew"));
                Crew _crew = new Crew();
                _crew.CrewID = Convert.ToInt16(row.Cells[1].Text);
                _crew.CrewName = row.Cells[2].Text;
                if (ck.Checked)
                {
                    if (SelectedCrews.Where(aa => aa.CrewID == _crew.CrewID).Count() == 0)
                        SelectedCrews.Add(_crew);
                }
                else
                {
                    if (SelectedCrews.Where(aa => aa.CrewID == _crew.CrewID).Count() > 0)
                    {
                        var crew = SelectedCrews.Where(aa => aa.CrewID == _crew.CrewID).First();
                        SelectedCrews.Remove(crew);
                    }
                }
            }
        }
        //for locations
        protected void chkCtrlLoc_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_Location.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlLoc"));
                Location _loc = new Location();
                _loc.LocID = Convert.ToInt16(row.Cells[1].Text);
                _loc.LocName = row.Cells[2].Text;
                if (ck.Checked)
                {
                    if (SelectedLocs.Where(aa => aa.LocID == _loc.LocID).Count() == 0)
                        SelectedLocs.Add(_loc);
                }
                else
                {
                    if (SelectedLocs.Where(aa => aa.LocID == _loc.LocID).Count() > 0)
                    {
                        var loc = SelectedLocs.Where(aa => aa.LocID == _loc.LocID).First();
                        SelectedLocs.Remove(loc);
                    }
                }
            }
        }
        //for Types
        protected void chkCtrlType_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grid_EmpType.Rows)
            {
                CheckBox ck = ((CheckBox)row.FindControl("chkCtrlType"));
                EmpType _Cat = new EmpType();
                _Cat.TypeID = Convert.ToByte(row.Cells[1].Text);
                _Cat.TypeName = row.Cells[2].Text;
                if (ck.Checked)
                {
                    if (SelectedTypes.Where(aa => aa.TypeID == _Cat.TypeID).Count() == 0)
                        SelectedTypes.Add(_Cat);
                }
                else
                {
                    if (SelectedTypes.Where(aa => aa.TypeID == _Cat.TypeID).Count() > 0)
                    {
                        var cat = SelectedTypes.Where(aa => aa.TypeID == _Cat.TypeID).First();
                        SelectedTypes.Remove(cat);
                    }
                }
            }
        }
        #endregion

        TAS2013Entities context = new TAS2013Entities();
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            RefreshLabels();
            DivGridSection.Visible = false;
            DivGridCrew.Visible = false;
            DivGridDept.Visible = false;
            DivGridEmp.Visible = false;
            DivShiftGrid.Visible = false;
            DivLocGrid.Visible = false;
            DivTypeGrid.Visible = false;
            ReportViewer1.Visible = true;
            List<ViewLvConsumed> _ViewList = new List<ViewLvConsumed>();
            List<ViewLvConsumed> _TempViewList = new List<ViewLvConsumed>();
            User _loggedUser = HttpContext.Current.Session["LoggedUser"] as User;
            if (_loggedUser.ViewContractual == true && _loggedUser.ViewPermanentMgm == false && _loggedUser.ViewPermanentStaff == false)
            {
                _ViewList = context.ViewLvConsumeds.Where(aa => aa.CatID == 3).ToList();
            }
            else if (_loggedUser.ViewContractual == false && _loggedUser.ViewPermanentMgm == true && _loggedUser.ViewPermanentStaff == false)
            {
                _ViewList = context.ViewLvConsumeds.Where(aa => aa.CatID == 2).ToList();
            }
            else if (_loggedUser.ViewContractual == false && _loggedUser.ViewPermanentMgm == false && _loggedUser.ViewPermanentStaff == true)
            {
                _ViewList = context.ViewLvConsumeds.Where(aa => aa.CatID == 2).ToList();
            }
            else
            {
                _ViewList = context.ViewLvConsumeds.ToList();
            }
            if (SelectedEmps.Count > 0)
            {
                foreach (var emp in SelectedEmps)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.EmpNo == emp.EmpNo).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();


            _TempViewList.Clear();
            //for department
            if (SelectedDepts.Count > 0)
            {
                foreach (var dept in SelectedDepts)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.DeptName == dept.DeptName).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for sections
            if (SelectedSections.Count > 0)
            {
                foreach (var sec in SelectedSections)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.SectionName == sec.SectionName).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();


            _TempViewList.Clear();

            //for crews
            if (SelectedCrews.Count > 0)
            {
                foreach (var cre in SelectedCrews)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.CrewName == cre.CrewName).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for location
            if (SelectedLocs.Count > 0)
            {
                foreach (var loc in SelectedLocs)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.LocName == loc.LocName).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();

            //for shifts
            if (SelectedShifts.Count > 0)
            {
                foreach (var shift in SelectedShifts)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.ShiftName == shift.ShiftName).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();


            _TempViewList.Clear();

            //for category
            if (SelectedTypes.Count > 0)
            {
                foreach (var cat in SelectedTypes)
                {
                    _TempViewList.AddRange(_ViewList.Where(aa => aa.TypeName == cat.TypeName).ToList());
                }
                _ViewList = _TempViewList.ToList();
            }
            else
                _TempViewList = _ViewList.ToList();
            _TempViewList.Clear();
            if (GlobalVariables.DeploymentType == false)
            {
                PathString = "/Reports/RDLC/YLvConsumed.rdlc";
            }
            else
                PathString = "/WMS/Reports/RDLC/YLvConsumed.rdlc";
            LoadReport(PathString, _ViewList);
        }
        //public DateTime DateFrom
        //{
        //    get
        //    {
        //        if (dateFrom.Value == "")
        //            return DateTime.Today.Date.AddDays(-1);
        //        else
        //            return DateTime.Parse(dateFrom.Value);
        //    }
        //}
        //public DateTime DateTo
        //{
        //    get
        //    {
        //        if (dateTo.Value == "")
        //            return DateTime.Today.Date.AddDays(-1);
        //        else
        //            return DateTime.Parse(dateTo.Value);
        //    }
        //}
        private void LoadReport(string path, List<ViewLvConsumed> _Employee)
        {
            string _Header = context.Options.FirstOrDefault().CompanyName + " - Anual Leave Report";
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);
            System.Security.PermissionSet sec = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
            ReportViewer1.LocalReport.SetBasePermissionsForSandboxAppDomain(sec);
            IEnumerable<ViewLvConsumed> ie;
            ie = _Employee.AsQueryable();
            ReportDataSource datasource1 = new ReportDataSource("DataSet1", ie);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource1);
            ReportParameter rp = new ReportParameter("Header", _Header, false);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
            ReportViewer1.LocalReport.Refresh();
        }
    }
}
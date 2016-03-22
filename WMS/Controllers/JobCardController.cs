using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WMS.Models;
using WMS.CustomClass;
using System.Data;
using System.Net;
namespace WMS.Controllers
{
    public class JobCardController : Controller
    {
        //
        // GET: /JobCard/
        public ActionResult Index()
        {
            return RedirectToAction("JCCreate");
        }
        private TAS2013Entities db = new TAS2013Entities();

        public ActionResult JCCreate(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DesigSortParm = sortOrder == "designation" ? "designation_desc" : "designation";
            ViewBag.LocSortParm = sortOrder == "location" ? "location_desc" : "location";
            ViewBag.SectionSortParm = sortOrder == "section" ? "section_desc" : "section";
            ViewBag.DepartmentSortParm = sortOrder == "wing" ? "wing_desc" : "wing";
            ViewBag.ShiftSortParm = sortOrder == "shift" ? "shift_desc" : "shift";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
            //List<EmpView> emps = new List<EmpView>();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            User LoggedInUser = Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.MakeCustomizeQuery(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from EmpView " + query);
            List<EmpView> emps1 = dt.ToList<EmpView>();

            // List<EmpView> emps = db.EmpViews.ToList();
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString == "Active" || searchString == "active")
                {
                    emps1 = emps1.Where(aa => aa.Status == true).ToList();
                }
                else if (searchString == "Inactive" || searchString == "inactive")
                {
                    emps1 = emps1.Where(aa => aa.Status == false).ToList();
                }
                else
                {
                    try
                    {

                        emps1 = emps1.Where(s => s.EmpName.ToUpper().Contains(searchString.ToUpper())
                         || s.EmpNo.ToUpper().Contains(searchString.ToUpper()) || s.DesignationName.ToString().Contains(searchString)
                        ).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    emps1 = emps1.OrderByDescending(s => s.EmpName).ToList();
                    break;
                case "designation_desc":
                    emps1 = emps1.OrderByDescending(s => s.DesignationName).ToList();
                    break;
                case "designation":
                    emps1 = emps1.OrderBy(s => s.DesignationName).ToList();
                    break;
                case "location_desc":
                    emps1 = emps1.OrderByDescending(s => s.LocName).ToList();
                    break;
                case "location":
                    emps1 = emps1.OrderBy(s => s.LocName).ToList();
                    break;
                case "section_desc":
                    emps1 = emps1.OrderByDescending(s => s.SectionName).ToList();
                    break;
                case "section":
                    emps1 = emps1.OrderBy(s => s.SectionName).ToList();
                    break;
                case "wing_desc":
                    emps1 = emps1.OrderByDescending(s => s.DeptName).ToList();
                    break;
                case "wing":
                    emps1 = emps1.OrderBy(s => s.DeptName).ToList();
                    break;
                case "shift_desc":
                    emps1 = emps1.OrderByDescending(s => s.ShiftName).ToList();
                    break;
                case "shift":
                    emps1 = emps1.OrderBy(s => s.ShiftName).ToList();
                    break;
                case "type_desc":
                    emps1 = emps1.OrderByDescending(s => s.TypeName).ToList();
                    break;
                case "type":
                    emps1 = emps1.OrderBy(s => s.TypeName).ToList();
                    break;
                default:
                    emps1 = emps1.OrderBy(s => s.EmpName).ToList();
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(emps1.ToPagedList(pageNumber, pageSize));



           
        }

        [HttpPost]
        public ActionResult EditAttJobCard()
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            string Message = "";
            QueryBuilder qb = new QueryBuilder();
            string query = qb.MakeCustomizeQuery(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from EmpView " + query);
            List<EmpView> emps = dt.ToList<EmpView>();
            try
            {
                string _EmpNo = "";
                // int CompID = Convert.ToInt16(Request.Form["CompanyID"].ToString());
                List<EmpView> _Emp = new List<EmpView>();
                short _WorkCardID = Convert.ToInt16(Request.Form["JobCardType"].ToString());
                //First Save Job Card Application
                JobCardApp jobCardApp = new JobCardApp();
                jobCardApp.CardType = _WorkCardID;
                jobCardApp.DateCreated = DateTime.Now;
                jobCardApp.DateStarted = Convert.ToDateTime(Request.Form["JobDateFrom"]);
                jobCardApp.DateEnded = Convert.ToDateTime(Request.Form["JobDateTo"]);
                jobCardApp.Status = false;
                string Remakrs = Request.Form["Remakrs"].ToString();
                if (Remakrs != "")
                    jobCardApp.Remarks = Remakrs;
                jobCardApp.UserID = LoggedInUser.UserID;
                _EmpNo = Request.Form["JobEmpNo"];
                _Emp = emps.Where(aa => aa.EmpNo == _EmpNo).ToList();
                if (_Emp.Count > 0)
                {
                    jobCardApp.CriteriaDate = _Emp.FirstOrDefault().EmpID;
                    jobCardApp.JobCardCriteria = "E";
                    if (ValidateJobCard(jobCardApp))
                    {
                        db.JobCardApps.Add(jobCardApp);
                        if (db.SaveChanges() > 0)
                        {
                            AddJobCardAppToJobCardData();
                            Message = "Job Card Created Sucessfully";
                        }
                        else
                            Message = "Job Card is not created due to server error";
                    }
                    else
                        Message = "Job Card already created for FPID: " + _EmpNo;
                }
                else
                    Message = "No Employee found, Please enter a valid FPID";
                //Add Job Card to JobCardData and Mark Legends in Attendance Data if attendance Created
                Session["EditAttendanceDate"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                ViewData["datef"] = Session["EditAttendanceDate"].ToString();
                ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
            }
            catch (Exception ex)
            {
                Message = "An Error occured while creating Job Card of " + Request.Form["JobCardType"].ToString();
            }
            //List<EmpView> emps = new List<EmpView>();
            ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
            ViewBag.CMessage = Message;
            ViewBag.CurrentFilter = "";
            int pageSize = 10;
            int? page = 1;
            int pageNumber = (page ?? 1);
            return View("Index", emps.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult JobCardList(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DesigSortParm = sortOrder == "designation" ? "designation_desc" : "designation";
            ViewBag.SectionSortParm = sortOrder == "section" ? "section_desc" : "section";
            //List<EmpView> emps = new List<EmpView>();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            User LoggedInUser = Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.MakeCustomizeQuery(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from ViewJobCardApp " + query);
            List<ViewJobCardApp> jobCardsApps = dt.ToList<ViewJobCardApp>();

            // List<EmpView> emps = db.EmpViews.ToList();
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {

                    jobCardsApps = jobCardsApps.Where(s => s.EmpName.ToUpper().Contains(searchString.ToUpper())
                        || s.EmpNo.ToUpper().Contains(searchString.ToUpper()) || s.DesignationName.ToString().Contains(searchString)
                    ).OrderByDescending(aa => aa.DateCreated).ToList();
                }
                catch (Exception ex)
                {

                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    jobCardsApps = jobCardsApps.OrderByDescending(s => s.EmpName).ToList();
                    break;
                case "designation_desc":
                    jobCardsApps = jobCardsApps.OrderByDescending(s => s.DesignationName).ToList();
                    break;
                case "designation":
                    jobCardsApps = jobCardsApps.OrderBy(s => s.DesignationName).ToList();
                    break;
                case "section_desc":
                    jobCardsApps = jobCardsApps.OrderByDescending(s => s.SectionName).ToList();
                    break;
                case "section":
                    jobCardsApps = jobCardsApps.OrderBy(s => s.SectionName).ToList();
                    break;
                default:
                    jobCardsApps = jobCardsApps.OrderBy(s => s.EmpName).ToList();
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(jobCardsApps.ToPagedList(pageNumber, pageSize));
        }
        private bool ValidateJobCard(JobCardApp jobCardApp)
        {
            List<JobCardApp> _Lc = new List<JobCardApp>();
            DateTime _DTime = new DateTime();
            DateTime _DTimeLV = new DateTime();
            _Lc = db.JobCardApps.Where(aa => aa.CriteriaDate == jobCardApp.CriteriaDate).ToList();
            foreach (var item in _Lc)
            {
                _DTime = (DateTime)item.DateStarted;
                _DTimeLV = (DateTime)jobCardApp.DateStarted;
                while (_DTime <= item.DateEnded)
                {
                    while (_DTimeLV <= jobCardApp.DateEnded)
                    {
                        if (_DTime == _DTimeLV)
                            return false;
                        _DTimeLV = _DTimeLV.AddDays(1);
                    }
                    _DTime = _DTime.AddDays(1);
                }
            }
            return true;
        }
        private void AddJobCardAppToJobCardData()
        {
            List<JobCardApp> _jobCardApp = new List<JobCardApp>();
            _jobCardApp = db.JobCardApps.Where(aa => aa.Status == false).ToList();
            List<Emp> _Emp = new List<Emp>();
            foreach (var jcApp in _jobCardApp)
            {
                jcApp.Status = true;
                switch (jcApp.JobCardCriteria)
                {

                    case "E":
                        int _EmpID = (int)jcApp.CriteriaDate;
                        _Emp = db.Emps.Where(aa => aa.EmpID == _EmpID).ToList();
                        break;
                }
                foreach (var selectedEmp in _Emp)
                {
                    AddJobCardData(selectedEmp, jcApp);
                }
            }
        }


        private bool AddJobCardDataToDatabase(string _empDate, int _empID, DateTime _currentDate, int _userID, JobCardApp jcApp)
        {
            bool check = false;
            try
            {
                JobCardDetail _jobCardEmp = new JobCardDetail();
                _jobCardEmp.EmpDate = _empDate;
                _jobCardEmp.EmpID = _empID;
                _jobCardEmp.Dated = _currentDate;
                //_jobCardEmp.SubmittedFrom = _userID;
                _jobCardEmp.WrkCardID = jcApp.CardType;
                //_jobCardEmp.DateCreated = DateTime.Now;
                //_jobCardEmp.WorkMin = jcApp.WorkMin;
                //_jobCardEmp.AppID = jcApp.JobCardID;
                _jobCardEmp.Remarks = jcApp.Remarks;
                db.JobCardDetails.Add(_jobCardEmp);
                if (db.SaveChanges() > 0)
                {
                    check = true;
                }
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        private void AddJobCardData(Emp _selEmp, JobCardApp jcApp)
        {
            int _empID = _selEmp.EmpID;
            string _empDate = "";
            int _userID = (int)jcApp.UserID;
            DateTime _Date = (DateTime)jcApp.DateStarted;
            while (_Date <= jcApp.DateEnded)
            {
                _empDate = _selEmp.EmpID + _Date.ToString("yyMMdd");
                AddJobCardDataToDatabase(_empDate, _empID, _Date, _userID, jcApp);
                if (db.AttProcesses.Where(aa => aa.ProcessDate == _Date).Count() > 0)
                {
                    switch (jcApp.CardType)
                    {

                        case 8:// Training
                            AddJCTrainingToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
                            break;
                        case 9:// Tour
                            AddJCTourToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
                            break;
                        case 10:// Visit
                            AddJCVisitToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
                            break;
                        case 11:// Official Assignment
                            AddJCOAToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
                            break;
                    }
                }
                _Date = _Date.AddDays(1);
            }
        }

        #region --Job Cards - AttData ---
        private bool AddJCNorrmalDayAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == _WorkCardID);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.WorkMin = _jcCard.WorkMin;
                        _attdata.ShifMin = _jcCard.WorkMin;
                        _attdata.Remarks = "[Present][Manual]";
                        _attdata.TimeIn = null;
                        _attdata.TimeOut = null;
                        _attdata.EarlyIn = null;
                        _attdata.EarlyOut = null;
                        _attdata.LateIn = null;
                        _attdata.LateOut = null;
                        _attdata.OTMin = null;
                        _attdata.StatusEI = null;
                        _attdata.StatusEO = null;
                        _attdata.StatusLI = null;
                        _attdata.StatusLO = null;
                        _attdata.StatusP = true;
                    }
                    context.SaveChanges();
                    if (context.SaveChanges() > 0)
                        check = true;
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }

        private bool AddJCVisitToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == _WorkCardID);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.WorkMin = _attdata.ShifMin;
                        _attdata.Remarks = "[OD-Visit]";
                        _attdata.EarlyIn = null;
                        _attdata.EarlyOut = null;
                        _attdata.LateIn = null;
                        _attdata.LateOut = null;
                        _attdata.OTMin = null;
                        _attdata.StatusEI = null;
                        _attdata.StatusEO = null;
                        _attdata.StatusLI = null;
                        _attdata.StatusLO = null;
                        _attdata.StatusP = true;
                    }
                    context.SaveChanges();
                    if (context.SaveChanges() > 0)
                        check = true;
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }
        private bool AddJCOAToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == _WorkCardID);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.WorkMin = _attdata.ShifMin;
                        _attdata.Remarks = "[Oficial Assignment]";
                        _attdata.EarlyIn = null;
                        _attdata.EarlyOut = null;
                        _attdata.LateIn = null;
                        _attdata.LateOut = null;
                        _attdata.OTMin = null;
                        _attdata.StatusEI = null;
                        _attdata.StatusEO = null;
                        _attdata.StatusLI = null;
                        _attdata.StatusLO = null;
                        _attdata.StatusP = true;
                    }
                    context.SaveChanges();
                    if (context.SaveChanges() > 0)
                        check = true;
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }
        private bool AddJCTourToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == _WorkCardID);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.WorkMin = _attdata.ShifMin;
                        _attdata.Remarks = "[OD-Tour]";
                        _attdata.EarlyIn = null;
                        _attdata.EarlyOut = null;
                        _attdata.LateIn = null;
                        _attdata.LateOut = null;
                        _attdata.OTMin = null;
                        _attdata.StatusEI = null;
                        _attdata.StatusEO = null;
                        _attdata.StatusLI = null;
                        _attdata.StatusLO = null;
                        _attdata.StatusP = true;
                    }
                    context.SaveChanges();
                    if (context.SaveChanges() > 0)
                        check = true;
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }

        private bool AddJCTrainingToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
                    JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == _WorkCardID);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.WorkMin = _attdata.ShifMin;
                        _attdata.Remarks = "[OD-Training]";
                        _attdata.EarlyIn = null;
                        _attdata.EarlyOut = null;
                        _attdata.LateIn = null;
                        _attdata.LateOut = null;
                        _attdata.OTMin = null;
                        _attdata.StatusEI = null;
                        _attdata.StatusEO = null;
                        _attdata.StatusLI = null;
                        _attdata.StatusLO = null;
                        _attdata.StatusP = true;
                    }
                    context.SaveChanges();
                    if (context.SaveChanges() > 0)
                        check = true;
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return check;
        }

        #endregion






        public ActionResult DeleteJC(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp != null)
            {
                List<JobCardDetail> jcEmps = new List<JobCardDetail>();
                jcEmps = db.JobCardDetails.Where(aa => aa.JCAppID == id).ToList();
                if (jcEmps.Count > 0)
                {
                    foreach (var item in jcEmps)
                    {
                        db.JobCardDetails.Remove(item);
                        db.SaveChanges();
                        RemoveFromAttData(item);
                    }
                }
                db.JobCardApps.Remove(jobcardapp);
                db.SaveChanges();
                //return HttpNotFound();
            }
            return RedirectToAction("JobCardList");
        }

        private void RemoveFromAttData(JobCardDetail item)
        {
            try
            {
                DateTime date = item.Dated.Value;
                List<Emp> emp = db.Emps.Where(aa => aa.EmpID == item.EmpID).ToList();
                //ManualProcess mp = new ManualProcess();
                //mp.ManualProcessAttendance(date, emp);
            }
            catch (Exception ex)
            {
            }
        }



        // GET: /JobCard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardapp);
        }

        // GET: /JobCard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /JobCard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobCardID,DateCreated,DateStarted,DateEnded,CardType,UserID,JobCardCriteria,CriteriaData,Status,TimeIn,TimeOut,WorkMin")] JobCardApp jobcardapp)
        {
            if (ModelState.IsValid)
            {
                db.JobCardApps.Add(jobcardapp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobcardapp);
        }

        // GET: /JobCard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardapp);
        }

        // POST: /JobCard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobCardID,DateCreated,DateStarted,DateEnded,CardType,UserID,JobCardCriteria,CriteriaData,Status,TimeIn,TimeOut,WorkMin")] JobCardApp jobcardapp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobcardapp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobcardapp);
        }

        // GET: /JobCard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardapp);
        }

        // POST: /JobCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            db.JobCardApps.Remove(jobcardapp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.CustomClass;
using WMS.Models;
using PagedList;
namespace WMS.Controllers
{
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
    public class JobCardTimeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /JobCard/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewData["JobDate"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
            ViewBag.JobCardDesignation = new SelectList(db.Designations, "DesignationID", "DesignationName");
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
        public ActionResult DeleteJC(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardTime jobcardapp = db.JobCardTimes.Find(id);
            if (jobcardapp != null)
            {
                RemoveFromAttData(jobcardapp);
                db.JobCardTimes.Remove(jobcardapp);
                db.SaveChanges();
                //return HttpNotFound();
            }
            return RedirectToAction("JobCardList");
        }

        private void RemoveFromAttData(JobCardTime item)
        {
            try
            {
                DateTime date = item.DutyDate.Value;
                List<Emp> emp = db.Emps.Where(aa => aa.EmpID == item.EmpID).ToList();
                //ManualProcess mp = new ManualProcess();
                //mp.ManualProcessAttendance(date, emp);
            }
            catch (Exception ex)
            {
            }
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
                JobCardTime jobCardTime = new JobCardTime();
                jobCardTime.JobCardID = _WorkCardID;
                jobCardTime.CreatedDate = DateTime.Now;
                jobCardTime.DutyDate = Convert.ToDateTime(Request.Form["JobDate"]);
                string STime = Request.Form["SHour"].ToString();
                string ETime = Request.Form["EHour"].ToString();
                string STimeH = STime.Substring(0, 2);
                string STimeM = STime.Substring(2, 2);
                string ETimeH = ETime.Substring(0, 2);
                string ETimeM = ETime.Substring(2, 2);
                jobCardTime.CreatedBy = LoggedInUser.UserID;
                jobCardTime.AssignedBy = Convert.ToInt16(Request.Form["JobCardDesignation"].ToString());
                jobCardTime.StartTime = new TimeSpan(Convert.ToInt32(STimeH), Convert.ToInt32(STimeM), 0);
                jobCardTime.EndTime = new TimeSpan(Convert.ToInt32(ETimeH), Convert.ToInt32(ETimeM), 0);
                jobCardTime.TTime = (jobCardTime.EndTime - jobCardTime.StartTime);
                //jobCardTime.StartTime = Convert.toti(Request.Form["SHour"]);
                //jobCardTime.EndTime = Convert.ToDateTime(Request.Form["EHour"]);
                string Remakrs = Request.Form["Remakrs"].ToString();
                if (Remakrs != "")
                    jobCardTime.Remarks = Remakrs;
                jobCardTime.CreatedBy = LoggedInUser.UserID;
                _EmpNo = Request.Form["JobEmpNo"];
                _Emp = emps.Where(aa => aa.EmpNo == _EmpNo).ToList();
                if (_Emp.Count > 0)
                {
                    jobCardTime.EmpID = _Emp.FirstOrDefault().EmpID;
                    //if ((jobCardTime.EndTime - jobCardTime.StartTime).TotalMinutes > 0)
                    //{
                    if (ValidateJobCard(jobCardTime))
                    {
                        db.JobCardTimes.Add(jobCardTime);
                        if (db.SaveChanges() > 0)
                        {
                            AddJobCardTimeInAttData(jobCardTime);
                            Message = "Job Card Created Sucessfully";
                        }
                        else
                            Message = "Job Card is not created due to server error";
                    }
                    else
                        Message = "Job Card already created for FPID: " + _EmpNo;
                    //}
                }
                else
                    Message = "No Employee found, Please enter a valid FPID";
                //Add Job Card to JobCardData and Mark Legends in Attendance Data if attendance Created
                Session["EditAttendanceDate"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                ViewData["datef"] = Session["EditAttendanceDate"].ToString();
                ViewData["JobDate"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
                ViewBag.JobCardDesignation = new SelectList(db.Designations, "DesignationID", "DesignationName");
            }
            catch (Exception ex)
            {
                Message = "An Error occured while creating Job Card of " + Request.Form["JobCardType"].ToString();
            }
            //List<EmpView> emps = new List<EmpView>();
            ViewData["JobDate"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
            ViewBag.JobCardDesignation = new SelectList(db.Designations, "DesignationID", "DesignationName");
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
            DataTable dt = qb.GetValuesfromDB("select * from ViewJobCardTime " + query);
            List<ViewJobCardTime> jobCardsApps = dt.ToList<ViewJobCardTime>();

            // List<EmpView> emps = db.EmpViews.ToList();
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {

                    jobCardsApps = jobCardsApps.Where(s => s.EmpName.ToUpper().Contains(searchString.ToUpper())
                        || s.EmpNo.ToUpper().Contains(searchString.ToUpper()) || s.DesignationName.ToString().Contains(searchString)
                    ).OrderByDescending(aa => aa.CreatedDate).ToList();
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
        private bool ValidateJobCard(JobCardTime jobCardApp)
        {
            //List<JobCardApp> _Lc = new List<JobCardApp>();
            //DateTime _DTime = new DateTime();
            //DateTime _DTimeLV = new DateTime();
            //_Lc = db.JobCardApps.Where(aa => aa.CriteriaData == jobCardApp.CriteriaData).ToList();
            //foreach (var item in _Lc)
            //{
            //    _DTime = (DateTime)item.DateStarted;
            //    _DTimeLV = (DateTime)jobCardApp.DateStarted;
            //    while (_DTime <= item.DateEnded)
            //    {
            //        while (_DTimeLV <= jobCardApp.DateEnded)
            //        {
            //            if (_DTime == _DTimeLV)
            //                return false;
            //            _DTimeLV = _DTimeLV.AddDays(1);
            //        }
            //        _DTime = _DTime.AddDays(1);
            //    }
            //}
            return true;
        }

        private void AddJobCardTimeInAttData(JobCardTime jobCardTime)
        {
            if (db.AttProcesses.Where(aa => aa.ProcessDate == jobCardTime.DutyDate).Count() > 0)
            {
                switch (jobCardTime.JobCardID)
                {

                    case 8:// Training
                        AddJCTrainingToAttData(jobCardTime);
                        break;
                    case 9:// Tour
                        AddJCTourToAttData(jobCardTime);
                        break;
                    case 10:// Visit
                        AddJCVisitToAttData(jobCardTime);
                        break;
                    case 11:// Assignment
                        AddJCAssignToAttData(jobCardTime);
                        break;
                }
            }
        }

        #region --Job Cards - AttData ---
        private bool AddJCNorrmalDayAttData(JobCardTime jcTime)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    string empDate = jcTime.EmpID.ToString() + jcTime.DutyDate.Value.ToString("yyMMdd");
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == empDate);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
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

        private bool AddJCVisitToAttData(JobCardTime jcTime)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    string empDate = jcTime.EmpID.ToString() + jcTime.DutyDate.Value.ToString("yyMMdd");
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == empDate);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.StatusLI = false;
                        _attdata.StatusEO = false;
                        if (_attdata.ShifMin > 0)
                            _attdata.ShifMin = (short)(_attdata.ShifMin - (short)jcTime.TTime.Value.TotalMinutes);
                        else
                            _attdata.ShifMin = (short)jcTime.TTime.Value.TotalMinutes;
                        _attdata.Remarks = "[ODT-Visit]";
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

        private bool AddJCTourToAttData(JobCardTime jcTime)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    string empDate = jcTime.EmpID.ToString() + jcTime.DutyDate.Value.ToString("yyMMdd");
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == empDate);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.StatusLI = false;
                        _attdata.StatusEO = false;
                        if (_attdata.ShifMin > 0)
                            _attdata.ShifMin = (short)(_attdata.ShifMin - (short)jcTime.TTime.Value.TotalMinutes);
                        else
                            _attdata.ShifMin = (short)jcTime.TTime.Value.TotalMinutes;
                        _attdata.Remarks = "[ODT-Tour]";
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

        private bool AddJCTrainingToAttData(JobCardTime jcTime)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    string empDate = jcTime.EmpID.ToString() + jcTime.DutyDate.Value.ToString("yyMMdd");
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == empDate);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.StatusLI = false;
                        _attdata.StatusEO = false;
                        if (_attdata.ShifMin > 0)
                            _attdata.ShifMin = (short)(_attdata.ShifMin - (short)jcTime.TTime.Value.TotalMinutes);
                        else
                            _attdata.ShifMin = (short)jcTime.TTime.Value.TotalMinutes;
                        _attdata.Remarks = "[ODT-Training]";
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
        private bool AddJCAssignToAttData(JobCardTime jcTime)
        {
            bool check = false;
            try
            {
                //Normal Duty
                using (var context = new TAS2013Entities())
                {
                    string empDate = jcTime.EmpID.ToString() + jcTime.DutyDate.Value.ToString("yyMMdd");
                    AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == empDate);
                    if (_attdata != null)
                    {
                        _attdata.DutyCode = "D";
                        _attdata.StatusAB = false;
                        _attdata.StatusDO = false;
                        _attdata.StatusLeave = false;
                        _attdata.StatusP = true;
                        _attdata.StatusLI = false;
                        _attdata.StatusEO = false;
                        if (_attdata.ShifMin > 0)
                            _attdata.ShifMin = (short)(_attdata.ShifMin - (short)jcTime.TTime.Value.TotalMinutes);
                        else
                            _attdata.ShifMin = (short)jcTime.TTime.Value.TotalMinutes;
                        _attdata.Remarks = "[ODT-Assignment]";
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


        // GET: /JobCardTime/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /JobCardTime/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmpID,StartTime,EndTime,TTime,DutyDate,Remarks,AssignedBy,CreatedBy,JobCardID,CreatedDate")] JobCardTime jobcardtime)
        {
            if (ModelState.IsValid)
            {
                db.JobCardTimes.Add(jobcardtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobcardtime);
        }

        // GET: /JobCardTime/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            if (jobcardtime == null)
            {
                return HttpNotFound();
            }
            return View(jobcardtime);
        }

        // POST: /JobCardTime/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpID,StartTime,EndTime,TTime,DutyDate,Remarks,AssignedBy,CreatedBy,JobCardID,CreatedDate")] JobCardTime jobcardtime)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(jobcardtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobcardtime);
        }

        // GET: /JobCardTime/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            if (jobcardtime == null)
            {
                return HttpNotFound();
            }
            return View(jobcardtime);
        }

        // POST: /JobCardTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            db.JobCardTimes.Remove(jobcardtime);
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

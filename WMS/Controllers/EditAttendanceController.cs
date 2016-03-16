using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Controllers.EditAttendance;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.Models;

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class EditAttendanceController : Controller
    {
        //
        // GET: /EditAttendance/
        //public ActionResult Index()
        //{
        //    if (Session["EditAttendanceDate"] == null)
        //    {
        //        ViewData["datef"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //    }
        //    else
        //    {
        //        ViewData["datef"] = Session["EditAttendanceDate"].ToString();
        //    }
        //    User LoggedInUser = Session["LoggedUser"] as User;
        //    ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //    ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //    ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
        //    ViewBag.ShiftList = new SelectList(db.Shifts, "ShiftID", "ShiftName");

        //    ViewBag.CrewList = new SelectList(db.Crews, "CrewID", "CrewName");
        //    ViewBag.SectionList = new SelectList(db.Sections, "SectionID", "SectionName");
        //    ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
        //    ViewBag.Message = "";
        //    return View();
        //}

        //TAS2013Entities db = new TAS2013Entities();
        ////Load Attendance Details of Selected Employee
        //[HttpPost]
        //public ActionResult EditAttWizardOne(FormCollection form)
        //{
        //    try
        //    {
        //        User LoggedInUser = Session["LoggedUser"] as User;
        //        //ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //        ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
        //        //ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
        //        ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
        //        ViewData["datef"] = Convert.ToDateTime(Request.Form["DateFrom"].ToString()).ToString("yyyy-MM-dd"); 
        //        ViewBag.DesignationID = new SelectList(db.Designations.OrderBy(s=>s.DesignationName), "DesignationID", "DesignationName");
        //        //ViewData["datef"] = Request.Form["DateFrom"].ToString();
        //        if (Request.Form["EmpNo"].ToString() != "" && Request.Form["DateFrom"].ToString() != "")
        //        {
        //            string _EmpNo = Request.Form["EmpNo"].ToString();
        //            DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
        //            Session["EditAttendanceDate"] = Request.Form["DateFrom"].ToString();
        //            AttData _attData = new AttData();
        //            List<Emp> _Emp = new List<Emp>();
        //            int EmpID = 0;
        //            _Emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo && aa.Status==true).ToList();
        //            if (_Emp.Count > 0)
        //                EmpID = _Emp.FirstOrDefault().EmpID;
        //            _attData = db.AttDatas.FirstOrDefault(aa => aa.EmpID == EmpID && aa.AttDate == _AttDataFrom);
        //            if (_attData != null)
        //            {
        //                List<PollData> _Polls = new List<PollData>();
        //                string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
        //                _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
        //                ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
        //                ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
        //                ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s => s.EmpName), "EmpID", "EmpNo", _attData.EmpID);
        //                Session["NEmpNo"] = _attData.EmpID;
        //                ViewBag.SucessMessage = "";
        //                if (_attData.WorkMin != null)
        //                    ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
        //                if (_attData.LateOut != null)
        //                    ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
        //                if (_attData.LateIn != null)
        //                    ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
        //                if (_attData.EarlyOut != null)
        //                    ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
        //                if (_attData.EarlyIn != null)
        //                    ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
        //                if (_attData.OTMin != null)
        //                    ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
        //                if (_attData.GZOTMin != null)
        //                    ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
        //                return View(_attData);
        //            }
        //            else
        //            {
        //                ViewBag.Message = "Attendance Not Found";
        //                return View("Index");
        //            }
        //        }
        //        else
        //            return View("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("Sequence"))
        //            ViewBag.Message = "No Entry found on this particular date";
        //        return View("Index");

        //    }

        //}
        ////Add New Times and Process Attendance of Particular Employee
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditAttWizardData([Bind(Include = "EmpDate,AttDate,EmpNo,EmpID,DutyCode,DutyTime,TimeIn,TimeOut,WorkMin,LateIn,LateOut,EarlyIn,EarlyOut,OTMin,GZOTMin,BreakMin,SLMin,StatusP,StatusAB,StatusLI,StatusLO,StatusEI,StatusEO,StatusOT,StatusGZOT,StatusGZ,StatusDO,StatusHD,StatusSL,StatusOD,StatusLeave,StatusMN,StatusIN,StatusBreak,ShifMin,ShfSplit,ProcessIn,Remarks,Tin0,Tout0,Tin1,Tout1,Tin2,Tout2,Tin3,Tout3,Tin4,Tout4,Tin5,Tout5,Tin6,Tout6,Tin7,Tout7,Tin8,Tout8,Tin9,Tout9,Tin10,Tout10,Tin11,Tout11,Tin12,Tout12,Tin13,Tout13,Tin14,Tout14,Tin15,Tout15")] AttData _attData, FormCollection form, string NewDutyCode)
        //{
        //     User LoggedInUser = Session["LoggedUser"] as User;
        //    string _EmpDate = _attData.EmpDate;
        //    ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //    ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
        //    ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
        //    ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
        //    ViewBag.DesignationID = new SelectList(db.Designations.OrderBy(s => s.DesignationName), "DesignationID", "DesignationName");
        //    try
        //    {
        //        string STimeIn = form["Inhours"].ToString();
        //        if (STimeIn.Count() < 4)
        //            STimeIn = "0" + STimeIn;
        //        string STimeOut = form["OutHour"].ToString();
        //        if (STimeOut.Count() < 4)
        //            STimeOut = "0" + STimeOut;
        //        string STimeInH = STimeIn.Substring(0, 2);
        //        string STimeInM = STimeIn.Substring(2, 2);
        //        string STimeOutH = STimeOut.Substring(0, 2);
        //        string STimeOutM = STimeOut.Substring(2, 2);
        //        string DutyTime = form["DutyTime"].ToString();
        //        string Remarks = form["NewRemarks"].ToString();
        //        string SDutyH = DutyTime.Substring(0, 2);
        //        string SDutyM = DutyTime.Substring(2, 2);
        //        string ShiftMinString = form["ShiftMinHidden"].ToString();
        //        if (TimeValid(STimeIn, STimeOut))
        //        {
        //            TimeSpan _TimeIn = new TimeSpan(Convert.ToInt16(STimeInH), Convert.ToInt16(STimeInM), 0);
        //            TimeSpan _TimeOut = new TimeSpan(Convert.ToInt16(STimeOutH), Convert.ToInt16(STimeOutM), 0);
        //            TimeSpan _DutyTime = Convert.ToDateTime(form["DutyTime"].ToString()).TimeOfDay;
        //            //TimeSpan _DutyTime = new TimeSpan(Convert.ToInt16(SDutyH), Convert.ToInt16(SDutyM), 0);
        //            TimeSpan _ThresHoldTimeS = new TimeSpan(14, 00, 00);
        //            TimeSpan _ThresHoldTimeE = new TimeSpan(06, 00, 00);
        //            string date = Request.Form["Attdate"].ToString();
        //            DateTime _AttDate = Convert.ToDateTime(date);
        //            short ShiftMins = Convert.ToInt16(ShiftMinString);
        //            DateTime _NewTimeIn = new DateTime();
        //            DateTime _NewTimeOut = new DateTime();
        //            _NewTimeIn = _AttDate + _TimeIn;
        //            if (_TimeOut <_TimeIn)
        //            {
        //                _NewTimeOut = _AttDate.AddDays(1) + _TimeOut;
        //            }
        //            else
        //            {
        //                _NewTimeOut = _AttDate + _TimeOut;
        //            }
        //            int _UserID = Convert.ToInt32(Session["LogedUserID"].ToString());
        //            HelperClass.MyHelper.SaveAuditLog(_UserID, (byte)MyEnums.FormName.EditAttendance, (byte)MyEnums.Operation.Edit, DateTime.Now);
        //            ManualAttendanceProcess _pma = new ManualAttendanceProcess(_EmpDate, "", false, _NewTimeIn, _NewTimeOut, NewDutyCode, _UserID, _DutyTime, Remarks, ShiftMins);
        //            List<PollData> _Polls = new List<PollData>();
        //            _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
        //            ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
        //            ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
        //            _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
        //            ViewBag.SucessMessage = "Attendance record updated.";
        //            if (_attData.WorkMin != null)
        //                ViewBag.WorkMin = TimeSpan.FromMinutes((double)_attData.WorkMin);
        //            if (_attData.LateOut != null)
        //                ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
        //            if (_attData.LateIn != null)
        //                ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
        //            if (_attData.EarlyOut != null)
        //                ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
        //            if (_attData.EarlyIn != null)
        //                ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
        //            if (_attData.OTMin != null)
        //                ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
        //            if(_attData.StatusGZOT == true)
        //                ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
        //            return View("EditAttWizardOne", _attData);
        //        }
        //        else
        //        {
        //            ViewBag.SucessMessage = "New Time In and New Time out is not valid";
        //            _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
        //            return View(_attData);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.SucessMessage = "An error occured while saving Entry";
        //        _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
        //        List<PollData> _Polls = new List<PollData>();
        //        _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
        //        ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
        //        ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
        //        return View(_attData);
        //    }
        //}

        //private bool TimeValid(string STimeIn, string STimeOut)
        //{
        //    if (STimeIn.Count() == 4 && STimeOut.Count() == 4)
        //    {
        //        return true;
        //    }
        //    else
        //        return false;

        //}

        //private void MarkAttendanceEditedData(AttData _atData)
        //{
        //    ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //}

        //public ActionResult NextEntry(FormCollection form)
        //{
        //    try
        //    {
        //        ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //        ViewData["datef"] = Convert.ToDateTime(Session["EditAttendanceDate"]).ToString("yyyy-MM-dd");
        //        int _EmpID = Convert.ToInt32(Session["NEmpNo"]);
        //        if (Session["NEmpNo"] != null)
        //        {
        //            DateTime _AttDataFrom = Convert.ToDateTime(ViewData["datef"].ToString()).AddDays(1);
        //            AttData _attData = new AttData();
        //            _attData = db.AttDatas.First(aa => aa.EmpID == _EmpID && aa.AttDate == _AttDataFrom);
        //            if (_attData != null)
        //            {
        //                Session["EditAttendanceDate"] = Convert.ToDateTime(ViewData["datef"]).AddDays(1);
        //                ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s=>s.EmpName), "EmpID", "EmpNo", _attData.EmpID);
        //                List<PollData> _Polls = new List<PollData>();
        //                string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
        //                _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
        //                ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
        //                ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
        //                ViewBag.SucessMessage = "";
        //                if (_attData.WorkMin != null)
        //                    ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
        //                if (_attData.LateOut != null)
        //                    ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
        //                if (_attData.LateIn != null)
        //                    ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
        //                if (_attData.EarlyOut != null)
        //                    ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
        //                if (_attData.EarlyIn != null)
        //                    ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
        //                if (_attData.OTMin != null)
        //                    ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
        //                if (_attData.StatusGZOT == true)
        //                    ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
        //                return View("EditAttWizardOne", _attData);
        //            }
        //            else
        //                return View("Index");
        //        }
        //        else
        //            return View("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("Sequence"))
        //            ViewBag.Message = "No Entry found on this particular date";
        //        return View("Index");

        //    }
        //}
        //public ActionResult PreviousEntry()
        //{
        //    try
        //    {
        //        ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //        ViewData["datef"] = Convert.ToDateTime(Session["EditAttendanceDate"]).ToString("yyyy-MM-dd");
        //        int _EmpID = Convert.ToInt32(Session["NEmpNo"]);
        //        if (_EmpID != null)
        //        {
        //            DateTime _AttDataFrom = Convert.ToDateTime(ViewData["datef"].ToString()).AddDays(-1);
        //            AttData _attData = new AttData();
        //            _attData = db.AttDatas.First(aa => aa.EmpID == _EmpID && aa.AttDate == _AttDataFrom);
        //            if (_attData != null)
        //            {
        //                Session["EditAttendanceDate"] = Convert.ToDateTime(ViewData["datef"]).AddDays(-1);
        //                ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s=>s.EmpName), "EmpID", "EmpNo", _attData.EmpID);
        //                ViewBag.SucessMessage = "";
        //                List<PollData> _Polls = new List<PollData>();
        //                string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
        //                _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
        //                ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
        //                ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
        //                if (_attData.WorkMin != null)
        //                    ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
        //                if (_attData.LateOut != null)
        //                    ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
        //                if (_attData.LateIn != null)
        //                    ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
        //                if (_attData.EarlyOut != null)
        //                    ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
        //                if (_attData.EarlyIn != null)
        //                    ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
        //                if (_attData.OTMin != null)
        //                    ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
        //                if (_attData.StatusGZOT == true)
        //                    ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
        //                return View("EditAttWizardOne", _attData);
        //            }
        //            else
        //                return View("Index");
        //        }
        //        else
        //            return View("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("Sequence"))
        //            ViewBag.Message = "No Entry found on this particular date";
        //        return View("Index");

        //    }
        //}

        ///// <summary>
        ///// Job Cards
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult EditAttJobCard()
        //{
        //    User LoggedInUser = Session["LoggedUser"] as User;
        //    try
        //    {
        //        string _EmpNo = "";
        //        List<Emp> _Emp = new List<Emp>();
        //        short _WorkCardID = Convert.ToInt16(Request.Form["JobCardType"].ToString());
        //        //First Save Job Card Application
        //        JobCardApp jobCardApp = new JobCardApp();
        //        jobCardApp.CardType = _WorkCardID;
        //        jobCardApp.DateCreated = DateTime.Now;
        //        jobCardApp.UserID = LoggedInUser.UserID;
        //        jobCardApp.DateStarted = Convert.ToDateTime(Request.Form["JobDateFrom"]);
        //        jobCardApp.DateEnded = Convert.ToDateTime(Request.Form["JobDateTo"]);
        //        // For Double Duty Only
        //            if(jobCardApp.CardType==8)
        //                jobCardApp.WorkMin = Convert.ToInt16(Request.Form["DDWorkMins"].ToString());
        //            if (jobCardApp.CardType == 9)
        //                jobCardApp.OtherValue = Convert.ToInt16(Request.Form["DesignationID"].ToString());
        //        jobCardApp.Status = false;
               
        //            switch (Request.Form["cars"].ToString())
        //            {
        //                case "shift":
        //                    jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["ShiftList"].ToString());
        //                    jobCardApp.JobCardCriteria = "S";
        //                    db.JobCardApps.Add(jobCardApp);
        //                    if (db.SaveChanges() > 0)
        //                    {
        //                        AddJobCardAppToJobCardData();
        //                    }
        //                    break;
        //                case "crew":
        //                    jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["CrewList"].ToString());
        //                    jobCardApp.JobCardCriteria = "C";
        //                    db.JobCardApps.Add(jobCardApp);
        //                    if (db.SaveChanges() > 0)
        //                    {
        //                        AddJobCardAppToJobCardData();
        //                    }
        //                    break;
        //                case "section":
        //                    jobCardApp.CriteriaData = Convert.ToInt32(Request.Form["SectionList"].ToString());
        //                    jobCardApp.JobCardCriteria = "T";
        //                    db.JobCardApps.Add(jobCardApp);
        //                    if (db.SaveChanges() > 0)
        //                    {
        //                        AddJobCardAppToJobCardData();
        //                    }
        //                    break;
        //                case "employee":
        //                    if (Request.Form["cars"].ToString() == "employee")
        //                    {
        //                        _EmpNo = Request.Form["JobEmpNo"];
        //                        _Emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo).ToList();
        //                        if (_Emp.Count > 0)
        //                        {
        //                            jobCardApp.CriteriaData = _Emp.FirstOrDefault().EmpID;
        //                            jobCardApp.JobCardCriteria = "E";
        //                            db.JobCardApps.Add(jobCardApp);
        //                            if (db.SaveChanges() > 0)
        //                            {
        //                                AddJobCardAppToJobCardData();
        //                            }
        //                        }
        //                    }
        //                    break;
        //            }
                
        //        //Add Job Card to JobCardData and Mark Legends in Attendance Data if attendance Created
        //        Session["EditAttendanceDate"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
        //        ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //        ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
        //        ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
        //        ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
        //        ViewBag.CMessage = "Job Card Created sucessfully";
        //        ViewData["datef"] = Session["EditAttendanceDate"].ToString();
        //        ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //        ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //        ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
        //            return View("Index");
        //    }
        //    catch (Exception)
        //    {
        //        //ViewData["datef"] = HttpContext.Session["EditAttendanceDate"].ToString();
        //        ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //        ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        //        ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        //        ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
        //        ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
        //        ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
        //        ViewBag.CMessage = "An Error occured while creating Job Card of" + Request.Form["JobCardType"].ToString();
        //        ViewBag.DesignationID = new SelectList(db.Designations, "DesignationID", "DesignationName");
        //        return View("Index");
        //    }
        //}

        //private bool ValidateJobCard(DateTime dateStart, short CardType)
        //{
        //    bool check = false;
        //    using (var ctx = new TAS2013Entities())
        //    {
        //        List<JobCardApp> jcApp = new List<JobCardApp>();
        //        if (ctx.JobCardApps.Where(aa => aa.DateStarted == dateStart && aa.CardType == CardType).Count() > 0)
        //            check = true;
        //        ctx.Dispose();
        //    }
        //    return check;
        //}

        //public ActionResult CompanyIDJobCardList(string ID)
        //{
        //    short Code = Convert.ToInt16(ID);
        //    var secs = db.Designations.OrderBy(s => s.DesignationName);
        //    if (HttpContext.Request.IsAjaxRequest())
        //        return Json(new SelectList(
        //                        secs.ToArray(),
        //                        "DesignationID",
        //                        "DesignationName")
        //                   , JsonRequestBehavior.AllowGet);

        //    return RedirectToAction("Index");
        //}

        ////Add Job Card To Job Card Data
        //private void AddJobCardAppToJobCardData()
        //{
        //    using (var ctx = new TAS2013Entities())
        //    {
        //        List<JobCardApp> _jobCardApp = new List<JobCardApp>();
        //        _jobCardApp = ctx.JobCardApps.Where(aa => aa.Status == false).ToList();
        //        List<Emp> _Emp = new List<Emp>();
        //        foreach (var jcApp in _jobCardApp)
        //        {
        //            jcApp.Status = true;
        //            switch (jcApp.JobCardCriteria)
        //            {
        //                case "S":
        //                    short _shiftID = Convert.ToByte(jcApp.CriteriaData);
        //                    _Emp = ctx.Emps.Where(aa => aa.ShiftID == _shiftID).ToList();
        //                    break;
        //                case "C":
        //                    short _crewID = Convert.ToByte(jcApp.CriteriaData);
        //                    _Emp = ctx.Emps.Where(aa => aa.CrewID == _crewID).ToList();
        //                    break;
        //                case "T":
        //                    short _secID = Convert.ToByte(jcApp.CriteriaData);
        //                    _Emp = ctx.Emps.Where(aa => aa.SecID == _secID).ToList();
        //                    break;
        //                case "E":
        //                    int _EmpID = (int)jcApp.CriteriaData;
        //                    _Emp = ctx.Emps.Where(aa => aa.EmpID == _EmpID).ToList();
        //                    break;
        //            }
        //            foreach (var selectedEmp in _Emp)
        //            {
        //                AddJobCardData(selectedEmp, jcApp);
        //            }
        //        }
        //        ctx.SaveChanges();
        //        ctx.Dispose();
        //    }
        //}

        //private void AddJobCardData(Emp _selEmp, JobCardApp jcApp)
        //{
        //    int _empID = _selEmp.EmpID;
        //    string _empDate = "";
        //    int _userID = (int)jcApp.UserID;
        //    DateTime _Date = (DateTime)jcApp.DateStarted;
        //    while (_Date <= jcApp.DateEnded)
        //    {
        //        _empDate = _selEmp.EmpID + _Date.ToString("yyMMdd");
        //        AddJobCardDataToDatabase(_empDate, _empID, _Date, _userID,jcApp);
        //        if (db.AttProcesses.Where(aa => aa.ProcessDate == _Date).Count()>0)
        //        {
        //            switch (jcApp.CardType)
        //            {
        //                case 1://Day Off
        //                    AddJCDayOffToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
        //                    break;
        //                case 2://GZ Holiday
        //                    AddJCGZDayToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
        //                    break;
        //                case 3://Absent
        //                    AddJCAbsentToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
        //                    break;
        //                case 4://official Duty
        //                    AddJCODDayToAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
        //                    break;
        //                case 8:// Double Duty
        //                    AddDoubleDutyAttData(_empDate, _empID, _Date, _userID, jcApp);
        //                    break;
        //                case 9:// Badli Duty
        //                    AddBadliTableData(_empID,_empDate,_Date,(short)jcApp.OtherValue,jcApp.Remarks);
        //                    AddBadliAttData(_empDate, _empID, _Date, _userID, jcApp);
        //                    break;
        //                //case 10:// Late In Margin
        //                //    AddLateInMarginAttData(_empDate, _empID, _Date, _userID, (short)jcApp.CardType);
        //                //    break;
        //            }
        //        }
        //        _Date = _Date.AddDays(1);
        //    }
        //    HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.EditAttendance, (byte)MyEnums.Operation.Edit, DateTime.Now);
        //}

        //private void AddBadliTableData(int _empID, string _empDate, DateTime _Date, short desigID, string remarks)
        //{
        //    try
        //    {
        //        BadliRecord br = new BadliRecord();
        //        br.EmpID = _empID;
        //        br.EmpDateBadli = _empDate;
        //        br.BadliDesgID = desigID;
        //        br.Remarks = remarks;
        //        br.Date = _Date;
        //        dbBadliRecords.Add(br);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}

        //private bool AddJobCardDataToDatabase(string _empDate, int _empID, DateTime _currentDate, int _userID,JobCardApp jcApp)
        //{
        //    bool check = false;
        //    try
        //    {
        //        JobCardEmp _jobCardEmp = new JobCardEmp();
        //        _jobCardEmp.EmpDate = _empDate;
        //        _jobCardEmp.EmpID = _empID;
        //        _jobCardEmp.Dated = _currentDate;
        //        _jobCardEmp.SubmittedFrom = _userID;
        //        _jobCardEmp.WrkCardID = jcApp.CardType;
        //        _jobCardEmp.DateCreated = DateTime.Now;
        //        _jobCardEmp.WorkMin = jcApp.WorkMin;
        //        _jobCardEmp.JCAppID = jcApp.JobCardID;
        //        _jobCardEmp.OtherValue = jcApp.OtherValue;
        //        db.JobCardEmps.Add(_jobCardEmp);
        //        if (db.SaveChanges() > 0)
        //        {
        //            check= true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        check = false;
        //    }
        //    return check;
        //}

        //#region --Job Cards - AttData --- 
        //private bool AddJCNorrmalDayAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        //{
        //    bool check = false;
        //    try{
        //    //Normal Duty
        //    using (var context = new TAS2013Entities())
        //    {
        //        AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //        JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == _WorkCardID);
        //        if (_attdata != null)
        //        {
        //            _attdata.DutyCode = "D";
        //            _attdata.StatusAB = false;
        //            _attdata.StatusDO = false;
        //            _attdata.StatusLeave = false;
        //            _attdata.StatusP = true;
        //            _attdata.WorkMin = _jcCard.WorkMin;
        //            _attdata.ShifMin = _jcCard.WorkMin;
        //            _attdata.Remarks = "[Present][Manual]";
        //            _attdata.TimeIn = null;
        //            _attdata.TimeOut = null;
        //            _attdata.EarlyIn = null;
        //            _attdata.EarlyOut = null;
        //            _attdata.LateIn = null;
        //            _attdata.LateOut = null;
        //            _attdata.OTMin = null;
        //            _attdata.StatusEI = null;
        //            _attdata.StatusEO = null;
        //            _attdata.StatusLI = null;
        //            _attdata.StatusLO = null;
        //            _attdata.StatusP = true;
        //        }
        //        context.SaveChanges();
        //        if (context.SaveChanges() > 0)
        //            check = true;
        //        context.Dispose();
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return check;
        //}

        //private bool AddDoubleDutyAttData(string _empDate, int _empID, DateTime _Date, int _userID, JobCardApp jcApp)
        //{
        //    bool check = false;
        //    try
        //    {
        //        //Normal Duty
        //        using (var context = new TAS2013Entities())
        //        {
        //            AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //            JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == jcApp.CardType);
        //            if (_attdata != null)
        //            {
        //                _attdata.DutyCode = "D";
        //                _attdata.StatusAB = false;
        //                _attdata.StatusDO = false;
        //                _attdata.StatusLeave = false;
        //                _attdata.StatusP = true;
        //                _attdata.WorkMin = _jcCard.WorkMin;
        //                _attdata.Remarks = _attdata.Remarks+"[DD][Manual]";
        //                _attdata.StatusMN = true;
        //                _attdata.TimeIn = null;
        //                _attdata.TimeOut = null;
        //                _attdata.EarlyIn = null;
        //                _attdata.EarlyOut = null;
        //                _attdata.LateIn = null;
        //                _attdata.LateOut = null;
        //                _attdata.OTMin = null;
        //                _attdata.StatusEI = null;
        //                _attdata.StatusEO = null;
        //                _attdata.StatusLI = null;
        //                _attdata.StatusLO = null;
        //                _attdata.StatusP = true;
        //            }
        //            context.SaveChanges();
        //            if (context.SaveChanges() > 0)
        //                check = true;
        //            context.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return check;
        //}

        //private bool AddBadliAttData(string _empDate, int _empID, DateTime _Date, int _userID, JobCardApp jcApp)
        //{
        //    bool check = false;
        //    try
        //    {
        //        //Normal Duty
        //        using (var context = new TAS2013Entities())
        //        {
        //            AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //            JobCard _jcCard = context.JobCards.FirstOrDefault(aa => aa.WorkCardID == jcApp.CardType);
        //            if (_attdata != null)
        //            {
        //                _attdata.DutyCode = "D";
        //                _attdata.StatusAB = false;
        //                _attdata.StatusLeave = false;
        //                _attdata.StatusP = true;
        //                _attdata.Remarks = _attdata.Remarks+ "[Badli][Manual]";
        //                _attdata.StatusMN = true;
        //            }
        //            context.SaveChanges();
        //            if (context.SaveChanges() > 0)
        //                check = true;
        //            context.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return check;
        //}
        //private bool AddJCODDayToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        //{

        //    bool check = false;
        //    try
        //    {
        //        //Official Duty
        //        using (var context = new TAS2013Entities())
        //        {
        //            AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //            if (_attdata != null)
        //            {
        //                _attdata.DutyCode = "O";
        //                _attdata.StatusAB = false;
        //                _attdata.StatusDO = false;
        //                _attdata.StatusLeave = false;
        //                _attdata.StatusP = true;
        //                _attdata.WorkMin = _attdata.ShifMin;
        //                _attdata.Remarks = _attdata.Remarks+"[Official Duty][Manual]";
        //                _attdata.TimeIn = null;
        //                _attdata.TimeOut = null;
        //                _attdata.WorkMin = null;
        //                _attdata.EarlyIn = null;
        //                _attdata.EarlyOut = null;
        //                _attdata.LateIn = null;
        //                _attdata.LateOut = null;
        //                _attdata.OTMin = null;
        //                _attdata.StatusEI = null;
        //                _attdata.StatusEO = null;
        //                _attdata.StatusLI = null;
        //                _attdata.StatusLO = null;
        //                _attdata.StatusP = null;
        //                _attdata.StatusGZ = false;
        //            }
        //            context.SaveChanges();
        //            if (context.SaveChanges() > 0)
        //                check = true;
        //            context.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return check;
        //}

        //private bool AddJCAbsentToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        //{
        //    bool check = false;
        //    try{
        //    //Absent
        //    using (var context = new TAS2013Entities())
        //    {
        //        AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //        if (_attdata != null)
        //        {
        //            _attdata.DutyCode = "D";
        //            _attdata.StatusAB = true;
        //            _attdata.StatusDO = false;
        //            _attdata.StatusLeave = false;
        //            _attdata.Remarks = "[Absent][Manual]";
        //            _attdata.TimeIn = null;
        //            _attdata.TimeOut = null;
        //            _attdata.WorkMin = null;
        //            _attdata.EarlyIn = null;
        //            _attdata.EarlyOut = null;
        //            _attdata.LateIn = null;
        //            _attdata.LateOut = null;
        //            _attdata.OTMin = null;
        //            _attdata.StatusEI = null;
        //            _attdata.StatusEO = null;
        //            _attdata.StatusLI = null;
        //            _attdata.StatusLO = null;
        //            _attdata.StatusP = null;
        //        }
        //        context.SaveChanges();
        //        if (context.SaveChanges() > 0)
        //            check = true;
        //        context.Dispose();
        //    }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return check;
        //}

        //private bool AddJCGZDayToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        //{
        //    bool check = false;
        //    try{
        //    //GZ Holiday
        //        using (var context = new TAS2013Entities())
        //        {
        //            AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //            if (_attdata != null)
        //            {
        //                _attdata.DutyCode = "G";
        //                _attdata.StatusAB = false;
        //                _attdata.StatusDO = true;
        //                _attdata.StatusLeave = false;
        //                _attdata.StatusGZ = true;
        //                _attdata.Remarks = "[GZ][Manual]";
        //                _attdata.TimeIn = null;
        //                _attdata.TimeOut = null;
        //                _attdata.WorkMin = null;
        //                _attdata.EarlyIn = null;
        //                _attdata.EarlyOut = null;
        //                _attdata.LateIn = null;
        //                _attdata.LateOut = null;
        //                _attdata.OTMin = null;
        //                _attdata.StatusEI = null;
        //                _attdata.StatusEO = null;
        //                _attdata.StatusLI = null;
        //                _attdata.StatusLO = null;
        //                _attdata.StatusP = null;
        //            }
        //            context.SaveChanges();
        //            if (context.SaveChanges() > 0)
        //                check = true;
        //            context.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return check;
        //}

        //private bool AddJCDayOffToAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        //{
        //    bool check = false;
        //    try
        //    {
        //        //Day Off
        //        using (var context = new TAS2013Entities())
        //        {
        //            AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //            if (_attdata != null)
        //            {
        //                _attdata.DutyCode = "R";
        //                _attdata.StatusAB = false;
        //                _attdata.StatusDO = true;
        //                _attdata.StatusLeave = false;
        //                _attdata.Remarks = "[DO][Manual]";
        //                _attdata.TimeIn = null;
        //                _attdata.TimeOut = null;
        //                _attdata.WorkMin = null;
        //                _attdata.EarlyIn = null;
        //                _attdata.EarlyOut = null;
        //                _attdata.LateIn = null;
        //                _attdata.LateOut = null;
        //                _attdata.OTMin = null;
        //                _attdata.StatusEI = null;
        //                _attdata.StatusEO = null;
        //                _attdata.StatusLI = null;
        //                _attdata.StatusLO = null;
        //                _attdata.StatusP = null;
        //            }
        //            context.SaveChanges();
        //            if (context.SaveChanges() > 0)
        //                check = true;
        //            context.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return check;
        //}
        
        //private bool AddLateInMarginAttData(string _empDate, int _empID, DateTime _Date, int _userID, short _WorkCardID)
        //{
        //    bool check = false;
        //    try
        //    {
        //        //Late In Job Card
        //        short LateInMins = 0;
        //        using (var context = new TAS2013Entities())
        //        {
        //            AttData _attdata = context.AttDatas.FirstOrDefault(aa => aa.EmpDate == _empDate);
        //            if (_attdata != null)
        //            {
        //                _attdata.StatusAB = false;
        //                _attdata.Remarks.Replace("[LI]", "");
        //                _attdata.Remarks = _attdata.Remarks + "[LI Job Card]";
        //                _attdata.LateIn = 0;
        //                _attdata.WorkMin = (short)(_attdata.WorkMin + (short)LateInMins);
        //                _attdata.StatusLI = false;
        //            }
        //            context.SaveChanges();
        //            if (context.SaveChanges() > 0)
        //                check = true;
        //            context.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return check;
        //}

        //#endregion
    }
}
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
        
        //GET: /EditAttendance/
        public ActionResult Index()
        {
            if (Session["EditAttendanceDate"] == null)
            {
                ViewData["datef"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                ViewData["datef"] = Session["EditAttendanceDate"].ToString();
            }
            User LoggedInUser = Session["LoggedUser"] as User;
            ViewData["JobDateFrom"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewData["JobDateTo"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.Message = "";
            return View();
        }

        TAS2013Entities db = new TAS2013Entities();
        //Load Attendance Details of Selected Employee
        [HttpPost]
        public ActionResult EditAttWizardOne(FormCollection form)
        {
            try
            {
                User LoggedInUser = Session["LoggedUser"] as User;
                if (Request.Form["EmpNo"].ToString() != "" && Request.Form["DateFrom"].ToString() != "")
                {
                    string _EmpNo = Request.Form["EmpNo"].ToString();
                    DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                    Session["EditAttendanceDate"] = Request.Form["DateFrom"].ToString();
                    AttData _attData = new AttData();
                    List<Emp> _Emp = new List<Emp>();
                    int EmpID = 0;
                    _Emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo && aa.Status == true).ToList();                   
                    if (_Emp.Count > 0)
                    {
                        EmpID = _Emp.FirstOrDefault().EmpID;
                        if (db.AttDatas.Where(aa => aa.EmpID == EmpID && aa.AttDate == _AttDataFrom).Count() > 0)
                        {

                            _attData = db.AttDatas.FirstOrDefault(aa => aa.EmpID == EmpID && aa.AttDate == _AttDataFrom);
                            List<PollData> _Polls = new List<PollData>();
                            string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
                            _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
                            ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                            ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                            ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s => s.EmpName), "EmpID", "EmpNo", _attData.EmpID);
                            Session["NEmpNo"] = _attData.EmpID;
                            ViewBag.SucessMessage = "";
                            if (_attData.WorkMin != null)
                                ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
                            if (_attData.LateOut != null)
                                ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                            if (_attData.LateIn != null)
                                ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                            if (_attData.EarlyOut != null)
                                ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                            if (_attData.EarlyIn != null)
                                ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
                            if (_attData.OTMin != null)
                                ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
                            if (_attData.GZOTMin != null)
                                ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                            return View(_attData);
                        }
                        else
                        {
                            ViewBag.Message = "Attendance Not Found";
                            return View("Index");
                        }
                    }
                    else
                        ViewBag.Message = "No employee found";
                }
                else
                    ViewBag.Message = "Please fill all field";
                    return View("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }

        }
        
        //Add New Times and Process Attendance of Particular Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttWizardData([Bind(Include = "EmpDate,AttDate,EmpNo,EmpID,DutyCode,DutyTime,TimeIn,TimeOut,WorkMin,LateIn,LateOut,EarlyIn,EarlyOut,OTMin,GZOTMin,BreakMin,SLMin,StatusP,StatusAB,StatusLI,StatusLO,StatusEI,StatusEO,StatusOT,StatusGZOT,StatusGZ,StatusDO,StatusHD,StatusSL,StatusOD,StatusLeave,StatusMN,StatusIN,StatusBreak,ShifMin,ShfSplit,ProcessIn,Remarks,Tin0,Tout0,Tin1,Tout1,Tin2,Tout2,Tin3,Tout3,Tin4,Tout4,Tin5,Tout5,Tin6,Tout6,Tin7,Tout7,Tin8,Tout8,Tin9,Tout9,Tin10,Tout10,Tin11,Tout11,Tin12,Tout12,Tin13,Tout13,Tin14,Tout14,Tin15,Tout15")] AttData _attData, FormCollection form, string NewDutyCode)
        {
             User LoggedInUser = Session["LoggedUser"] as User;
            string _EmpDate = _attData.EmpDate;
            ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
            ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
            ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
            ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
            ViewBag.DesignationID = new SelectList(db.Designations.OrderBy(s => s.DesignationName), "DesignationID", "DesignationName");
            try
            {
                string STimeIn = form["Inhours"].ToString();
                if (STimeIn.Count() < 4)
                    STimeIn = "0" + STimeIn;
                string STimeOut = form["OutHour"].ToString();
                if (STimeOut.Count() < 4)
                    STimeOut = "0" + STimeOut;
                string STimeInH = STimeIn.Substring(0, 2);
                string STimeInM = STimeIn.Substring(2, 2);
                string STimeOutH = STimeOut.Substring(0, 2);
                string STimeOutM = STimeOut.Substring(2, 2);
                string DutyTime = form["DutyTime"].ToString();
                string Remarks = form["NewRemarks"].ToString();
                string SDutyH = DutyTime.Substring(0, 2);
                string SDutyM = DutyTime.Substring(2, 2);
                string ShiftMinString = form["ShiftMinHidden"].ToString();
                if (TimeValid(STimeIn, STimeOut))
                {
                    TimeSpan _TimeIn = new TimeSpan(Convert.ToInt16(STimeInH), Convert.ToInt16(STimeInM), 0);
                    TimeSpan _TimeOut = new TimeSpan(Convert.ToInt16(STimeOutH), Convert.ToInt16(STimeOutM), 0);
                    TimeSpan _DutyTime = Convert.ToDateTime(form["DutyTime"].ToString()).TimeOfDay;
                    //TimeSpan _DutyTime = new TimeSpan(Convert.ToInt16(SDutyH), Convert.ToInt16(SDutyM), 0);
                    TimeSpan _ThresHoldTimeS = new TimeSpan(14, 00, 00);
                    TimeSpan _ThresHoldTimeE = new TimeSpan(06, 00, 00);
                    string date = Request.Form["Attdate"].ToString();
                    DateTime _AttDate = Convert.ToDateTime(date);
                    short ShiftMins = Convert.ToInt16(ShiftMinString);
                    DateTime _NewTimeIn = new DateTime();
                    DateTime _NewTimeOut = new DateTime();
                    _NewTimeIn = _AttDate + _TimeIn;
                    if (_TimeOut <_TimeIn)
                    {
                        _NewTimeOut = _AttDate.AddDays(1) + _TimeOut;
                    }
                    else
                    {
                        _NewTimeOut = _AttDate + _TimeOut;
                    }
                    int _UserID = Convert.ToInt32(Session["LogedUserID"].ToString());
                    HelperClass.MyHelper.SaveAuditLog(_UserID, (byte)MyEnums.FormName.EditAttendance, (byte)MyEnums.Operation.Edit, DateTime.Now);
                    ManualAttendanceProcess _pma = new ManualAttendanceProcess(_EmpDate, "", false, _NewTimeIn, _NewTimeOut, NewDutyCode, _UserID, _DutyTime, Remarks, ShiftMins);
                    List<PollData> _Polls = new List<PollData>();
                    _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
                    ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                    ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                    _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
                    ViewBag.SucessMessage = "Attendance record updated.";
                    if (_attData.WorkMin != null)
                        ViewBag.WorkMin = TimeSpan.FromMinutes((double)_attData.WorkMin);
                    if (_attData.LateOut != null)
                        ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                    if (_attData.LateIn != null)
                        ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                    if (_attData.EarlyOut != null)
                        ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                    if (_attData.EarlyIn != null)
                        ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
                    if (_attData.OTMin != null)
                        ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
                    if(_attData.StatusGZOT == true)
                        ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                    return View("EditAttWizardOne", _attData);
                }
                else
                {
                    ViewBag.SucessMessage = "New Time In and New Time out is not valid";
                    _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
                    return View(_attData);
                }

            }
            catch (Exception ex)
            {
                ViewBag.SucessMessage = "An error occured while saving Entry";
                _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
                List<PollData> _Polls = new List<PollData>();
                _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
                ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                return View(_attData);
            }
        }

        private bool TimeValid(string STimeIn, string STimeOut)
        {
            if (STimeIn.Count() == 4 && STimeOut.Count() == 4)
            {
                return true;
            }
            else
                return false;

        }

        private void MarkAttendanceEditedData(AttData _atData)
        {
            ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
        }

        public ActionResult NextEntry(FormCollection form)
        {
            try
            {
                ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
                ViewData["datef"] = Convert.ToDateTime(Session["EditAttendanceDate"]).ToString("yyyy-MM-dd");
                int _EmpID = Convert.ToInt32(Session["NEmpNo"]);
                if (Session["NEmpNo"] != null)
                {
                    DateTime _AttDataFrom = Convert.ToDateTime(ViewData["datef"].ToString()).AddDays(1);
                    AttData _attData = new AttData();
                    _attData = db.AttDatas.First(aa => aa.EmpID == _EmpID && aa.AttDate == _AttDataFrom);
                    if (_attData != null)
                    {
                        Session["EditAttendanceDate"] = Convert.ToDateTime(ViewData["datef"]).AddDays(1);
                        ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s=>s.EmpName), "EmpID", "EmpNo", _attData.EmpID);
                        List<PollData> _Polls = new List<PollData>();
                        string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
                        _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
                        ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                        ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                        ViewBag.SucessMessage = "";
                        if (_attData.WorkMin != null)
                            ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
                        if (_attData.LateOut != null)
                            ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                        if (_attData.LateIn != null)
                            ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                        if (_attData.EarlyOut != null)
                            ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                        if (_attData.EarlyIn != null)
                            ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
                        if (_attData.OTMin != null)
                            ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
                        if (_attData.StatusGZOT == true)
                            ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                        return View("EditAttWizardOne", _attData);
                    }
                    else
                        return View("Index");
                }
                else
                    return View("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }
        }
        public ActionResult PreviousEntry()
        {
            try
            {
                ViewBag.JobCardType = new SelectList(db.JobCards.OrderBy(s=>s.WorkCardName), "WorkCardID", "WorkCardName");
                ViewData["datef"] = Convert.ToDateTime(Session["EditAttendanceDate"]).ToString("yyyy-MM-dd");
                int _EmpID = Convert.ToInt32(Session["NEmpNo"]);
                if (_EmpID != null)
                {
                    DateTime _AttDataFrom = Convert.ToDateTime(ViewData["datef"].ToString()).AddDays(-1);
                    AttData _attData = new AttData();
                    _attData = db.AttDatas.First(aa => aa.EmpID == _EmpID && aa.AttDate == _AttDataFrom);
                    if (_attData != null)
                    {
                        Session["EditAttendanceDate"] = Convert.ToDateTime(ViewData["datef"]).AddDays(-1);
                        ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s=>s.EmpName), "EmpID", "EmpNo", _attData.EmpID);
                        ViewBag.SucessMessage = "";
                        List<PollData> _Polls = new List<PollData>();
                        string _EmpDate = _attData.EmpID.ToString() + _AttDataFrom.Date.ToString("yyMMdd");
                        _Polls = db.PollDatas.Where(aa => aa.EmpDate == _EmpDate).OrderBy(a => a.EntTime).ToList();
                        ViewBag.PollsDataIn = _Polls.Where(aa => aa.RdrDuty == 1);
                        ViewBag.PollsDataOut = _Polls.Where(aa => aa.RdrDuty == 5);
                        if (_attData.WorkMin != null)
                            ViewBag.WorkMin = (TimeSpan.FromMinutes((double)_attData.WorkMin));
                        if (_attData.LateOut != null)
                            ViewBag.LateOut = TimeSpan.FromMinutes((double)_attData.LateOut);
                        if (_attData.LateIn != null)
                            ViewBag.LateIn = TimeSpan.FromMinutes((double)_attData.LateIn);
                        if (_attData.EarlyOut != null)
                            ViewBag.EarlyOut = TimeSpan.FromMinutes((double)_attData.EarlyOut);
                        if (_attData.EarlyIn != null)
                            ViewBag.EarlyIn = TimeSpan.FromMinutes((double)_attData.EarlyIn);
                        if (_attData.OTMin != null)
                            ViewBag.OT = TimeSpan.FromMinutes((double)_attData.OTMin);
                        if (_attData.StatusGZOT == true)
                            ViewBag.GZOT = TimeSpan.FromMinutes((double)_attData.GZOTMin);
                        return View("EditAttWizardOne", _attData);
                    }
                    else
                        return View("Index");
                }
                else
                    return View("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }
        }
        public ActionResult GetEmployeeInfo(string empNo)
        {
            List<Emp> emp = db.Emps.Where(aa => aa.EmpNo == empNo).ToList();
            string Name = "";
            string Designation = "";
            string Section = "";
            string FatherName = "";
            string Type = "";
            string DOJ = "";
            if (emp.Count > 0)
            {
                Name = "Name: " + emp.FirstOrDefault().EmpName;
                FatherName = "FatherName:" + emp.FirstOrDefault().FatherName;
                Designation = "Designation: " + emp.FirstOrDefault().Designation.DesignationName;
                Section = "Section: " + emp.FirstOrDefault().Section.SectionName;
                Type = "Type: " + emp.FirstOrDefault().Status;
                if (emp.FirstOrDefault().BirthDate != null)
                    DOJ = "Join Date: " + emp.FirstOrDefault().BirthDate.Value.ToString("dd-MMM-yyyy");
                else
                    DOJ = "Join Date: Not Added";
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(Name + "@" + FatherName + "@" + Designation + "@" + Section + "@" + Type + "@" + DOJ, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Name = "Name: Not found";
                Designation = "Designation: Not found";
                Section = "Section: Not found";
                Type = "Type: Not found";
                DOJ = "Join Date: Not found";
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(Name + "@" + FatherName + "@" + Designation + "@" + Section + "@" + Type + "@" + DOJ
                       , JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

    }
}
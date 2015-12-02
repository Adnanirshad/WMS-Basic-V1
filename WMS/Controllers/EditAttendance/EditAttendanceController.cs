using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.Models;

namespace WMS.Controllers.EditAttendance
{
    [CustomControllerAttributes]
    public class EditAttendanceController : Controller
    {
        //
        // GET: /EditAttendance/
        public ActionResult Index()
        {
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
            ViewBag.Message = "";
            TempData["Message"] = "";
            return View();
        }
        TAS2013Entities db = new TAS2013Entities();
        
        [HttpPost]
        public ActionResult EditAttWizardOne(FormCollection form)
        {
            try
            {
                ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
                if (Request.Form["EmpNo"].ToString() != "" && Request.Form["DateFrom"].ToString() != "")
                {
                    string _EmpNo = Request.Form["EmpNo"].ToString();
                    DateTime _AttDataFrom = Convert.ToDateTime(Request.Form["DateFrom"].ToString());
                    AttData _attData = new AttData();
                    _attData = db.AttDatas.First(aa => aa.EmpNo == _EmpNo && aa.AttDate == _AttDataFrom);
                    if (_attData != null)
                    {
                        ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", _attData.EmpID);
                        return View(_attData);
                    }
                    else
                        return View("Index");
                }
                else
                    return View("Index");
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("Sequence"))
                    ViewBag.Message = "No Entry found on this particular date";
                return View("Index");

            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAttWizardData([Bind(Include = "EmpDate,AttDate,EmpNo,EmpID,DutyCode,DutyTime,TimeIn,TimeOut,WorkMin,LateIn,LateOut,EarlyIn,EarlyOut,OTMin,GZOTMin,BreakMin,SLMin,StatusP,StatusAB,StatusLI,StatusLO,StatusEI,StatusEO,StatusOT,StatusGZOT,StatusGZ,StatusDO,StatusHD,StatusSL,StatusOD,StatusLeave,StatusMN,StatusIN,StatusBreak,ShifMin,ShfSplit,ProcessIn,Remarks,Tin0,Tout0,Tin1,Tout1,Tin2,Tout2,Tin3,Tout3,Tin4,Tout4,Tin5,Tout5,Tin6,Tout6,Tin7,Tout7,Tin8,Tout8,Tin9,Tout9,Tin10,Tout10,Tin11,Tout11,Tin12,Tout12,Tin13,Tout13,Tin14,Tout14,Tin15,Tout15")] AttData _attData, FormCollection form, string NewDutyCode)
        {
            string _EmpDate = _attData.EmpDate;
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");

            try
            {
                string STimeIn = form["Inhours"].ToString();
                string STimeOut = form["OutHour"].ToString();
                if (TimeValid(STimeIn, STimeOut))
                {
                    TimeSpan _TimeIn = new TimeSpan(0, 0, 0);
                    TimeSpan _TimeOut = new TimeSpan(0, 0, 0);
                    TimeSpan _ThresHoldTimeS = new TimeSpan(20, 00, 00);
                    TimeSpan _ThresHoldTimeE = new TimeSpan(06, 00, 00);
                    string date = Request.Form["Attdate"].ToString();
                    DateTime _AttDate = Convert.ToDateTime(date);

                    DateTime _NewTimeIn = new DateTime();
                    DateTime _NewTimeOut = new DateTime();
                    _NewTimeIn = _AttDate + _TimeIn;
                    if (_TimeIn > _ThresHoldTimeS && _TimeOut < _ThresHoldTimeE)
                    {
                        _NewTimeOut = _AttDate.AddDays(1) + _TimeOut;
                    }
                    else
                    {
                        _NewTimeOut = _AttDate + _TimeOut;
                    }
                    int _UserID = Convert.ToInt32(Session["LogedUserID"].ToString());
                    HelperClass.MyHelper.SaveAuditLog(_UserID, (byte)MyEnums.FormName.EditAttendance, (byte)MyEnums.Operation.Edit, DateTime.Now);
                    ManualAttendanceProcess _pma = new ManualAttendanceProcess(_EmpDate, "", false, _NewTimeIn, _NewTimeOut, NewDutyCode, _UserID);
                    return View("Index");
                }
                else
                {
                    _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
                    return View(_attData);
                }
                
            }
            catch (Exception ex)
            {
                _attData = db.AttDatas.First(aa => aa.EmpDate == _EmpDate);
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
            ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");

        }
        [HttpPost]
        public ActionResult EditAttJobCard()
        {
            string _EmpNo = "";
            _EmpNo = Request.Form["JobEmpNo"];
            if (_EmpNo != "")
            {
                if (Request.Form["JobDateFrom"] != null && Request.Form["JobDateTo"] != null)
                {
                    short _WorkCardID = Convert.ToInt16(Request.Form["JobCardType"].ToString());
                    Emp selectedEmp = db.Emps.FirstOrDefault(aa => aa.EmpNo == _EmpNo);
                    if (selectedEmp != null)
                    {
                        CalculateWorkCardAttendance(selectedEmp, _WorkCardID, Convert.ToDateTime(Request.Form["JobDateFrom"]), Convert.ToDateTime(Request.Form["JobDateTo"]));
                        return View();
                    }
                }
            }
                ViewBag.JobCardType = new SelectList(db.JobCards, "WorkCardID", "WorkCardName");
                return View("Index");              
            
        }

        private void CalculateWorkCardAttendance(Emp _selEmp,short _WorkCardID, DateTime _dateStart, DateTime _dateEnd)
        {
            int _empID = _selEmp.EmpID;
            string _empDate = "";
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            short _CompID = Convert.ToInt16(Session["UserCompany"].ToString());
            DateTime _Date = _dateStart;
            while (_Date <= _dateEnd)
            {
                _empDate = _selEmp.EmpID + _Date.ToString("yyMMdd");
                switch (_WorkCardID)
                {
                    case 1:
                        AddJobCardToDatabase(_empDate, _empID, _Date, _userID, _WorkCardID, _CompID, DateTime.Now);
                        break;
                    case 2:
                        AddJobCardToDatabase(_empDate, _empID, _Date, _userID, _WorkCardID, _CompID, DateTime.Now);
                        break;
                    case 3:
                        AddJobCardToDatabase(_empDate, _empID, _Date, _userID, _WorkCardID, _CompID, DateTime.Now);
                        break;
                    case 4:
                        AddJobCardToDatabase(_empDate, _empID, _Date, _userID, _WorkCardID, _CompID, DateTime.Now);
                        break;
                    case 5:
                        AddJobCardToDatabase(_empDate, _empID, _Date, _userID, _WorkCardID, _CompID, DateTime.Now);
                        break;
                }
                _Date = _Date.AddDays(1);
            }
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.EditAttendance, (byte)MyEnums.Operation.Edit, DateTime.Now);
        }

        private void AddJobCardToDatabase(string _empDate, int _empID, DateTime _currentDate, int _userID, short _WorkCardID, short _CompID, DateTime dateTime)
        {
            JobCardEmp _jobCardEmp = new JobCardEmp();
            _jobCardEmp.EmpDate = _empDate;
            _jobCardEmp.EmpID = _empID;
            _jobCardEmp.Dated = _currentDate;
            _jobCardEmp.SubmittedFrom = _userID;
            _jobCardEmp.WrkCardID = _WorkCardID;
            _jobCardEmp.CompanyID = _CompID;
            _jobCardEmp.DateCreated = dateTime;
            db.JobCardEmps.Add(_jobCardEmp);
            db.SaveChanges();

            // ToDo  ===== check for already processed Attendance
            
        }

	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.HelperClass;
using WMS.Models;

namespace WMS.Controllers
{
    public class RosterController : Controller
    {
        //
        // GET: /Roster/
        private TAS2013Entities db = new TAS2013Entities();
        public ActionResult Index()
        {
            ViewBag.RosterType = new SelectList(db.RosterTypes.OrderBy(s=>s.Name), "ID", "Name");
            ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
            ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
            ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
            return View();
            //return View();
        }

        //
        public ActionResult RosterAppIndex(FormCollection form)
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            List<RosterApp> rosterapps = new List<RosterApp>();
            if (LoggedInUser.UserRoleD == "A")
            {
                rosterapps = db.RosterApps.Where(aa => aa.Status == true).ToList();
            }
            else
            {
                int _UserID = Convert.ToInt32(Session["LogedUserID"].ToString());
                rosterapps = db.RosterApps.Where(aa => aa.Status == true && aa.UserID == _UserID).ToList();
            }
            
            List<RosterApplication> _RosterApplicationsList = new List<RosterApplication>();
            List<Crew> Crews = db.Crews.ToList();
            List<Section> Sections = db.Sections.ToList();
            List<Emp> Emps = db.Emps.ToList();
            List<Shift> shifts = db.Shifts.ToList();
            foreach(var item in rosterapps)
            {
                try
                {
                    RosterApplication _RosterApplication = new RosterApplication();
                    _RosterApplication.RotaApplD = item.RotaApplD;
                    _RosterApplication.DateStarted = item.DateStarted;
                    _RosterApplication.DateEnded = item.DateEnded;
                    _RosterApplication.RosterCriteria = item.RosterCriteria;
                    switch (item.RosterCriteria)
                    {
                        case "S":
                            _RosterApplication.CriteriaData = item.Shift.ShiftName;
                            break;
                        case "C":
                            short CrewID = (short)item.CriteriaData;
                            _RosterApplication.CriteriaData = Crews.Where(aa => aa.CrewID == CrewID).FirstOrDefault().CrewName;
                            break;
                        case "T":
                            short SecID = (short)item.CriteriaData;
                            _RosterApplication.CriteriaData = Sections.Where(aa => aa.SectionID == SecID).FirstOrDefault().SectionName;
                            break;
                        case "E":
                            _RosterApplication.CriteriaData = Emps.Where(aa => aa.EmpID == item.CriteriaData).FirstOrDefault().EmpName;
                            break;
                    }
                    _RosterApplication.WorkMin = item.WorkMin;
                    _RosterApplication.DutyTime = item.DutyTime;
                    _RosterApplication.RosterType = item.RosterType.Name;
                    _RosterApplication.Shift = shifts.First(aa => aa.ShiftID == item.ShiftID).ShiftName;
                    _RosterApplicationsList.Add(_RosterApplication);
                }
                catch (Exception ex)
                {
                }
            }
            return View(_RosterApplicationsList);
        }

        public ActionResult RosterDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RosterDetailsCustom _RosterDetails = new RosterDetailsCustom();
            //_RosterDetails.RosterDetails = db.RosterDetails.Where(aa => aa.RosterAppID == id).ToList();
            //RosterApp _RosterApp = new RosterApp();
            //_RosterApp = db.RosterApps.First(aa => aa.RotaApplD == id);
            ////_RosterDetails
            var rosterdetails = db.RosterDetails.Where(aa=>aa.RosterAppID==id).ToList();
            RosterApp rosterApp = db.RosterApps.First(aa => aa.RotaApplD == id);
            return View(CalculateRosterDetails(rosterdetails,rosterApp));
        }

        private List<RosterDetailAttributes> CalculateRosterDetails(List<Models.RosterDetail> rosterdetails, RosterApp rosterApp)
        {
            //List<RosterDetailModel> rdm = new List<RosterDetailModel>();
            List<RosterDetailAttributes> rda = new List<RosterDetailAttributes>();
            Shift shift = new Shift();
            shift = db.Shifts.First(aa => aa.ShiftID == rosterApp.ShiftID);
            DateTime currentDate = rosterApp.DateStarted.Value;
            List<RosterDetail> tempRotaDetails = new List<RosterDetail>();
            while (currentDate <= rosterApp.DateEnded)
            {
                RosterDetailAttributes rdaS = new RosterDetailAttributes();
                tempRotaDetails = rosterdetails.Where(aa => aa.RosterDate == currentDate).ToList();
                if (tempRotaDetails.Count > 0)
                {
                    rdaS.Changed = true;
                    rdaS.Day = tempRotaDetails.FirstOrDefault().RosterDate.Value.ToString("dddd");
                    rdaS.DutyCode = tempRotaDetails.FirstOrDefault().DutyCode;
                    rdaS.DutyDate = tempRotaDetails.FirstOrDefault().RosterDate.Value;
                    rdaS.DutyTime = (TimeSpan)tempRotaDetails.FirstOrDefault().DutyTime;
                    rdaS.WorkMin = (short)tempRotaDetails.FirstOrDefault().WorkMin;
                }
                else
                {
                    rdaS.Changed = false;
                    rdaS.Day = currentDate.ToString("dddd");
                    int wrkMin = CalculateDutyCode(shift, currentDate);
                    if (wrkMin == 0)
                        rdaS.DutyCode = "R";
                    else
                        rdaS.DutyCode = "D";
                    rdaS.DutyDate = currentDate;
                    rdaS.DutyTime = shift.StartTime;
                    rdaS.WorkMin = wrkMin;
                }
                rda.Add(rdaS);
                currentDate = currentDate.AddDays(1);
            }
            return rda;
        }

        private int CalculateDutyCode(Shift shift, DateTime currentDate)
        {
            string dutyCode = "D";
            int workMin = 0;
            switch (currentDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    workMin=shift.MonMin;
                    break;
                case DayOfWeek.Tuesday:
                    workMin = shift.TueMin;
                    break;
                case DayOfWeek.Wednesday:
                    workMin = shift.WedMin;
                    break;
                case DayOfWeek.Thursday:
                    workMin = shift.ThuMin;
                    break;
                case DayOfWeek.Friday:
                    workMin = shift.FriMin;
                    break;
                case DayOfWeek.Saturday:
                    workMin = shift.SatMin;
                    break;
                case DayOfWeek.Sunday:
                    workMin = shift.SunMin;
                    break;
            }
            return workMin;
        }

        public ActionResult RosterContinue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RosterDetailsCustom _RosterDetails = new RosterDetailsCustom();
            //_RosterDetails.RosterDetails = db.RosterDetails.Where(aa => aa.RosterAppID == id).ToList();
            //RosterApp _RosterApp = new RosterApp();
            //_RosterApp = db.RosterApps.First(aa => aa.RotaApplD == id);
            //_RosterDetails
            RosterApp rosterApp = db.RosterApps.First(aa => aa.RotaApplD == id); 
            return View(CalculateRosterFields((int)rosterApp.RotaTypeID, (DateTime)rosterApp.DateEnded.Value.AddDays(1), (int)rosterApp.WorkMin, (TimeSpan)rosterApp.DutyTime, rosterApp.RosterCriteria, (int)rosterApp.CriteriaData, (int)rosterApp.ShiftID, (int)rosterApp.RotaApplD));
        }

        public ActionResult RosterDelete(int? id)
        {
            ViewBag.RosterType = new SelectList(db.RosterTypes, "ID", "Name");
            ViewBag.ShiftList = new SelectList(db.Shifts, "ShiftID", "ShiftName");
            ViewBag.CrewList = new SelectList(db.Crews, "CrewID", "CrewName");
            ViewBag.SectionList = new SelectList(db.Sections, "SectionID", "SectionName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            DeleteRoster((int)id, _userID);
            return View("Index");
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            int _Shift = Convert.ToInt32(Request.Form["ShiftList"].ToString());
            int _RosterType = Convert.ToInt32(Request.Form["RosterType"].ToString());
            DateTime _StartDate = Convert.ToDateTime(Request.Form["dateStart"]);
            DateTime _EndDate = Convert.ToDateTime(Request.Form["dateEndHidden"]);
            TimeSpan _DutyTime = MyHelper.ConvertTime(Request.Form["dutyTime"]);
            int _WorkMin = Convert.ToInt16(Request.Form["mints"]);
            string Criteria = "";
            bool check = false;
            int RosterCriteriaValue = 0;
                switch (Request.Form["cars"].ToString())
                {
                    case "shift":
                        RosterCriteriaValue = Convert.ToInt32(Request.Form["ShiftList"].ToString());
                        Criteria = "S";
                        break;
                    case "crew":
                        RosterCriteriaValue = Convert.ToInt32(Request.Form["CrewList"].ToString());
                        Criteria = "C";
                        break;
                    case "section":
                        RosterCriteriaValue = Convert.ToInt32(Request.Form["SectionList"].ToString());
                        Criteria = "T";
                        break;
                    case "employee":
                        
                        break;
                }
            if (check == false)
            {
                RosterApp ra = new RosterApp()
                {
                    DateStarted = _StartDate,
                    DateEnded = _EndDate.AddDays(-1),
                    DateCreated = DateTime.Now,
                    RosterCriteria = Criteria,
                    CriteriaData = RosterCriteriaValue,
                    DutyTime = _DutyTime,
                    RotaTypeID = (byte)_RosterType,
                    WorkMin = (short)_WorkMin,
                    Status = true,
                    ShiftID = (byte)_Shift,
                    UserID = Convert.ToInt32(Session["LogedUserID"].ToString())
                };
                db.RosterApps.Add(ra);
                db.SaveChanges();

                return View(CalculateRosterFields(_RosterType, _StartDate, _WorkMin, _DutyTime, Criteria, RosterCriteriaValue, _Shift, ra.RotaApplD));
            }
            else
            {
                ViewBag.RosterType = new SelectList(db.RosterTypes.OrderBy(s=>s.Name), "ID", "Name");
                ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
                ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
                ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult CreateRoster(FormCollection form)
        {

            int shiftID = Convert.ToInt16(Request.Form["shiftId"].ToString());
            string criteria = Request.Form["criteria"].ToString();
            int criteriaValue = Convert.ToInt16(Request.Form["criteriaValue"].ToString());
            int noOfDays = Convert.ToInt16(Request.Form["noOfDays"].ToString());
            DateTime startDate = Convert.ToDateTime(Request.Form["startDate"]);
            List<RosterAttributes> rosters = new List<RosterAttributes>();
            Shift _selectedShift = db.Shifts.FirstOrDefault(ss => ss.ShiftID == shiftID);
            int _RotaAppID = Convert.ToInt32(Request.Form["RotaAppID"].ToString());
           

            for (int i = 0; i < noOfDays; i++)
            {
                rosters.Add(new RosterAttributes()
                {
                    DutyTime = MyHelper.ConvertTime(Request.Form["StudentList" + i.ToString() + "DutyTime"].ToString()),
                    DutyDate = Convert.ToDateTime(Request.Form["StudentList" + i.ToString() + "Date"]),
                    WorkMin = Convert.ToInt32(Request.Form["StudentList" + i.ToString() + "WorkMin"])
                });
            }
            CreateRosterEntries(_selectedShift, criteria, criteriaValue, startDate, noOfDays, rosters, _RotaAppID);


            ViewBag.RosterType = new SelectList(db.RosterTypes.OrderBy(s=>s.Name), "ID", "Name");
            ViewBag.ShiftList = new SelectList(db.Shifts.OrderBy(s=>s.ShiftName), "ShiftID", "ShiftName");
            ViewBag.CrewList = new SelectList(db.Crews.OrderBy(s=>s.CrewName), "CrewID", "CrewName");
            ViewBag.SectionList = new SelectList(db.Sections.OrderBy(s=>s.SectionName), "SectionID", "SectionName");
            return View("Index");
        }

        private void CreateRosterEntries(Shift _selectedShift, string criteria, int criteriaValue, DateTime startDate, int noOfDays, List<RosterAttributes> rosters, int _RotaAppID)
        {
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            foreach (var roster in rosters)
            {
                if (isRosterValueChanged(roster, _selectedShift))
                {
                    RosterDetail _RotaDetail = new RosterDetail();
                    _RotaDetail.CriteriaValueDate = criteria.ToString() + criteriaValue.ToString() + roster.DutyDate.ToString("yyMMdd");
                    _RotaDetail.OpenShift = _selectedShift.OpenShift;
                    _RotaDetail.UserID = _userID;
                    _RotaDetail.RosterAppID = _RotaAppID;
                    if (roster.WorkMin == 0)
                    {
                        _RotaDetail.DutyCode = "R";
                    }
                    else
                    {
                        _RotaDetail.DutyCode = "D";
                    }
                    if (roster.DutyTime == new TimeSpan(0, 0, 0))
                    {
                        _RotaDetail.OpenShift = true;
                    }
                    else
                    {
                        _RotaDetail.OpenShift = false;
                    }
                    _RotaDetail.DutyTime = roster.DutyTime;
                    _RotaDetail.WorkMin = (short)roster.WorkMin;
                    _RotaDetail.RosterDate = roster.DutyDate;
                    db.RosterDetails.Add(_RotaDetail);
                    db.SaveChanges();
                }
            }
        }

        private bool isRosterValueChanged(RosterAttributes roster, Shift _selectedShift)
        {
            DayOfWeek day = roster.DutyDate.DayOfWeek;
            bool isChanged = roster.DutyTime == _selectedShift.StartTime ? false : true;
            switch (day)
            {
                case DayOfWeek.Monday:
                    if (roster.WorkMin != _selectedShift.MonMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Tuesday:
                    if (roster.WorkMin != _selectedShift.TueMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Wednesday:
                    if (roster.WorkMin != _selectedShift.WedMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Thursday:
                    if (roster.WorkMin != _selectedShift.ThuMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Friday:
                    if (roster.WorkMin != _selectedShift.FriMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Saturday:
                    if (roster.WorkMin != _selectedShift.SatMin)
                        isChanged = true;
                    break;
                case DayOfWeek.Sunday:
                    if (roster.WorkMin != _selectedShift.SunMin)
                        isChanged = true;
                    break;
            }
            return isChanged;
        }

        private RosterModel CalculateRosterFields(int _RosterType, DateTime _StartDate,int _WorkMin,TimeSpan _DutyTime, string _Criteria, int _CriteriaValue, int _Shift,int _RotaAppID)
        {
            RosterModel _objstudentmodel = new RosterModel();
            try
            {
                int endPoint = 0;
                if (_RosterType == 2)
                    endPoint = 7;
                else if (_RosterType == 3)
                    endPoint = 15;
                else if (_RosterType == 4)
                {
                    endPoint = System.DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
                }
                else if (_RosterType == 5)
                {
                    endPoint = 84;
                }
                _objstudentmodel._RosterAttributes = new List<RosterAttributes>();
                for (int i = 1; i <= endPoint; i++)
                {
                    string _day = _StartDate.Date.ToString("dddd");
                    string _date = _StartDate.Date.ToString("dd-MMM-yyyy");
                    string _DTime = _DutyTime.Hours.ToString("00") + _DutyTime.Minutes.ToString("00");
                    _objstudentmodel.Criteria = ConvertCriteriaAbrvToFull(_Criteria);
                    _objstudentmodel.RotaAppID = _RotaAppID;
                    _objstudentmodel.CriteriaValue = _CriteriaValue;
                    _objstudentmodel.ShiftID = _Shift;
                    _objstudentmodel.StartDate = _StartDate;
                    _objstudentmodel.NoOfDays = endPoint;
                    _objstudentmodel.CriteriaValueName = GetCriteriaValueName(_Criteria, _CriteriaValue);
                    _objstudentmodel.ShiftName = db.Shifts.FirstOrDefault(ss => ss.ShiftID == _Shift).ShiftName;
                    _objstudentmodel._RosterAttributes.Add(new RosterAttributes { ID = i, DateString = _date, Day = _day, DutyDate = _StartDate.Date, DutyTimeString = _DTime, DutyTime = _DutyTime, WorkMin = _WorkMin });
                    _StartDate = _StartDate.AddDays(1);
                }
                RosterApp rosterApp = db.RosterApps.First(aa => aa.RotaApplD == _RotaAppID);
                rosterApp.DateEnded = _StartDate.AddDays(-1);
                db.SaveChanges();
                return _objstudentmodel;
            }
            catch (Exception ex)
            {
                return _objstudentmodel;
            }
        }

        private string ConvertCriteriaAbrvToFull(string _Criteria)
        {
            String Criteria = "";
            switch (_Criteria)
            {
                case "S":
                    Criteria = "Shift";
                    break;
                case "C":
                    Criteria = "Crew";
                    break;
                case "T":
                    Criteria = "Section";
                    break;
                case "E":
                    Criteria = "Employee";
                    break;
            }
            return Criteria;
        }

        private string GetCriteriaValueName(string _Criteria, int criteriaValue)
        {
            String CriteriaValueName = "";
            switch (_Criteria)
            {
                case "S":
                    CriteriaValueName = db.Shifts.FirstOrDefault(aa => aa.ShiftID == criteriaValue).ShiftName;
                    break;
                case "C":
                    CriteriaValueName = db.Crews.FirstOrDefault(aa => aa.CrewID == criteriaValue).CrewName;
                    break;
                case "T":
                    CriteriaValueName = db.Sections.FirstOrDefault(aa => aa.SectionID == criteriaValue).SectionName;
                    break;
                case "E":
                    CriteriaValueName = db.Emps.FirstOrDefault(aa => aa.EmpID == criteriaValue).EmpName;
                    break;
            }
            return CriteriaValueName;
        }
        public bool DeleteRoster(int RosterAppID, int LoggedUserID)
        {
            RosterApp RApp = new RosterApp();
            //User uid = new User();
            RApp = db.RosterApps.First(rr => rr.RotaApplD == RosterAppID);
            if (RApp.UserID == LoggedUserID)
            {
                try
                {
                    List<RosterDetail> RAppDetail = new List<Models.RosterDetail>();
                    RAppDetail = db.RosterDetails.Where(aa => aa.RosterAppID == RosterAppID).ToList();
                    foreach (var item in RAppDetail)
                    {
                        db.RosterDetails.Remove(item);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                db.RosterApps.Remove(RApp);
                db.SaveChanges();
            }

                return true;
        }
        //public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        //{
        //    for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
        //        yield return day;
        //}
	}

    public class RosterApplication
    {
        public int RotaApplD { get; set; }
        public Nullable<System.DateTime> DateStarted { get; set; }
        public Nullable<System.DateTime> DateEnded { get; set; }
        public string RosterCriteria { get; set; }
        public string CriteriaData { get; set; }
        public Nullable<short> WorkMin { get; set; }
        public Nullable<System.TimeSpan> DutyTime { get; set; }
        public string RosterType{ get; set; }
        public string Shift { get; set; }
    }
    public class RosterModel
    {
        public List<RosterAttributes> _RosterAttributes { get; set; }
        public string Criteria { get; set; }
        public int CriteriaValue { get; set; }
        public string CriteriaValueName { get; set; }
        public int ShiftID {get; set;}
        public string ShiftName { get; set; }
        public DateTime StartDate { get; set; }
        public int NoOfDays { get; set; }
        public int RotaAppID { get; set; }
    }
    public class RosterAttributes
    {
        public int ID { get; set; }
        public string DateString { get; set; }
        public string Day { get; set; }
        public DateTime DutyDate { get; set; }
        public int WorkMin { get; set; }
        public string DutyTimeString { get; set; }
        public TimeSpan DutyTime { get; set; }
        }

    public class RosterDetailModel
    {
        public List<RosterDetailAttributes> _RosterAttributes { get; set; }
        public string Criteria { get; set; }
        public int CriteriaValue { get; set; }
        public string CriteriaValueName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public DateTime StartDate { get; set; }
        public int NoOfDays { get; set; }
        public int RotaAppID { get; set; }
    }
    public class RosterDetailAttributes
    {
        public string CriteriaValueDate { get; set; }
        public string Day { get; set; }
        public DateTime DutyDate { get; set; }
        public int WorkMin { get; set; }
        public TimeSpan DutyTime { get; set; }
        public string DutyCode { get; set; }
        public bool Changed { get; set; }
        }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.CustomClass
{
    public class PermanentMonthly
    {
        TAS2013Entities context = new TAS2013Entities();
        List<AttData> EmpAttData = new List<AttData>();
        public AttMnDataPer processPermanentMonthlyAttSingle(DateTime startDate, DateTime endDate, EmpView _Emp, List<AttData> _EmpAttData)
        {
            //Get Attendance data of employee according to selected month
            _attMonth = new AttMnDataPer();
            try
            {
                EmpAttData = _EmpAttData;
            }
            catch (Exception ex)
            {

            }
            string EmpMonth = _Emp.EmpID + endDate.Date.Month.ToString();
            //Check for already processed data
            _attMonth.StartDate = startDate;
            _attMonth.EndDate = endDate;
            _attMonth.PreDays = 0;
            _attMonth.WorkDays = 0;
            _attMonth.AbDays = 0;
            _attMonth.LeaveDays = 0;
            _attMonth.OfficialDutyDays = 0;
            _attMonth.ExpectedWrkTime = 0;
            _attMonth.GZDays = 0;
            _attMonth.RestDays = 0;
            _attMonth.TEarlyIn = 0;
            _attMonth.TEarlyOut = 0;
            _attMonth.TGZOT = 0;
            _attMonth.TLateIn = 0;
            _attMonth.TLateOut = 0;
            _attMonth.TNOT = 0;
            _attMonth.TotalDays = 0;
            _attMonth.TWorkTime = 0;
            _attMonth.OT1 = 0;
            _attMonth.OT2 = 0;
            _attMonth.OT3 = 0;
            _attMonth.OT4 = 0;
            _attMonth.OT5 = 0;
            _attMonth.OT6 = 0;
            _attMonth.OT7 = 0;
            _attMonth.OT8 = 0;
            _attMonth.OT9 = 0;
            _attMonth.OT10 = 0;
            _attMonth.OT11 = 0;
            _attMonth.OT12 = 0;
            _attMonth.OT13 = 0;
            _attMonth.OT14 = 0;
            _attMonth.OT15 = 0;
            _attMonth.OT16 = 0;
            _attMonth.OT17 = 0;
            _attMonth.OT18 = 0;
            _attMonth.OT19 = 0;
            _attMonth.OT20 = 0;
            _attMonth.OT21 = 0;
            _attMonth.OT22 = 0;
            _attMonth.OT23 = 0;
            _attMonth.OT24 = 0;
            _attMonth.OT25 = 0;
            _attMonth.OT26 = 0;
            _attMonth.OT27 = 0;
            _attMonth.OT28 = 0;
            _attMonth.OT29 = 0;
            _attMonth.OT30 = 0;
            _attMonth.OT31 = 0;
            TDays = 0;
            WorkDays = 0;
            PresentDays = 0;
            AbsentDays = 0;
            LeaveDays = 0;
            RestDays = 0;
            GZDays = 0;
            EarlyIn = 0;
            EarlyOut = 0;
            LateIn = 0;
            LateOut = 0;
            WorkTime = 0;
            NOT = 0;
            GOT = 0;
            TDays = Convert.ToByte((endDate - startDate).Days + 1);
            CalculateMonthlyAttendanceSheet(EmpAttData);
            _attMonth.Period = endDate.Date.Month.ToString() + endDate.Date.Year.ToString();
            _attMonth.EmpMonth = EmpMonth;
            _attMonth.EmpID = _Emp.EmpID;
            _attMonth.EmpNo = _Emp.EmpNo;
            _attMonth.EmpName = _Emp.EmpName;
            return _attMonth;
        }

        AttMnDataPer _attMonth = new AttMnDataPer();
        byte TDays = 0;
        byte WorkDays = 0;
        byte PresentDays = 0;
        byte AbsentDays = 0;
        byte LeaveDays = 0;
        byte RestDays = 0;
        byte GZDays = 0;
        Int16 EarlyIn = 0;
        Int16 EarlyOut = 0;
        Int16 LateIn = 0;
        Int16 LateOut = 0;
        Int16 WorkTime = 0;
        Int16 NOT = 0;
        Int16 GOT = 0;
        Int16 ExpectedWorkMins = 0;
        Int16 OfficialVisit = 0;

        private void CalculateMonthlyAttendanceSheet(List<AttData> _EmpAttData)
        {
            foreach (var item in _EmpAttData)
            {
                try
                {
                    Int16 OverTime = 0;
                    //current day is GZ holiday
                    if (item.StatusGZ == true && item.DutyCode == "G")
                    {
                        Marksheet(item.AttDate.Value.Day, "G");
                        GZDays++;
                        if (item.GZOTMin != null && item.GZOTMin > 0)
                        {
                            MarkOverTime(item.AttDate.Value.Day, Convert.ToInt16(item.GZOTMin));
                        }
                    }
                    //if current day is Rest day
                    if (item.StatusDO == true && item.DutyCode == "R")
                    {
                        Marksheet(item.AttDate.Value.Day, "R");
                        RestDays++;
                        if (item.OTMin != null && item.OTMin > 0)
                        {
                            MarkOverTime(item.AttDate.Value.Day, Convert.ToInt16(item.OTMin));
                        }
                    }
                    //current day is leave
                    if (item.StatusLeave == true)
                    {
                        Marksheet(item.AttDate.Value.Day, "L");
                        LeaveDays++;
                    }
                    //if current day is absent
                    if (item.StatusAB == true && item.DutyCode == "D")
                    {
                        if (item.TimeIn == null && item.TimeOut == null)
                        {
                            Marksheet(item.AttDate.Value.Day, "A");
                            AbsentDays++;
                        }
                        if (item.TimeIn != null && item.TimeOut != null)
                        {
                            if (item.TimeIn.Value.TimeOfDay == item.TimeOut.Value.TimeOfDay)
                            {
                                Marksheet(item.AttDate.Value.Day, "A");
                                AbsentDays++;
                            }
                        }

                    }
                    //currentday is present
                    if (item.TimeIn != null && item.TimeOut != null)
                    {
                        if (item.DutyCode == "D")
                        {
                            Marksheet(item.AttDate.Value.Day, "P");
                            if (item.StatusDO != true && item.StatusGZ != true)
                            {
                                PresentDays++;
                            }
                            if (item.OTMin != null && item.OTMin > 0)
                            {
                                OverTime = (Int16)(OverTime + Convert.ToInt16(item.OTMin));
                            }
                            if (item.EarlyIn != null && item.EarlyIn > 0)
                            {
                                //OverTime = (Int16)(OverTime + Convert.ToInt16(item.EarlyIn));
                            }
                            if (item.OTMin > 0)
                            {
                                MarkOverTime(item.AttDate.Value.Day, Convert.ToInt16(item.OTMin));
                            }
                            if (item.GZOTMin > 0)
                            {
                                MarkOverTime(item.AttDate.Value.Day, Convert.ToInt16(item.GZOTMin));
                            }
                        }
                    }
                    //Manual 
                    if (item.StatusMN == true && item.DutyCode == "D")
                    {
                        if (item.TimeIn == null && item.TimeOut == null)
                        {
                            if (item.StatusP == true)
                            {
                                if (item.StatusDO != true && item.StatusAB != true)
                                {
                                    if (!item.Remarks.Contains("[Official Duty]"))
                                    {
                                        Marksheet(item.AttDate.Value.Day, "P");
                                        PresentDays++;
                                    }
                                }
                            }
                        }
                    }
                    if (item.Remarks != null)
                    {
                        if (item.Remarks.Contains("[Official Duty]"))
                        {
                            Marksheet(item.AttDate.Value.Day, "O");
                            PresentDays++;
                            OfficialVisit++;
                        }
                        if (item.Remarks.Contains("[Badli]"))
                        {
                            if (!item.Remarks.Contains("[Official Duty]"))
                            {
                                Marksheet(item.AttDate.Value.Day, "B");
                                PresentDays++;
                            }
                        }
                    }
                    //Missing Attendance
                    if ((item.TimeIn == null && item.TimeOut != null) || (item.TimeIn != null && item.TimeOut == null))
                    {
                        Marksheet(item.AttDate.Value.Day, "I");
                    }
                    //Sum EarlyIn/Out, LateIn/Out, WorkTime, NOT, GOT
                    if (item.EarlyIn != null && item.EarlyIn > 0)
                        EarlyIn = (Int16)(EarlyIn + Convert.ToInt16(item.EarlyIn));
                    if (item.EarlyOut != null && item.EarlyOut > 0)
                        EarlyOut = (Int16)(EarlyOut + Convert.ToInt16(item.EarlyOut));
                    if (item.LateIn != null && item.LateIn > 0)
                        LateIn = (Int16)(LateIn + Convert.ToInt16(item.LateIn));
                    if (item.OTMin != null && item.OTMin > 0)
                        NOT = (Int16)(NOT + Convert.ToInt16(item.OTMin));
                    if (item.GZOTMin != null && item.GZOTMin > 0)
                        GOT = (Int16)(GOT + Convert.ToInt16(item.GZOTMin));
                    if (item.WorkMin != null && item.WorkMin > 0)
                        WorkTime = (Int16)(WorkTime + Convert.ToInt16(item.WorkMin));
                    if (item.ShifMin > 0)
                        ExpectedWorkMins = (short)(ExpectedWorkMins + item.ShifMin);
                }
                catch (Exception ex)
                {

                }
            }
            //
            _attMonth.TotalDays = TDays;
            _attMonth.PreDays = PresentDays;
            _attMonth.AbDays = AbsentDays;
            _attMonth.LeaveDays = LeaveDays;
            _attMonth.RestDays = RestDays;
            _attMonth.GZDays = GZDays;
            _attMonth.WorkDays = (byte)(PresentDays + RestDays + GZDays + LeaveDays);

            _attMonth.TEarlyIn = EarlyIn;
            _attMonth.TEarlyOut = EarlyOut;
            _attMonth.TLateIn = LateIn;
            _attMonth.TWorkTime = WorkTime;
            _attMonth.TGZOT = GOT;
            _attMonth.TNOT = NOT;
            _attMonth.ExpectedWrkTime = ExpectedWorkMins;
            _attMonth.OfficialDutyDays = (byte)OfficialVisit;

        }

        public void Marksheet(int day, string _Code)
        {
            switch (day)
            {
                case 1:
                    _attMonth.D1 = _Code;
                    break;
                case 2:
                    _attMonth.D2 = _Code;
                    break;
                case 3:
                    _attMonth.D3 = _Code;
                    break;
                case 4:
                    _attMonth.D4 = _Code;
                    break;
                case 5:
                    _attMonth.D5 = _Code;
                    break;
                case 6:
                    _attMonth.D6 = _Code;
                    break;
                case 7:
                    _attMonth.D7 = _Code;
                    break;
                case 8:
                    _attMonth.D8 = _Code;
                    break;
                case 9:
                    _attMonth.D9 = _Code;
                    break;
                case 10:
                    _attMonth.D10 = _Code;
                    break;
                case 11:
                    _attMonth.D11 = _Code;
                    break;
                case 12:
                    _attMonth.D12 = _Code;
                    break;
                case 13:
                    _attMonth.D13 = _Code;
                    break;
                case 14:
                    _attMonth.D14 = _Code;
                    break;
                case 15:
                    _attMonth.D15 = _Code;
                    break;
                case 16:
                    _attMonth.D16 = _Code;
                    break;
                case 17:
                    _attMonth.D17 = _Code;
                    break;
                case 18:
                    _attMonth.D18 = _Code;
                    break;
                case 19:
                    _attMonth.D19 = _Code;
                    break;
                case 20:
                    _attMonth.D20 = _Code;
                    break;
                case 21:
                    _attMonth.D21 = _Code;
                    break;
                case 22:
                    _attMonth.D22 = _Code;
                    break;
                case 23:
                    _attMonth.D23 = _Code;
                    break;
                case 24:
                    _attMonth.D24 = _Code;
                    break;
                case 25:
                    _attMonth.D25 = _Code;
                    break;
                case 26:
                    _attMonth.D26 = _Code;
                    break;
                case 27:
                    _attMonth.D27 = _Code;
                    break;
                case 28:
                    _attMonth.D28 = _Code;
                    break;
                case 29:
                    _attMonth.D29 = _Code;
                    break;
                case 30:
                    _attMonth.D30 = _Code;
                    break;
                case 31:
                    _attMonth.D31 = _Code;
                    break;
            }
        }

        //For OT
        public void MarkOverTime(int day, Int16 _OTMin)
        {
            switch (day)
            {
                case 1:
                    _attMonth.OT1 = _OTMin;
                    break;
                case 2:
                    _attMonth.OT2 = _OTMin;
                    break;
                case 3:
                    _attMonth.OT3 = _OTMin;
                    break;
                case 4:
                    _attMonth.OT4 = _OTMin;
                    break;
                case 5:
                    _attMonth.OT5 = _OTMin;
                    break;
                case 6:
                    _attMonth.OT6 = _OTMin;
                    break;
                case 7:
                    _attMonth.OT7 = _OTMin;
                    break;
                case 8:
                    _attMonth.OT8 = _OTMin;
                    break;
                case 9:
                    _attMonth.OT9 = _OTMin;
                    break;
                case 10:
                    _attMonth.OT10 = _OTMin;
                    break;
                case 11:
                    _attMonth.OT11 = _OTMin;
                    break;
                case 12:
                    _attMonth.OT12 = _OTMin;
                    break;
                case 13:
                    _attMonth.OT13 = _OTMin;
                    break;
                case 14:
                    _attMonth.OT14 = _OTMin;
                    break;
                case 15:
                    _attMonth.OT15 = _OTMin;
                    break;
                case 16:
                    _attMonth.OT16 = _OTMin;
                    break;
                case 17:
                    _attMonth.OT17 = _OTMin;
                    break;
                case 18:
                    _attMonth.OT18 = _OTMin;
                    break;
                case 19:
                    _attMonth.OT19 = _OTMin;
                    break;
                case 20:
                    _attMonth.OT20 = _OTMin;
                    break;
                case 21:
                    _attMonth.OT21 = _OTMin;
                    break;
                case 22:
                    _attMonth.OT22 = _OTMin;
                    break;
                case 23:
                    _attMonth.OT23 = _OTMin;
                    break;
                case 24:
                    _attMonth.OT24 = _OTMin;
                    break;
                case 25:
                    _attMonth.OT25 = _OTMin;
                    break;
                case 26:
                    _attMonth.OT26 = _OTMin;
                    break;
                case 27:
                    _attMonth.OT27 = _OTMin;
                    break;
                case 28:
                    _attMonth.OT28 = _OTMin;
                    break;
                case 29:
                    _attMonth.OT29 = _OTMin;
                    break;
                case 30:
                    _attMonth.OT30 = _OTMin;
                    break;
                case 31:
                    _attMonth.OT31 = _OTMin;
                    break;
            }
        }
    }
}
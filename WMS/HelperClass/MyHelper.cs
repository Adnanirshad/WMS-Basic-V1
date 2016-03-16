using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Models;
using WMSLibrary;

namespace WMS.HelperClass
{
    public static class MyHelper
    {
        public static void SaveAuditLog(int _userID,short _form,short _operation,DateTime _date)
        {
            using (var ctx = new TAS2013Entities())
            {
                AuditLog auditEntry = new AuditLog();
                auditEntry.AuditUserID = _userID;
                auditEntry.FormID = _form;
                auditEntry.OperationID = _operation;
                auditEntry.AuditDateTime = _date;
                ctx.AuditLogs.Add(auditEntry);
                ctx.SaveChanges();
            }
        }
        public static TimeSpan ConvertTime(string p)
        {
            try
            {
                string hour = "";
                string min = "";
                int count = 0;
                int chunkSize = 2;
                int stringLength = 4;

                for (int i = 0; i < stringLength; i += chunkSize)
                {
                    count++;
                    if (count == 1)
                    {
                        hour = p.Substring(i, chunkSize);
                    }
                    if (count == 2)
                    {
                        min = p.Substring(i, chunkSize);
                    }
                    if (i + chunkSize > stringLength)
                    {
                        chunkSize = stringLength - i;
                    }
                }
                TimeSpan _currentTime = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), 00);
                return _currentTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay;
            }
        }
        public static bool CheckforPermission(User _User, ReportName _report)
        {
            bool check = false;
            try
            {
                switch (_report)
                {
                    case ReportName.Audit:
                        if (_User.MRAudit == true)
                            check = true;
                        break;
                    case ReportName.Daily:
                        if (_User.MRDailyAtt == true)
                            check = true;
                        break;
                    case ReportName.Detail:
                        if (_User.MRDetail == true)
                            check = true;
                        break;
                    case ReportName.Employee:
                        if (_User.MREmployee == true)
                            check = true;
                        break;
                    case ReportName.Grpah:
                        if (_User.MRGraph == true)
                            check = true;
                        break;
                    case ReportName.Leave:
                        if (_User.MRLeave == true)
                            check = true;
                        break;
                    case ReportName.ManualAtt:
                        if (_User.MRManualEditAtt == true)
                            check = true;
                        break;
                    case ReportName.Monthly:
                        if (_User.MRMonthly == true)
                            check = true;
                        break;
                    case ReportName.Summary:
                        if (_User.MRSummary == true)
                            check = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                check = false;
            }
            return check;
        }

        public enum ReportName
        {
            Daily = 1,
            Leave,
            Monthly,
            Audit,
            ManualAtt,
            Employee,
            Detail,
            Summary,
            Grpah
        }
        public static bool UserHasValuesInSession(FiltersModel fm)
        {
            bool check = false;
            //if (fm.CompanyFilter.Count > 0)
            //    check = true;
            if (fm.LocationFilter.Count > 0)
                check = true;
            //if (fm.DivisionFilter.Count > 0)
            //    check = true;
            if (fm.ShiftFilter.Count > 0)
                check = true;
            if (fm.DepartmentFilter.Count > 0)
                check = true;
            if (fm.SectionFilter.Count > 0)
                check = true;
            if (fm.TypeFilter.Count > 0)
                check = true;
            //if (fm.CrewFilter.Count > 0)
            //    check = true;
            if (fm.EmployeeFilter.Count > 0)
                check = true;
            return check;
        }
    }
}
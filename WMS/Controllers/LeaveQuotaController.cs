using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using PagedList;
namespace WMS.Controllers
{
    public class LeaveQuotaController : Controller
    {
        TAS2013Entities db = new TAS2013Entities(); 
        //
        // GET: /LeaveQuota/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            List<LvConsumed> LeavesQuota = new List<LvConsumed>();
            LeavesQuota = db.LvConsumeds.ToList();
            List<LeaveQuotaModel> _leavesQuotaModel = new List<LeaveQuotaModel>();
            foreach (var item in LeavesQuota)
            {
                if (_leavesQuotaModel.Where(aa => aa.EmpID == item.EmpID).Count() > 0)
                {
                    switch (item.LeaveType)
                    {
                        case "A"://casual
                            _leavesQuotaModel.Where(aa => aa.EmpID == item.EmpID).FirstOrDefault().CL = (float)item.YearRemaining;
                            break;
                        case "B"://anual
                            _leavesQuotaModel.Where(aa => aa.EmpID == item.EmpID).FirstOrDefault().AL = (float)item.YearRemaining;
                            break;
                        case "C"://sick
                            _leavesQuotaModel.Where(aa => aa.EmpID == item.EmpID).FirstOrDefault().SL = (float)item.YearRemaining;
                            break;
                    }
                }
                else
                {
                    LeaveQuotaModel lvModel = new LeaveQuotaModel();
                    lvModel.EmpID = item.Emp.EmpID;
                    lvModel.EmpNo = item.Emp.EmpNo;
                    lvModel.EmpName = item.Emp.EmpName;
                    lvModel.SectionName = item.Emp.Section.SectionName;
                    switch (item.LeaveType)
                    {
                        case "A"://casual
                            lvModel.CL = (float)item.YearRemaining;
                            break;
                        case "B"://anual
                            lvModel.AL = (float)item.YearRemaining;
                            break;
                        case "C"://sick
                            lvModel.SL = (float)item.YearRemaining;
                            break;
                    }
                    _leavesQuotaModel.Add(lvModel);
                }
            }
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                    try
                    {
                        _leavesQuotaModel = _leavesQuotaModel.Where(s => s.EmpName.ToUpper().Contains(searchString.ToUpper())
                         || s.EmpNo.ToUpper().Contains(searchString.ToUpper())
                         || s.SectionName.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    catch (Exception ex)
                    {

                    }
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(_leavesQuotaModel.ToPagedList(pageNumber, pageSize));
            //return View(_leavesQuotaModel);
        }
	}

    public class LeaveQuotaModel
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string SectionName{ get; set; }
        public float AL { get; set; }
        public float CL { get; set; }
        public float SL { get; set; }
    }
}
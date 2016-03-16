using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.CustomClass;
using WMS.Models;
using WMSLibrary;

namespace WMS.Controllers
{
    public class GraphController : Controller
    {
        //
        // GET: /Graph/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetInitialValues()
        {
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            var getInitialValues = GetInitialValuesFromFilters(fm);
            return Json(getInitialValues, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDatesValues()
        {
            DateTime dateFrom = new DateTime();
            DateTime dateTo = new DateTime();
            List<DateTime> dtlist = new List<DateTime>();
            FiltersModel fm = Session["FiltersModel"] as FiltersModel;
            List<string> list = Session["ReportSession"] as List<string>;
            dateFrom = Convert.ToDateTime(list[0]);
            dateTo = Convert.ToDateTime(list[1]);
            dtlist.Add(dateFrom);
            dtlist.Add(dateTo);
            return Json(list, JsonRequestBehavior.AllowGet);

            
        }



        private List<CriteriaFoAngular> GetInitialValuesFromFilters(FiltersModel fm)
        {
            List<CriteriaFoAngular> populateData = new List<CriteriaFoAngular>();
           // CriteriaFoAngular cfa = new CriteriaFoAngular();
            //if (fm.CompanyFilter.Count > 0)
            //    populateData.Add(new CriteriaFoAngular("C","Company"));
             if (fm.LocationFilter.Count > 0)
                populateData.Add(new CriteriaFoAngular("L", "Location"));
            if (fm.SectionFilter.Count > 0)
                populateData.Add(new CriteriaFoAngular("E", "Section"));
            if (fm.ShiftFilter.Count > 0)
                populateData.Add(new CriteriaFoAngular("S", "Shift"));
            if (fm.DepartmentFilter.Count > 0)
                populateData.Add(new CriteriaFoAngular("D", "Department"));

                
            return populateData;
        }
        [HttpPost]
        public ActionResult GetGraphValuesForMultipleSelectAndDifferentDate(string GeneralCriteria, List<string> Ids, string datefrom, string dateto)
        {
            List<DailySummary> GetListOfDailySummaries = new List<DailySummary>();
            using (TAS2013Entities dc = new TAS2013Entities())
            {
                
                    foreach (var id in Ids)
                    {
                        for (DateTime date = Convert.ToDateTime(datefrom); date.Date <= Convert.ToDateTime(dateto).Date; date = date.AddDays(1))
                        {
                            string query = date.Year.ToString().Substring(2) + (("0" + date.Month).Count() != 2 ? date.Month + "" : ("0" + date.Month)) + (("0" + date.Day).Count() != 2 ? date.Day + "" : ("0" + date.Day)) + GeneralCriteria + id;
                        DailySummary ds = dc.DailySummaries.Where(aa => aa.SummaryDateCriteria == query).FirstOrDefault();
                        GetListOfDailySummaries.Add(ds);

                    }


                }

                return Json(GetListOfDailySummaries, JsonRequestBehavior.AllowGet);

            }
        
        }
        [HttpPost]
        public ActionResult GetGraphValuesForDifferentDatesSS(string CriteriaValue, string datefrom,string dateto)
        {
            List<DailySummary> GetListOfDailySummaries = new List<DailySummary>();
            string criteria = CriteriaValue.Substring(0,1);
            string value  = CriteriaValue.Substring(1);
            if (value.ToLower() != "null")
            {
                using (TAS2013Entities dc = new TAS2013Entities())
                {

                    for (DateTime date = Convert.ToDateTime(datefrom); date.Date <= Convert.ToDateTime(dateto).Date; date = date.AddDays(1))
                    {
                        string query = date.Year.ToString().Substring(2) + (("0" + date.Month).Count() != 2 ? date.Month + "" : ("0" + date.Month)) + (("0" + date.Day).Count() != 2 ? date.Day + "" : ("0" + date.Day)) + criteria + value;
                        short criteriav = Convert.ToInt16(value);
                        DailySummary ds = dc.DailySummaries.Where(aa => aa.SummaryDateCriteria == query).First();
                        GetListOfDailySummaries.Add(ds);

                    }

                }
            }
            return Json(GetListOfDailySummaries, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetGraphValues(string CriteriaValue)
        {

            using (TAS2013Entities dc = new TAS2013Entities())
            {
                DailySummary ds = dc.DailySummaries.Where(aa => aa.SummaryDateCriteria == CriteriaValue).FirstOrDefault();
                return Json(ds, JsonRequestBehavior.AllowGet);

            }


        }

        [HttpPost]
        public ActionResult GetGraphValuesForMultipleSelect(string GeneralCriteria, List<string> Ids)
        {
            List<DailySummary> GetListOfDailySummaries = new List<DailySummary>();
            foreach (string id in Ids)
            {
                string CriteriaValue = "";
                CriteriaValue = GeneralCriteria + id;
              using (TAS2013Entities dc = new TAS2013Entities())
                {
                    DailySummary ds = dc.DailySummaries.Where(aa => aa.SummaryDateCriteria == CriteriaValue).FirstOrDefault();
                    GetListOfDailySummaries.Add(ds);
                    
                }
            }
            return Json(GetListOfDailySummaries, JsonRequestBehavior.AllowGet);

            
        }
        [HttpPost]
        public ActionResult GetBestCriteria(string CriteriaValue)
        {
            List<DailySummary> BestCriteria = new List<DailySummary>();
            using (TAS2013Entities dc = new TAS2013Entities())
            {
                DateTime To = (DateTime)dc.DailySummaries.Max(x => x.Date);
                DateTime From = To.AddDays(-20);
                var GetSummaries = dc.DailySummaries.Where(aa => aa.Criteria == CriteriaValue && (aa.Date >= From && aa.Date <= To)).ToList();
                var DistinctCriterias = GetSummaries.Select(aa => new { aa.CriteriaValue }).Distinct().ToList();
                foreach (var distinctcriteria in DistinctCriterias)
                {
                    DailySummary gatherinfo = new DailySummary();

                    List<DailySummary> intermediate = GetSummaries.Where(aa => aa.CriteriaValue == distinctcriteria.CriteriaValue).ToList();
                    gatherinfo = InitializeDailySummaryValues(intermediate);

                    foreach (DailySummary inter in intermediate)
                    {
                        gatherinfo = AddValues(gatherinfo, inter);

                    }
                    BestCriteria.Add(gatherinfo);

                }
            }
            Console.Write(CriteriaValue);
            return Json(BestCriteria, JsonRequestBehavior.AllowGet);


        }

        private DailySummary AddValues(DailySummary gatherinfo, DailySummary inter)
        {

            gatherinfo.ActualWorkMins = gatherinfo.ActualWorkMins + inter.ActualWorkMins;
            gatherinfo.ExpectedWorkMins = gatherinfo.ExpectedWorkMins + inter.ExpectedWorkMins;
            return gatherinfo;

        }

        private DailySummary InitializeDailySummaryValues(List<DailySummary> intermediate)
        {
            DailySummary gatherinfo = new DailySummary();
            gatherinfo.CriteriaName = intermediate.FirstOrDefault().CriteriaName;
            gatherinfo.CriteriaValue = intermediate.FirstOrDefault().CriteriaValue;
            gatherinfo.Criteria = intermediate.FirstOrDefault().Criteria;
            gatherinfo.AActualMins = 0;
            gatherinfo.AbsentEmps = 0;
            gatherinfo.ActualWorkMins = 0;
            gatherinfo.AEIMins = 0;
            gatherinfo.AEOMins = 0;
            gatherinfo.AExpectedMins = 0;
            gatherinfo.ALIMins = 0;
            gatherinfo.ALOMins = 0;
            gatherinfo.ALossMins = 0;
            gatherinfo.AOTMins = 0;

            gatherinfo.EIEmps = 0;
            gatherinfo.EIMins = 0;
            gatherinfo.EOEmps = 0;
            gatherinfo.EOMins = 0;
            gatherinfo.ExpectedWorkMins = 0;
            gatherinfo.HalfLvEmps = 0;
            gatherinfo.LIEmps = 0;
            gatherinfo.LIMins = 0;
            gatherinfo.LOEmps = 0;
            gatherinfo.LOMins = 0;
            gatherinfo.LossWorkMins = 0;
            gatherinfo.LvEmps = 0;
            gatherinfo.OnTimeEmps = 0;
            gatherinfo.OTEmps = 0;
            gatherinfo.OTMins = 0;
            gatherinfo.PresentEmps = 0;
            gatherinfo.ShortLvEmps = 0;
            gatherinfo.TotalEmps = 0;
            return gatherinfo;
        }
        [HttpPost]
        public ActionResult GetCriteriaNames(string Criteria)
        {
            List<CriteriaFoAngular> GetValueNames = new List<CriteriaFoAngular>();
            using (TAS2013Entities dc = new TAS2013Entities())
            {
                FiltersModel fm = Session["FiltersModel"] as FiltersModel;
                //if (Criteria == "C")
                //{
                //    foreach (var company in fm.CompanyFilter)
                //        GetValueNames.Add(new CriteriaFoAngular("" + company.ID, company.FilterName));
                
                //}
                if (Criteria == "L")
                {
                    foreach (var location in fm.LocationFilter)
                        GetValueNames.Add(new CriteriaFoAngular("" + location.ID, location.FilterName));
                }
                if (Criteria == "S")
                {
                    foreach (var shift in fm.ShiftFilter)
                        GetValueNames.Add(new CriteriaFoAngular("" + shift.ID, shift.FilterName));
                }
                if (Criteria == "E")
                {
                    foreach (var section in fm.SectionFilter)
                        GetValueNames.Add(new CriteriaFoAngular("" + section.ID, section.FilterName));
                }
                if (Criteria == "D")
                {
                    foreach (var department in fm.DepartmentFilter)
                        GetValueNames.Add(new CriteriaFoAngular("" + department.ID, department.FilterName));
                }
                //var getSummaries = dc.DailySummaries.Where(aa => aa.Criteria == Criteria).Select(aa => new { aa.CriteriaValue, aa.CriteriaName }).Distinct().ToList();
                return Json(
                              GetValueNames.ToArray()
                              
                         , JsonRequestBehavior.AllowGet);
            }



        }
	}
}
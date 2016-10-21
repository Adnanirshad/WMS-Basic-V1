using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using PagedList;
using System.IO;
using System.Web.Helpers;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.CustomClass;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
namespace WMS.Controllers
{
    
    public class AttProcessorsController : Controller
    {
        private TAS2013Entities context = new TAS2013Entities();

        //
        // GET: /AttProcessors/

        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TagSortParm = String.IsNullOrEmpty(sortOrder) ? "tag_desc" : "";
            ViewBag.FromSortParm = sortOrder == "from" ? "from_desc" : "from";
            ViewBag.ToSortParm = sortOrder == "to" ? "to_desc" : "to";
            ViewBag.WhenToSortParm = sortOrder == "whento" ? "whento_desc" : "whento";
            ViewBag.LocationSortParm = sortOrder == "location" ? "location_desc" : "location";
            ViewBag.CatSortParm = sortOrder == "cat" ? "cat_desc" : "cat";

            if (searchString != null)
                page = 1;
             else
               searchString = currentFilter;
            DateTime dtS = DateTime.Today.AddDays(-10);
            DateTime dtE = DateTime.Today.AddDays(1);
            List<AttProcessorScheduler> attprocess = context.AttProcessorSchedulers.Where(aa=>aa.CreatedDate>=dtS&&aa.CreatedDate<=dtE).ToList();
            switch (sortOrder)
            {
                case "tag_desc": attprocess = attprocess.OrderByDescending(s => s.PeriodTag).ToList();                   break;
                case "from_desc":
                    attprocess = attprocess.OrderByDescending(s => s.DateFrom).ToList();
                    break;
                case "from":
                   attprocess = attprocess.OrderBy(s => s.DateFrom).ToList();
                    break;
                case "to_desc":
                    attprocess = attprocess.OrderByDescending(s => s.DateTo).ToList();
                    break;
                case "to":
                    attprocess = attprocess.OrderBy(s => s.DateTo).ToList();
                    break;
               
                default:
                    attprocess = attprocess.OrderBy(s => s.PeriodTag).ToList();
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(attprocess.ToPagedList(pageNumber, pageSize));
           
        }

        //
        // GET: /AttProcessors/Details/5

        public ViewResult Details(int id)
        {
            AttProcessorScheduler attprocessor = context.AttProcessorSchedulers.Single(x => x.AttProcesserSchedulerID == id);
            return View(attprocessor);
        }

        //
        // GET: /AttProcessors/Create

        public ActionResult Create()
        {
            TAS2013Entities db = new TAS2013Entities();
            User LoggedInUser = Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            String query = qb.QueryForCompanyViewLinq(LoggedInUser);
            ViewBag.PeriodTag = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Daily", Value = "D"},
                new SelectListItem { Selected = false, Text = "Monthly", Value = "M"},
                new SelectListItem { Selected = false, Text = "Summary", Value = "S"},

            }, "Value" , "Text",1);
            ViewBag.CriteriaID = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Company", Value = "C"},
                new SelectListItem { Selected = false, Text = "Location", Value = "L"},
                new SelectListItem { Selected = false, Text = "Employee", Value = "E"},

            }, "Value", "Text", 1);
            query = qb.QueryForLocationTableSegerationForLinq(LoggedInUser);

            ViewBag.LocationID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName");

            
              return View();
        } 

        //
        // POST: /AttProcessors/Create

        [HttpPost]
        public ActionResult Create(AttProcessorScheduler attprocessor)
        {
            string d = Request.Form["CriteriaID"].ToString();
            switch (d)
            {
                case "C":
                    attprocessor.Criteria = "C";
                    break;
                case "L": attprocessor.Criteria = "L"; break;
                case "E":
                    {
                        attprocessor.Criteria = "E";
                        string ee = Request.Form["EmpNo"].ToString();
                        List<Emp> empss = new List<Emp>();
                        empss = context.Emps.Where(aa => aa.EmpNo == ee).ToList();
                        if (empss.Count() > 0)
                        {
                            attprocessor.EmpID = empss.First().EmpID;
                            attprocessor.EmpNo = empss.First().EmpNo;
                        }
                    }
                    break;
            }
            attprocessor.ProcessingDone = false;
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            if (ModelState.IsValid)
            {
                attprocessor.UserID = _userID;
                attprocessor.CreatedDate = DateTime.Today;
                context.AttProcessorSchedulers.Add(attprocessor);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }
            return View(attprocessor);
        }
        
        //
        // GET: /AttProcessors/Edit/5
 
        public ActionResult Edit(int id)
        {
            AttProcessorScheduler attprocessor = context.AttProcessorSchedulers.Single(x => x.AttProcesserSchedulerID == id);
            return View(attprocessor);
        }

        //
        // POST: /AttProcessors/Edit/5

        [HttpPost]
        public ActionResult Edit(AttProcessorScheduler attprocessor)
        {
            if (ModelState.IsValid)
            {
                context.Entry(attprocessor).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attprocessor);
        }

        //
        // GET: /AttProcessors/Delete/5
 
        public ActionResult Delete(int id)
        {
            AttProcessorScheduler attprocessor = context.AttProcessorSchedulers.Single(x => x.AttProcesserSchedulerID == id);
            return View(attprocessor);
        }

        //
        // POST: /AttProcessors/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AttProcessorScheduler attprocessor = context.AttProcessorSchedulers.Single(x => x.AttProcesserSchedulerID == id);
            context.AttProcessorSchedulers.Remove(attprocessor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
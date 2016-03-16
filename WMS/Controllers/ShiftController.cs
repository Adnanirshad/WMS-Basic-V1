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
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.CustomClass;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class ShiftController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /Shift/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            User LoggedInUser = Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForLocationSegeration(LoggedInUser);
            DataTable dt = qb.GetValuesfromDB("select * from Shift " + query);
            List<Shift> shift1 = dt.ToList<Shift>();
            var shift = shift1.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                shift = shift.Where(s => s.ShiftName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    shift = shift.OrderByDescending(s => s.ShiftName);
                    break;
                default:
                    shift = shift.OrderBy(s => s.ShiftName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(shift.ToPagedList(pageNumber, pageSize));

        }

        // GET: /Shift/Details/5
         [CustomActionAttribute]
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: /Shift/Create
         [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.DayOff1 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name");
            ViewBag.DayOff2 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name");
            QueryBuilder qb = new QueryBuilder();
            User LoggedInUser = Session["LoggedUser"] as User;
            string query = qb.QueryForLocationTableSegerationForLinq(LoggedInUser);
          
            ViewBag.LocationID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName");
            //ViewBag.RosterType = new SelectList(db.RosterTypes.OrderBy(s=>s.Name), "ID", "Name");
            return View();
        }

        // POST: /Shift/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
         public ActionResult Create([Bind(Include = "ShiftID,ShiftName,StartTime,DayOff1,DayOff2,Holiday,RosterType,MonMin,TueMin,WedMin,ThuMin,FriMin,SatMin,SunMin,LateIn,EarlyIn,EarlyOut,LateOut,OverTimeMin,MinHrs,HasBreak,BreakMin,GZDays,LocationID,OpenShift")] Shift shift, FormCollection form)
        {
            if (string.IsNullOrEmpty(shift.ShiftName))
                ModelState.AddModelError("ShiftName", "Required");
            if (shift.ShiftName != null)
            {
                if (shift.ShiftName.Length > 50)
                    ModelState.AddModelError("ShiftName", "String length exceeds!");
                if (db.Shifts.Where(aa => aa.ShiftName == shift.ShiftName).Count() > 0)
                {
                    ModelState.AddModelError("ShiftName", "Shift Name must be unique");
                }
            }
            if (shift.HasBreak == true)
            {
                if (shift.BreakMin == null)
                {
                    ModelState.AddModelError("BreakMin", "Required");
                }


                if (shift.LateIn == null)
                {
                    ModelState.AddModelError("LateIn", "Required");
                }
                if (shift.EarlyIn == null)
                {
                    ModelState.AddModelError("EarlyIn", "Required");
                }
                if (shift.EarlyOut == null)
                {
                    ModelState.AddModelError("EarlyOut", "Required");
                }
                if (shift.LateOut == null)
                {
                    ModelState.AddModelError("LateOut", "Required");
                }
                if (shift.OverTimeMin == null)
                {
                    ModelState.AddModelError("OverTimeMin", "Required");
                }
                if (shift.MinHrs == null)
                {
                    ModelState.AddModelError("MinHrs", "Required");
                }
            }
            if (ModelState.IsValid)
            {
                var aaa = form["HasBreak"];
                User LoggedInUser = Session["LoggedUser"] as User;
                if (shift.OpenShift == true)
                    shift.StartTime = TimeSpan.Zero;
                //shift.OpenShift = false;
                //shift.HasBreak = false;
                shift.GZDays = shift.Holiday;
                db.Shifts.Add(shift);
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Shift, (byte)MyEnums.Operation.Add, DateTime.Now);
                return RedirectToAction("Index");
                
            }
            ViewBag.LocationID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName");
            ViewBag.DayOff1 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name", shift.DayOff1);
            ViewBag.DayOff2 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name", shift.DayOff2);
             return View(shift);
        }

        // GET: /Shift/Edit/5
         [CustomActionAttribute]
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName",shift.LocationID);
            ViewBag.DayOff1 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name", shift.DayOff1);
            ViewBag.DayOff2 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name", shift.DayOff2);
            return View(shift);
        }

        // POST: /Shift/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include="ShiftID,ShiftName,StartTime,DayOff1,DayOff2,Holiday,RosterType,MonMin,TueMin,WedMin,ThuMin,FriMin,SatMin,SunMin,LateIn,EarlyIn,EarlyOut,LateOut,OverTimeMin,MinHrs,HasBreak,BreakMin,GZDays,OpenShift,LocationID")] Shift shift)
        {
            if (string.IsNullOrEmpty(shift.ShiftName))
                ModelState.AddModelError("ShiftName", "Required");
            if (shift.ShiftName != null)
            {
                if (shift.ShiftName.Length > 50)
                    ModelState.AddModelError("ShiftName", "String length exceeds!");
                //if (db.Shifts.Where(aa => aa.ShiftName == shift.ShiftName).Count() > 0)
                //{
                //    ModelState.AddModelError("ShiftName", "Shift Name must be unique");
                //}
            }
            if (shift.HasBreak == true)
            {
                if (shift.BreakMin == null)
                {
                    ModelState.AddModelError("BreakMin", "Required");
                }
            }
            if(shift.LateIn == null)
            {
                ModelState.AddModelError("LateIn","Required");
            }
            if (ModelState.IsValid)
            {
                User LoggedInUser = Session["LoggedUser"] as User;
                shift.GZDays = shift.Holiday;
                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Shift, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
            ViewBag.DayOff1 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name", shift.DayOff1);
            ViewBag.DayOff2 = new SelectList(db.DaysNames.OrderBy(s=>s.Name), "ID", "Name", shift.DayOff2);
              return View(shift);
        }

        // GET: /Shift/Delete/5
         [CustomActionAttribute]
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: /Shift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(byte id)
        {
            Shift shift = db.Shifts.Find(id);
           
                try
                {
                        db.Shifts.Remove(shift);
                        db.SaveChanges();
                        int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                        HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Shift, (byte)MyEnums.Operation.Delete, DateTime.Now);
                    return RedirectToAction("Index");

                }
                catch (Exception ez)
                {
                    ViewBag.ShiftException = "This shift cannot be deleted.";
                    return View(shift);
                }
            
        }


        public ActionResult ShiftList()
        {

            //QueryBuilder qb = new QueryBuilder();
            //User LoggedInUser = Session["LoggedUser"] as User;
            List<Shift> shifts = db.Shifts.ToList();
            var type = shifts;
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                type.ToArray(),
                                "ShiftID",
                                "ShiftName")
                           , JsonRequestBehavior.AllowGet);

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

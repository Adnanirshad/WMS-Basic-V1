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
using WMS.CustomClass;
using System.Linq.Dynamic;
using WMS.Controllers.Filters;
using WMS.HelperClass;

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class CityController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
        // GET: /City/
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
            List<City> ct = new List<City>();
            ct = db.Cities.ToList();
            var cities = ct.AsEnumerable<City>();
             if (!String.IsNullOrEmpty(searchString))
            {
                cities = cities.Where(s => s.CityName.ToUpper().Contains(searchString.ToUpper())
                     || s.CityName.ToUpper().Contains(searchString.ToUpper()));
                
            }

            switch (sortOrder)
            {
                case "name_desc":
                    cities = cities.OrderByDescending(s => s.CityName);
                    break;
                default:
                    cities = cities.OrderBy(s => s.CityName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(cities.ToPagedList(pageNumber, pageSize));

            //return View(cities.ToList());
        }

        // GET: /City/Details/5
         [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: /City/Create
         [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.RegionID = new SelectList(db.Regions.OrderBy(s=>s.RegionName), "RegionID", "RegionName");
            return View();
        }

        // POST: /City/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "CityID,CityName,RegionID")] City city)
        {
            if (string.IsNullOrEmpty(city.CityName))
                ModelState.AddModelError("CityName", "This field is required!");
            if (city.CityName != null)
            {
                if (city.CityName.Length > 50)
                    ModelState.AddModelError("CityName", "String length exceeds!");
                if (!myClass.IsAllLetters(city.CityName))
                {
                    ModelState.AddModelError("CityName", "This field only contain Alphabets");
                }
                if (CheckDuplicate(city.CityName))
                    ModelState.AddModelError("CityName", "This Type already exist in record, Please select an unique name");
            }

            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                ViewBag.JS = "toastr.success('"+city.CityName+" Successfully created');";
                return RedirectToAction("Index");
            }

            ViewBag.RegionID = new SelectList(db.Regions.OrderBy(s=>s.RegionName), "RegionID", "RegionName", city.RegionID);
            return View(city);
        }
        private bool CheckDuplicate(string _Name)
        {
            var _city = db.Cities;
            foreach (var item in _city)
            {
                if (item.CityName.ToUpper() == _Name.ToUpper())
                    return true;
            }
            return false;
        }
        // GET: /City/Edit/5
         [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionID = new SelectList(db.Regions.OrderBy(s=>s.RegionName), "RegionID", "RegionName", city.RegionID);
            return View(city);
        }

        // POST: /City/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "CityID,CityName,RegionID")] City city)
        {
            if (string.IsNullOrEmpty(city.CityName))
                ModelState.AddModelError("CityName", "This field is required!");
            if (city.CityName != null)
            {
                if (city.CityName.Length > 50)
                    ModelState.AddModelError("CityName", "String length exceeds!");
                if (!myClass.IsAllLetters(city.CityName))
                {
                    ModelState.AddModelError("CityName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegionID = new SelectList(db.Regions.OrderBy(s=>s.RegionName), "RegionID", "RegionName", city.RegionID);
            return View(city);
        }

        // GET: /City/Delete/5
         [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: /City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city);
            db.SaveChanges();
            //int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            //HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.C, (byte)MyEnums.Operation.Add, DateTime.Now);
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

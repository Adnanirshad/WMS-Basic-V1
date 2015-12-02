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
namespace WMS.Controllers
{
     [CustomControllerAttributes]
    public class RegionController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
        // GET: /Region/
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
            string query = qb.QueryForLocationTableSegerationForLinq(LoggedInUser);
            var locations = db.Locations.GroupBy(x => x.CityID).Select(group => group.FirstOrDefault());
            List<City> ct = new List<City>();
            foreach (var loc in locations)
                ct.Add(loc.City);
            var cities = ct.AsEnumerable<City>();
            query = qb.QueryForRegionFromCitiesForLinq(cities);
            var region = db.Regions.Where(query).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                region = region.Where(s => s.RegionName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    region = region.OrderByDescending(s => s.RegionName);
                    break;
                default:
                    region = region.OrderBy(s => s.RegionName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(region.ToPagedList(pageNumber, pageSize));

        }

        // GET: /Region/Details/5
         [CustomActionAttribute]
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // GET: /Region/Create
         [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Region/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "RegionID,RegionName")] Region region)
        {
            if (string.IsNullOrEmpty(region.RegionName))
                ModelState.AddModelError("RegionName", "Required");
            if (region.RegionName != null)
            {
                if (region.RegionName.Length > 50)
                    ModelState.AddModelError("RegionName", "Length exceeds");
                if (!myClass.IsAllLetters(region.RegionName))
                {
                    ModelState.AddModelError("RegionName", "This field only contains Alphabets");
                }
                if (CheckDuplicate(region.RegionName))
                    ModelState.AddModelError("RegionName", "This name already exist in record");
            }
            if (ModelState.IsValid)
            {
                db.Regions.Add(region);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(region);
        }
        private bool CheckDuplicate(string _Name)
        {
            var _regions = db.Regions;
            foreach (var item in _regions)
            {
                if (item.RegionName.ToUpper() == _Name.ToUpper())
                    return true;
            }
            return false;
        }
        // GET: /Region/Edit/5
         [CustomActionAttribute]
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: /Region/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "RegionID,RegionName")] Region region)
        {
            if (string.IsNullOrEmpty(region.RegionName))
                ModelState.AddModelError("RegionName", "Required");
            if (region.RegionName != null)
            {
                if (region.RegionName.Length > 50)
                    ModelState.AddModelError("RegionName", "Length exceeds");
                if (!myClass.IsAllLetters(region.RegionName))
                {
                    ModelState.AddModelError("RegionName", "This field only contains Alphabets");
                }
                if (CheckDuplicate(region.RegionName))
                    ModelState.AddModelError("RegionName", "This name already exist in record");
            }
            if (ModelState.IsValid)
            {
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        // GET: /Region/Delete/5
         [CustomActionAttribute]
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: /Region/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(byte id)
        {
            Region region = db.Regions.Find(id);
            db.Regions.Remove(region);
            db.SaveChanges();
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

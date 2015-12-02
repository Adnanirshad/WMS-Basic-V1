using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WMS.CustomClass;
using WMS.Models;
using WMS.Controllers.Filters;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class SiteController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
        // GET: /Site/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CitySortParm = sortOrder == "city" ? "city_desc" : "city";
            ViewBag.RegionSortParm = sortOrder == "region" ? "region_desc" : "region";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var sites = db.Sites.Include(s => s.City).Include(s => s.City.Region);
            if (!String.IsNullOrEmpty(searchString))
            {
                sites = sites.Where(s => s.SiteName.ToUpper().Contains(searchString.ToUpper())
                     || s.City.CityName.ToUpper().Contains(searchString.ToUpper())
                     || s.City.Region.RegionName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sites = sites.OrderByDescending(s => s.SiteName);
                    break;
                case "city_desc":
                    sites = sites.OrderByDescending(s => s.City.CityName);
                    break;
                case "city":
                    sites = sites.OrderBy(s => s.City.CityName);
                    break;
                case "region_desc":
                    sites = sites.OrderByDescending(s => s.City.Region.RegionName);
                    break;
                case "region":
                    sites = sites.OrderBy(s => s.City.Region.RegionName);
                    break;
                default:
                    sites = sites.OrderBy(s => s.SiteName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(sites.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Site/Details/5
        [CustomActionAttribute]
        public ActionResult Details(short? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // GET: /Site/Create
        [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities.OrderBy(s=>s.CityName), "CityID", "CityName");
            return View();
        }

        // POST: /Site/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "SiteID,SiteName,CityID")] Site site)
        {

            if (string.IsNullOrEmpty(site.SiteName))
                ModelState.AddModelError("SiteName", "This field is required!");
            if (site.SiteName != null)
            {
                if (site.SiteName.Length > 50)
                    ModelState.AddModelError("SiteName", "String length exceeds!");
                if (CheckDuplicate(site.SiteName))
                    ModelState.AddModelError("SiteName", "This Type already exist in record, Please select an unique name");
            }
            if (ModelState.IsValid)
            {
                db.Sites.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities.OrderBy(s=>s.CityName), "CityID", "CityName", site.CityID);
            return View(site);
        }
        private bool CheckDuplicate(string _Name)
        {
            var _sites = db.Sites;
            foreach (var item in _sites)
            {
                if (item.SiteName.ToUpper() == _Name.ToUpper())
                    return true;
            }
            return false;
        }
        // GET: /Site/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities.OrderBy(s=>s.CityName), "CityID", "CityName", site.CityID);
            return View(site);
        }

        // POST: /Site/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "SiteID,SiteName,CityID")] Site site)
        {
            if (string.IsNullOrEmpty(site.SiteName))
                ModelState.AddModelError("SiteName", "This field is required!");
            if (site.SiteName != null)
            {
                if (site.SiteName.Length > 50)
                    ModelState.AddModelError("SiteName", "String length exceeds!");
            }
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities.OrderBy(s=>s.CityName), "CityID", "CityName", site.CityID);
            return View(site);
        }

        // GET: /Site/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: /Site/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
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

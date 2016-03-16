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
using WMS.Controllers.Filters;
using System.Linq.Dynamic;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class LocationController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
             // GET: /Location/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (Session["LogedUserFullname"].ToString() != "")
            {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.SiteSortParm = sortOrder == "site" ? "site_desc" : "site";
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
            User LoggedInUser = Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForLocationTableSegerationForLinq(LoggedInUser);
           
          //  var locations = qb.GetUserLocationsLinq(locations, LoggedInUser);

            var locations = db.Locations.AsQueryable();

            // 
           // var locations = dt.ToList<DerivedLocation>();
         //   var locations = db.Locations.Include(l => l.City).Include(l => l.City.Region);
           // String searchParameter = "SELECT [Extent1].[LocID] AS [LocID],[Extent1].[LocName] AS [LocName], [Extent1].[CityID] AS [CityID] FROM   [dbo].[Location] AS [Extent1] LEFT OUTER JOIN [dbo].[City] AS [Extent2] ON [Extent1].[CityID] = [Extent2].[CityID] LEFT OUTER JOIN [dbo].[Region] AS [Extent3] ON [Extent2].[RegionID] = [Extent3].[RegionID] WHERE (( CAST(CHARINDEX(UPPER(N"+searchString+"), UPPER([Extent1].[LocName])) AS int)) > 0) OR (( CAST(CHARINDEX(UPPER(N"+searchString+"), UPPER([Extent2].[CityName])) AS int)) > 0) OR (( CAST(CHARINDEX(UPPER(N" + searchString + "), UPPER([Extent3].[RegionName])) AS int)) > 0) ";
            
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    locations = locations.Where(s => s.LocName.ToUpper().Contains(searchString.ToUpper())
            //         || s.City.CityName.ToUpper().Contains(searchString.ToUpper())
            //         || s.City.Region.RegionName.ToUpper().Contains(searchString.ToUpper()));
            //}
            //
            switch (sortOrder)
            {
                case "name_desc":
                    locations = locations.OrderByDescending(s => s.LocName);
                    break;
                //case "site_desc":
                //    locations = locations.OrderByDescending(s => s.Site.SiteName);
                //    break;
                //case "site":
                //    locations = locations.OrderBy(s => s.Site.SiteName);
                //    break;
                //case "city_desc":
                //    locations = locations.OrderByDescending(s => s.City.CityName);
                //    break;
                //case "city":
                //    locations = locations.OrderBy(s => s.City.CityName);
                //    break;
                //case "region_desc":
                //    locations = locations.OrderByDescending(s => s.City.Region.RegionName);
                //    break;
                //case "region":
                //    locations = locations.OrderBy(s => s.City.Region.RegionName);
                //    break;
                default:
                    locations = locations.OrderBy(s => s.LocName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(locations.ToPagedList(pageNumber, pageSize));
            }
            else
                return Redirect(Request.UrlReferrer.ToString());

        }

       

        // GET: /Location/Details/5
          [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: /Location/Create
          [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          [CustomActionAttribute]
          public ActionResult Create([Bind(Include = "LocID,LocName")] Location location)
          {
              if (string.IsNullOrEmpty(location.LocName))
                  ModelState.AddModelError("LocName", "This field is required!");
              if (location.LocName != null)
              {
                  if (location.LocName.Length > 50)
                      ModelState.AddModelError("LocName", "String length exceeds!");
                  if (!myClass.IsAllLetters(location.LocName))
                  {
                      ModelState.AddModelError("LocName", "This field only contain Alphabets");
                  }
                  if (ModelState.IsValid)
                  {
                      db.Locations.Add(location);
                      db.SaveChanges();
                      return RedirectToAction("Index");
                  }
                  //ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", location.SiteID);
                  
              }
              return View(location);
          }

        // GET: /Location/Edit/5
          [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", location.SiteID);
            return View(location);
        }

        // POST: /Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
          public ActionResult Edit([Bind(Include = "LocID,LocName,CityID")] Location location)
        {
            if (string.IsNullOrEmpty(location.LocName))
                ModelState.AddModelError("LocName", "This field is required!");
            if (location.LocName != null)
            {
                if (location.LocName.Length > 50)
                    ModelState.AddModelError("LocName", "String length exceeds!");
                if (!myClass.IsAllLetters(location.LocName))
                {
                    ModelState.AddModelError("LocName", "This field only contain Alphabets");
                }
                //if (CheckDuplicate(location.LocName))
                //    ModelState.AddModelError("SectionName", "This Type already exist in record, Please select an unique name");
            }
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.SiteID = new SelectList(db.Sites, "SiteID", "SiteName", location.SiteID);
            return View(location);
        }

        // GET: /Location/Delete/5
          [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: /Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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

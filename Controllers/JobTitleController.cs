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
using WMS.Controllers.Filters;

namespace WMS.Controllers
{
     [CustomControllerAttributes]
    public class JobTitleController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /JobTitle/
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

            var jobtitle = db.JobTitles.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                jobtitle = jobtitle.Where(s => s.JobTitle1.ToUpper().Contains(searchString.ToUpper())
                     || s.JobTitle1.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    jobtitle = jobtitle.OrderByDescending(s => s.JobTitle1);
                    break;
                default:
                    jobtitle = jobtitle.OrderBy(s => s.JobTitle1);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(jobtitle.ToPagedList(pageNumber, pageSize));

        }

        // GET: /JobTitle/Details/5
           [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobtitle = db.JobTitles.Find(id);
            if (jobtitle == null)
            {
                return HttpNotFound();
            }
            return View(jobtitle);
        }

        // GET: /JobTitle/Create
           [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /JobTitle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "JobID,JobTitle1")] JobTitle jobtitle)
        {
            if (ModelState.IsValid)
            {
                db.JobTitles.Add(jobtitle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobtitle);
        }

        // GET: /JobTitle/Edit/5
           [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobtitle = db.JobTitles.Find(id);
            if (jobtitle == null)
            {
                return HttpNotFound();
            }
            return View(jobtitle);
        }

        // POST: /JobTitle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "JobID,JobTitle1")] JobTitle jobtitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobtitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobtitle);
        }

        // GET: /JobTitle/Delete/5
           [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobtitle = db.JobTitles.Find(id);
            if (jobtitle == null)
            {
                return HttpNotFound();
            }
            return View(jobtitle);
        }

        // POST: /JobTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            JobTitle jobtitle = db.JobTitles.Find(id);
            db.JobTitles.Remove(jobtitle);
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

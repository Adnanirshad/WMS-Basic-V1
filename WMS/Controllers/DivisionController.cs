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
using System.Linq.Dynamic;
using WMS.CustomClass;

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class DivisionController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /Division/
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
            string query = qb.QueryForCompanyViewForLinq(LoggedInUser);
            var divisions = db.Divisions.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                divisions = divisions.Where(s => s.DivisionName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    divisions = divisions.OrderByDescending(s => s.DivisionName);
                    break;
                default:
                    divisions = divisions.OrderBy(s => s.DivisionName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(divisions.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Division/Details/5
        [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.Divisions.Find(id);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // GET: /Division/Create
        [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Division/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DivisionID,DivisionName")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.Divisions.Add(division);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(division);
        }

        // GET: /Division/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.Divisions.Find(id);
            
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // POST: /Division/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "DivisionID,DivisionName")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.Entry(division).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(division);
        }

        // GET: /Division/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.Divisions.Find(id);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // POST: /Division/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            Division division = db.Divisions.Find(id);
            db.Divisions.Remove(division);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DivisionList()
        {

            var divisions = db.Divisions.ToList();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                divisions.ToArray(),
                                "DivisionID",
                                "DivisionName")
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

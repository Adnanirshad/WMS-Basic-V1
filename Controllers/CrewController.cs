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
using WMS.Controllers.Filters;
using WMS.CustomClass;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class CrewController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /Crew/
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
            var crew = db.Crews.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                crew = crew.Where(s => s.CrewName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    crew = crew.OrderByDescending(s => s.CrewName);
                    break;
                default:
                    crew = crew.OrderBy(s => s.CrewName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(crew.ToPagedList(pageNumber, pageSize));

        }

        // GET: /Crew/Details/5
        [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crew crew = db.Crews.Find(id);
            if (crew == null)
            {
                return HttpNotFound();
            }
            return View(crew);
        }

        // GET: /Crew/Create
        [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Crew/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "CrewID,CrewName")] Crew crew)
        {
            if (crew.CrewName == "")
                ModelState.AddModelError("CrewName", "Please enter Crew Name");
            if (ModelState.IsValid)
            {
                db.Crews.Add(crew);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crew);
        }

        // GET: /Crew/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crew crew = db.Crews.Find(id);
            if (crew == null)
            {
                return HttpNotFound();
            }
            return View(crew);
        }

        // POST: /Crew/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "CrewID,CrewName")] Crew crew)
        {
            if (crew.CrewName == "")
                ModelState.AddModelError("CrewName", "Please enter Crew Name");
            if (ModelState.IsValid)
            {
                db.Entry(crew).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crew);
        }

        // GET: /Crew/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crew crew = db.Crews.Find(id);
            
            if (crew == null)
            {
                return HttpNotFound();
            }
                return View(crew);

        }

        // POST: /Crew/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            
            Crew crew = db.Crews.Find(id);
            int rostcount = db.RosterApps.Where(aa => aa.RosterCriteria == "C" && aa.CriteriaData == crew.CrewID).Count();
            if (rostcount == 0)
            {
                db.Crews.Remove(crew);

                db.SaveChanges();
            
            }
           

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

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
    public class GradeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /Grade/
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
            var grade = db.Grades.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                grade = grade.Where(s => s.GradeName == searchString);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    grade = grade.OrderByDescending(s => s.GradeName);
                    break;
                default:
                    grade = grade.OrderBy(s => s.GradeName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(grade.ToPagedList(pageNumber, pageSize));

        }

        // GET: /Grade/Details/5
          [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // GET: /Grade/Create
          [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Grade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
          public ActionResult Create([Bind(Include = "GradeID,GradeName")] Grade grade)
        {
           
            if(db.Grades.Where(aa=>aa.GradeName==grade.GradeName ).Count()>0)
                ModelState.AddModelError("GradeName", "Grade Name must be unique");
            if (ModelState.IsValid)
            {
                db.Grades.Add(grade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(grade);
        }

        // GET: /Grade/Edit/5
          [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // POST: /Grade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
          public ActionResult Edit([Bind(Include = "GradeID,GradeName")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grade);
        }

        // GET: /Grade/Delete/5
          [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // POST: /Grade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
          public ActionResult DeleteConfirmed(short id)
        {
            Grade grade = db.Grades.Find(id);
            db.Grades.Remove(grade);
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

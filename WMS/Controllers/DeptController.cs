using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using System.Linq.Dynamic;
using PagedList;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.CustomClass;

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class DeptController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /Dept/
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
            var departments = db.Departments.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(s => s.DeptName.ToUpper().Contains(searchString.ToUpper())
                    //||s.Division.DivisionName.ToUpper().Contains(searchString.ToUpper())
                    );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    departments = departments.OrderByDescending(s => s.DeptName);
                    break;
                default:
                    departments = departments.OrderBy(s => s.DeptName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(departments.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Dept/Details/5
          [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: /Dept/Create
           [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Dept/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
           public ActionResult Create([Bind(Include = "DeptID,DeptName,DivID")] Department department)
        {
            if (department.DeptName == "")
                ModelState.AddModelError("DeptName", "Please enter Department Name");
           
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Department, (byte)MyEnums.Operation.Add, DateTime.Now);
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: /Dept/Edit/5
           [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            User LoggedInUser = Session["LoggedUser"] as User;
            QueryBuilder qb = new QueryBuilder();
            string query = qb.QueryForCompanyViewForLinq(LoggedInUser);
            string query1 = qb.QueryForCompanyViewLinq(LoggedInUser);
         
            return View(department);
        }

        // POST: /Dept/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
           public ActionResult Edit([Bind(Include = "DeptID,DeptName,DivID")] Department department)
        {
            if (department.DeptName == "")
                ModelState.AddModelError("DeptName", "Please enter Department Name");
            
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Department, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
                        return View(department);
        }

        // GET: /Dept/Delete/5
           [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: /Dept/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            Department department = db.Departments.Find(id);
            List<Section> sections = db.Sections.Where(aa => aa.DeptID == department.DeptID).ToList();
            foreach (var section in sections)
            {
                db.Sections.Remove(section);
                db.SaveChanges();
            
            }
            db.Departments.Remove(department);
            db.SaveChanges();
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Department, (byte)MyEnums.Operation.Delete, DateTime.Now);
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

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
using WMS.HelperClass;
using WMS.Controllers.Filters;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class DesignationController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
        // GET: /Designation/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (Session["LogedUserFullname"].ToString() != "")
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
            QueryBuilder qb = new QueryBuilder();

            var designation = db.Designations.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                designation = designation.Where(s => s.DesignationName.ToUpper().Contains(searchString.ToUpper())
                    );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    designation = designation.OrderByDescending(s => s.DesignationName);
                    break;
                default:
                    designation = designation.OrderBy(s => s.DesignationName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(designation.ToPagedList(pageNumber, pageSize));
            }
             else
                 return Redirect(Request.UrlReferrer.ToString());

        }

        // GET: /Designation/Details/5
         [CustomActionAttribute]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return HttpNotFound();
            }
            return View(designation);
        }

        // GET: /Designation/Create
         [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Designation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "DesignationID,DesignationName")] Designation designation)
        {
            if (string.IsNullOrEmpty(designation.DesignationName))
                ModelState.AddModelError("DesignationName", "This field is required!");
            if (designation.DesignationName != null)
            {
                if (designation.DesignationName.Length > 100)
                    ModelState.AddModelError("DesignationName", "String length exceeds!");
                if (!myClass.IsAllLetters(designation.DesignationName))
                {
                    ModelState.AddModelError("DesignationName", "This field only contain Alphabets");
                }
                // on test basis
            }
            if (ModelState.IsValid)
            {
                db.Designations.Add(designation);
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Designation, (byte)MyEnums.Operation.Add, DateTime.Now);
                return RedirectToAction("Index");
            }
            return View(designation);
        }
        // Check Duplicate
        private bool CheckDuplicate(string _Name)
        {
            var _desig = db.Designations;
            foreach (var item in _desig)
            {
                if (item.DesignationName.ToUpper() == _Name.ToUpper())
                    return true;
            }
            return false;
        }

        // GET: /Designation/Edit/5
         [CustomActionAttribute]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return HttpNotFound();
            }
            return View(designation);
        }

        // POST: /Designation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "DesignationID,DesignationName")] Designation designation)
        {
            if (string.IsNullOrEmpty(designation.DesignationName))
                ModelState.AddModelError("DesignationName", "This field is required!");
            if (designation.DesignationName != null)
            {
                if (designation.DesignationName.Length > 100)
                    ModelState.AddModelError("DesignationName", "String length exceeds!");
                if (!myClass.IsAllLetters(designation.DesignationName))
                {
                    ModelState.AddModelError("DesignationName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(designation).State = EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Designation, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
            return View(designation);
        }

        // GET: /Designation/Delete/5
         [CustomActionAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation designation = db.Designations.Find(id);
            if (designation == null)
            {
                return HttpNotFound();
            }
            return View(designation);
        }

        // POST: /Designation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            Designation designation = db.Designations.Find(id);
            db.Designations.Remove(designation);
            db.SaveChanges();
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Designation, (byte)MyEnums.Operation.Delete, DateTime.Now);
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

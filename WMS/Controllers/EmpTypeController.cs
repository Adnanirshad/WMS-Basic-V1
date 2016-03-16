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
namespace WMS.Controllers
{
     [CustomControllerAttributes]
    public class EmpTypeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
        // GET: /EmpType/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CatSortParm = sortOrder == "cat" ? "cat_desc" : "cat";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var emptype = db.EmpTypes.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                emptype = emptype.Where(s => s.TypeName.ToUpper().Contains(searchString.ToUpper())
                    //|| s.Category.CatName.ToUpper().Contains(searchString.ToUpper())
                    );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    emptype = emptype.OrderByDescending(s => s.TypeName);
                    break;
                
                default:
                    emptype = emptype.OrderBy(s => s.TypeName);
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(emptype.ToPagedList(pageNumber, pageSize));

        }

        // GET: /EmpType/Details/5
        [CustomActionAttribute]
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpType emptype = db.EmpTypes.Find(id);
            if (emptype == null)
            {
                return HttpNotFound();
            }
            return View(emptype);
        }

        // GET: /EmpType/Create
        [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /EmpType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,CatID")] EmpType emptype)
        {
            //if (emptype.CatID == null)
            //    ModelState.AddModelError("CatID", "Please select Category");
            if (string.IsNullOrEmpty(emptype.TypeName))
                ModelState.AddModelError("TypeName", "This field is required!");
            if (emptype.TypeName != null)
            {
                if (emptype.TypeName.Length > 50)
                    ModelState.AddModelError("TypeName", "String length exceeds!");
                if (!myClass.IsAllLetters(emptype.TypeName))
                {
                    ModelState.AddModelError("TypeName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.EmpTypes.Add(emptype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
              return View(emptype);
        }
        private bool CheckDuplicate(string _Name)
        {
            var _empType = db.EmpTypes;
            foreach (var item in _empType)
            {
                if (item.TypeName.ToUpper() == _Name.ToUpper())
                    return true;
            }
            return false;
        }
        // GET: /EmpType/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpType emptype = db.EmpTypes.Find(id);
            if (emptype == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CatID = new SelectList(db.Categories.OrderBy(s => s.CatName), "CatID", "CatName", emptype.CatID);
            return View(emptype);
        }

        // POST: /EmpType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,CatID")] EmpType emptype)
        {
            if (string.IsNullOrEmpty(emptype.TypeName))
                ModelState.AddModelError("TypeName", "This field is required!");
            if (emptype.TypeName != null)
            {
                if (emptype.TypeName.Length > 50)
                    ModelState.AddModelError("TypeName", "String length exceeds!");
                if (!myClass.IsAllLetters(emptype.TypeName))
                {
                    ModelState.AddModelError("TypeName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(emptype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CatID = new SelectList(db.Categories.OrderBy(s=>s.CatName), "CatID", "CatName", emptype.CatID);
            return View(emptype);
        }

        // GET: /EmpType/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpType emptype = db.EmpTypes.Find(id);
            if (emptype == null)
            {
                return HttpNotFound();
            }
            return View(emptype);
        }

        // POST: /EmpType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(byte id)
        {
            EmpType emptype = db.EmpTypes.Find(id);
            db.EmpTypes.Remove(emptype);
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

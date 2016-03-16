using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using WMS.CustomClass;
using WMS.Controllers.Filters;
using PagedList;
namespace WMS.Controllers
{
     [CustomControllerAttributes]
    public class ReaderTypeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        CustomFunc myClass = new CustomFunc();
        // GET: /ReaderType/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LocationSortParm = sortOrder == "vendor" ? "vendor_desc" : "vendor";
            if (searchString != null)
            {
                //page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var readertypes = db.ReaderTypes.ToList();
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    readertypes = readertypes.Where(s => s.RdrTypeName.ToUpper().Contains(searchString.ToUpper()));
            //}

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        readertypes = readertypes.OrderByDescending(s => s.RdrTypeName);
            //        break;
            //    case "vendor_desc":
            //        readertypes = readertypes.OrderByDescending(s => s.VendorID);
            //        break;
            //    case "vendor":
            //        readertypes = readertypes.OrderBy(s => s.RdrTypeName);
            //        break;
            //    default:
            //        readertypes = readertypes.OrderBy(s => s.RdrTypeName);
            //        break;
            //}
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(readertypes.ToPagedList(pageNumber, pageSize));
            //return View(readertypes.ToList());
        }

        // GET: /ReaderType/Details/5
         [CustomActionAttribute]
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReaderType readertype = db.ReaderTypes.Find(id);
            if (readertype == null)
            {
                return HttpNotFound();
            }
            return View(readertype);
        }

        // GET: /ReaderType/Create
         [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ReaderType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "RdrTypeID,RdrTypeName,VendorID")] ReaderType readertype)
        {
            if (string.IsNullOrEmpty(readertype.RdrTypeName))
                ModelState.AddModelError("RdrTypeName", "Reader Type field is required!");
            if (readertype.RdrTypeName != null)
            {
                if (readertype.RdrTypeName.Length > 50)
                    ModelState.AddModelError("RdrTypeName", "String length exceeds!");
                if (!myClass.IsAllLetters(readertype.RdrTypeName))
                {
                    ModelState.AddModelError("RdrTypeName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.ReaderTypes.Add(readertype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

             return View(readertype);
        }

        // GET: /ReaderType/Edit/5
         [CustomActionAttribute]
        public ActionResult Edit(byte? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReaderType readertype = db.ReaderTypes.Find(id);
            if (readertype == null)
            {
                return HttpNotFound();
            }
             return View(readertype);
        }

        // POST: /ReaderType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "RdrTypeID,RdrTypeName,VendorID")] ReaderType readertype)
        {
            if (string.IsNullOrEmpty(readertype.RdrTypeName))
                ModelState.AddModelError("RdrTypeName", "Reader Type field is required!");
            if (readertype.RdrTypeName != null)
            {
                if (readertype.RdrTypeName.Length > 50)
                    ModelState.AddModelError("RdrTypeName", "String length exceeds!");
                if (!myClass.IsAllLetters(readertype.RdrTypeName))
                {
                    ModelState.AddModelError("RdrTypeName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(readertype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
             return View(readertype);
        }

        // GET: /ReaderType/Delete/5
         [CustomActionAttribute]
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReaderType readertype = db.ReaderTypes.Find(id);
            if (readertype == null)
            {
                return HttpNotFound();
            }
            return View(readertype);
        }

        // POST: /ReaderType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(byte id)
        {
            ReaderType readertype = db.ReaderTypes.Find(id);
            db.ReaderTypes.Remove(readertype);
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

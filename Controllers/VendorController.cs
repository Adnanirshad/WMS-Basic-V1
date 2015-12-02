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
    public class VendorController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /Vendor/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                //page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var vendor = db.ReaderVendors.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                vendor = vendor.Where(s => s.VendorName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vendor = vendor.OrderByDescending(s => s.VendorName);
                    break;
                default:
                    vendor = vendor.OrderBy(s => s.VendorName);
                    break;
            }
            //int pageSize = 8;
            //int pageNumber = (page ?? 1);
            //return View(vendor.ToPagedList(pageNumber, pageSize));
            return View(vendor.ToList());

        }

        // GET: /Vendor/Details/5
        [CustomActionAttribute]
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReaderVendor readervendor = db.ReaderVendors.Find(id);
            if (readervendor == null)
            {
                return HttpNotFound();
            }
            return View(readervendor);
        }

        // GET: /Vendor/Create
        [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        CustomFunc myClass = new CustomFunc(); 
        // POST: /Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include="VendorID,VendorName")] ReaderVendor readervendor)
        {
            if (string.IsNullOrEmpty(readervendor.VendorName))
                ModelState.AddModelError("VendorName", "Reader Vendor field is required!");
            if (readervendor.VendorName != null)
            {
                if (readervendor.VendorName.Length > 50)
                    ModelState.AddModelError("VendorName", "String length exceeds!");
                if (!myClass.IsAllLetters(readervendor.VendorName))
                {
                    ModelState.AddModelError("VendorName", "This field only contain Alphabets");
                }
            }
           
            if (ModelState.IsValid)
            {
                db.ReaderVendors.Add(readervendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(readervendor);
        }

        // GET: /Vendor/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReaderVendor readervendor = db.ReaderVendors.Find(id);
            if (readervendor == null)
            {
                return HttpNotFound();
            }
            return View(readervendor);
        }

        // POST: /Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include="VendorID,VendorName")] ReaderVendor readervendor)
        {
            if (string.IsNullOrEmpty(readervendor.VendorName))
                ModelState.AddModelError("VendorName", "Reader Vendor field is required!");
            if (readervendor.VendorName != null)
            {
                if (readervendor.VendorName.Length > 50)
                    ModelState.AddModelError("VendorName", "String length exceeds!");
                if (!myClass.IsAllLetters(readervendor.VendorName))
                {
                    ModelState.AddModelError("VendorName", "This field only contain Alphabets");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(readervendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(readervendor);
        }

        // GET: /Vendor/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReaderVendor readervendor = db.ReaderVendors.Find(id);
            if (readervendor == null)
            {
                return HttpNotFound();
            }
            return View(readervendor);
        }

        // POST: /Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(byte id)
        {
            ReaderVendor readervendor = db.ReaderVendors.Find(id);
            db.ReaderVendors.Remove(readervendor);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class LvConsumedController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LvConsumed/
        public ActionResult Index()
        {
            var lvconsumeds = db.LvConsumeds.Include(l => l.Emp);
            return View(lvconsumeds.ToList());
        }

        // GET: /LvConsumed/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvConsumed lvconsumed = db.LvConsumeds.Find(id);
            if (lvconsumed == null)
            {
                return HttpNotFound();
            }
            return View(lvconsumed);
        }

        // GET: /LvConsumed/Create
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo");
            return View();
        }

        // POST: /LvConsumed/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EmpLvType,EmpID,LeaveType,CompanyID,JanConsumed,FebConsumed,MarchConsumed,AprConsumed,MayConsumed,JuneConsumed,JulyConsumed,AugustConsumed,SepConsumed,OctConsumed,NovConsumed,DecConsumed,TotalForYear,YearRemaining,GrandTotal,GrandTotalRemaining")] LvConsumed lvconsumed)
        {
            if (ModelState.IsValid)
            {
                db.LvConsumeds.Add(lvconsumed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvconsumed.EmpID);
            return View(lvconsumed);
        }

        // GET: /LvConsumed/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvConsumed lvconsumed = db.LvConsumeds.Find(id);
            if (lvconsumed == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvconsumed.EmpID);
            return View(lvconsumed);
        }

        // POST: /LvConsumed/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EmpLvType,EmpID,LeaveType,CompanyID,JanConsumed,FebConsumed,MarchConsumed,AprConsumed,MayConsumed,JuneConsumed,JulyConsumed,AugustConsumed,SepConsumed,OctConsumed,NovConsumed,DecConsumed,TotalForYear,YearRemaining,GrandTotal,GrandTotalRemaining")] LvConsumed lvconsumed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lvconsumed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvconsumed.EmpID);
            return View(lvconsumed);
        }

        // GET: /LvConsumed/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvConsumed lvconsumed = db.LvConsumeds.Find(id);
            if (lvconsumed == null)
            {
                return HttpNotFound();
            }
            return View(lvconsumed);
        }

        // POST: /LvConsumed/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LvConsumed lvconsumed = db.LvConsumeds.Find(id);
            db.LvConsumeds.Remove(lvconsumed);
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

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
    public class RosterAppController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /RosterApp/
        public ActionResult Index()
        {
            var rosterapps = db.RosterApps.Include(r => r.RosterType).Include(r => r.Shift);
            return View(rosterapps.ToList());
        }

        // GET: /RosterApp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterApp rosterapp = db.RosterApps.Find(id);
            if (rosterapp == null)
            {
                return HttpNotFound();
            }
            return View(rosterapp);
        }

        // GET: /RosterApp/Create
        public ActionResult Create()
        {
            ViewBag.RotaTypeID = new SelectList(db.RosterTypes, "ID", "Name");
            ViewBag.ShiftID = new SelectList(db.Shifts, "ShiftID", "ShiftName");
            return View();
        }

        // POST: /RosterApp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RotaApplD,DateCreated,DateStarted,DateEnded,UserID,RosterCriteria,CriteriaData,Status,RotaTypeID,WorkMin,DutyTime,ShiftID")] RosterApp rosterapp)
        {
            if (ModelState.IsValid)
            {
                db.RosterApps.Add(rosterapp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RotaTypeID = new SelectList(db.RosterTypes, "ID", "Name", rosterapp.RotaTypeID);
            ViewBag.ShiftID = new SelectList(db.Shifts, "ShiftID", "ShiftName", rosterapp.ShiftID);
            return View(rosterapp);
        }

        // GET: /RosterApp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterApp rosterapp = db.RosterApps.Find(id);
            if (rosterapp == null)
            {
                return HttpNotFound();
            }
            ViewBag.RotaTypeID = new SelectList(db.RosterTypes, "ID", "Name", rosterapp.RotaTypeID);
            ViewBag.ShiftID = new SelectList(db.Shifts, "ShiftID", "ShiftName", rosterapp.ShiftID);
            return View(rosterapp);
        }

        // POST: /RosterApp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RotaApplD,DateCreated,DateStarted,DateEnded,UserID,RosterCriteria,CriteriaData,Status,RotaTypeID,WorkMin,DutyTime,ShiftID")] RosterApp rosterapp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rosterapp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RotaTypeID = new SelectList(db.RosterTypes, "ID", "Name", rosterapp.RotaTypeID);
            ViewBag.ShiftID = new SelectList(db.Shifts, "ShiftID", "ShiftName", rosterapp.ShiftID);
            return View(rosterapp);
        }

        // GET: /RosterApp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterApp rosterapp = db.RosterApps.Find(id);
            if (rosterapp == null)
            {
                return HttpNotFound();
            }
            return View(rosterapp);
        }

        // POST: /RosterApp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RosterApp rosterapp = db.RosterApps.Find(id);
            db.RosterApps.Remove(rosterapp);
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

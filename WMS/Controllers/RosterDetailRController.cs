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
    public class RosterDetailRController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /RosterDetailR/
        public ActionResult Index()
        {
            var rosterdetails = db.RosterDetails.Include(r => r.RosterApp);
            return View(rosterdetails.ToList());
        }

        // GET: /RosterDetailR/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterDetail rosterdetail = db.RosterDetails.Find(id);
            if (rosterdetail == null)
            {
                return HttpNotFound();
            }
            return View(rosterdetail);
        }

        // GET: /RosterDetailR/Create
        public ActionResult Create()
        {
            ViewBag.RosterAppID = new SelectList(db.RosterApps, "RotaApplD", "RosterCriteria");
            return View();
        }

        // POST: /RosterDetailR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CriteriaValueDate,RosterAppID,DutyCode,DutyTime,WorkMin,Remarks,CompanyID,OpenShift,RosterDate,UserID")] RosterDetail rosterdetail)
        {
            if (ModelState.IsValid)
            {
                db.RosterDetails.Add(rosterdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RosterAppID = new SelectList(db.RosterApps, "RotaApplD", "RosterCriteria", rosterdetail.RosterAppID);
            return View(rosterdetail);
        }

        // GET: /RosterDetailR/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterDetail rosterdetail = db.RosterDetails.Find(id);
            if (rosterdetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.RosterAppID = new SelectList(db.RosterApps, "RotaApplD", "RosterCriteria", rosterdetail.RosterAppID);
            return View(rosterdetail);
        }

        // POST: /RosterDetailR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CriteriaValueDate,RosterAppID,DutyCode,DutyTime,WorkMin,Remarks,CompanyID,OpenShift,RosterDate,UserID")] RosterDetail rosterdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rosterdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RosterAppID = new SelectList(db.RosterApps, "RotaApplD", "RosterCriteria", rosterdetail.RosterAppID);
            return View(rosterdetail);
        }

        // GET: /RosterDetailR/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterDetail rosterdetail = db.RosterDetails.Find(id);
            if (rosterdetail == null)
            {
                return HttpNotFound();
            }
            return View(rosterdetail);
        }

        // POST: /RosterDetailR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RosterDetail rosterdetail = db.RosterDetails.Find(id);
            db.RosterDetails.Remove(rosterdetail);
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

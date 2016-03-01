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
    public class JobCardDateController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /JobCardDate/
        public ActionResult Index()
        {
            return View(db.JobCardApps.ToList());
        }

        // GET: /JobCardDate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardapp);
        }

        // GET: /JobCardDate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /JobCardDate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="JobCardID,DateCreated,DateStarted,DateEnded,CardType,UserID,JobCardCriteria,CriteriaData,Status,TimeIn,TimeOut,WorkMin,Remarks,OtherValue")] JobCardApp jobcardapp)
        {
            if (ModelState.IsValid)
            {
                db.JobCardApps.Add(jobcardapp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobcardapp);
        }

        // GET: /JobCardDate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardapp);
        }

        // POST: /JobCardDate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="JobCardID,DateCreated,DateStarted,DateEnded,CardType,UserID,JobCardCriteria,CriteriaData,Status,TimeIn,TimeOut,WorkMin,Remarks,OtherValue")] JobCardApp jobcardapp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobcardapp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobcardapp);
        }

        // GET: /JobCardDate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            if (jobcardapp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardapp);
        }

        // POST: /JobCardDate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobCardApp jobcardapp = db.JobCardApps.Find(id);
            db.JobCardApps.Remove(jobcardapp);
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

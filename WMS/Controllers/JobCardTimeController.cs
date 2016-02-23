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
    public class JobCardTimeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /JobCardTime/
        public ActionResult Index()
        {
            return View(db.JobCardTimes.ToList());
        }

        // GET: /JobCardTime/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            if (jobcardtime == null)
            {
                return HttpNotFound();
            }
            return View(jobcardtime);
        }

        // GET: /JobCardTime/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /JobCardTime/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,EmpID,StartTime,EndTime,TTime,DutyDate,Remarks,AssignedBy,CreatedBy,JobCardID,CreatedDate")] JobCardTime jobcardtime)
        {
            if (ModelState.IsValid)
            {
                db.JobCardTimes.Add(jobcardtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobcardtime);
        }

        // GET: /JobCardTime/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            if (jobcardtime == null)
            {
                return HttpNotFound();
            }
            return View(jobcardtime);
        }

        // POST: /JobCardTime/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,EmpID,StartTime,EndTime,TTime,DutyDate,Remarks,AssignedBy,CreatedBy,JobCardID,CreatedDate")] JobCardTime jobcardtime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobcardtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobcardtime);
        }

        // GET: /JobCardTime/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            if (jobcardtime == null)
            {
                return HttpNotFound();
            }
            return View(jobcardtime);
        }

        // POST: /JobCardTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobCardTime jobcardtime = db.JobCardTimes.Find(id);
            db.JobCardTimes.Remove(jobcardtime);
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

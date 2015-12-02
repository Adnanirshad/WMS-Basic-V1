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
    public class JobCardEmpRController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /JobCardEmpR/
        public ActionResult Index()
        {
            return View(db.JobCardEmps.OrderBy(s=>s.DateCreated).ToList());
        }

        // GET: /JobCardEmpR/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardEmp jobcardemp = db.JobCardEmps.Find(id);
            if (jobcardemp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardemp);
        }

        // GET: /JobCardEmpR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /JobCardEmpR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EmpDate,JobDataID,EmpID,Dated,SubmittedFrom,WrkCardID,SendTo,Approved,Rejected,DateCreated,Remarks,TimeIn,TimeOut,WorkMin")] JobCardEmp jobcardemp)
        {
            if (ModelState.IsValid)
            {
                db.JobCardEmps.Add(jobcardemp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobcardemp);
        }

        // GET: /JobCardEmpR/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardEmp jobcardemp = db.JobCardEmps.Find(id);
            if (jobcardemp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardemp);
        }

        // POST: /JobCardEmpR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EmpDate,JobDataID,EmpID,Dated,SubmittedFrom,WrkCardID,SendTo,Approved,Rejected,DateCreated,Remarks,TimeIn,TimeOut,WorkMin")] JobCardEmp jobcardemp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobcardemp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobcardemp);
        }

        // GET: /JobCardEmpR/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCardEmp jobcardemp = db.JobCardEmps.Find(id);
            if (jobcardemp == null)
            {
                return HttpNotFound();
            }
            return View(jobcardemp);
        }

        // POST: /JobCardEmpR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            JobCardEmp jobcardemp = db.JobCardEmps.Find(id);
            db.JobCardEmps.Remove(jobcardemp);
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

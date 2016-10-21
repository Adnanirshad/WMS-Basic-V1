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
    public class LeaveTypeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LeaveType/
        public ActionResult Index()
        {
            return View(db.LvTypes.ToList());
        }

        // GET: /LeaveType/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvType lvtype = db.LvTypes.Find(id);
            if (lvtype == null)
            {
                return HttpNotFound();
            }
            return View(lvtype);
        }

        // GET: /LeaveType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /LeaveType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="LvTypeID,LvDesc,FldName,HalfLeave,HalfLvCode,HalfAbCode,MaxDays,UpdateBalance,Enable,CarryForward,CarryForwardYear,CarryForwardDays,CountGZDays,CountRestDays,MaxDaysConsective,MaxDaysMonth")] LvType lvtype)
        {
            if (ModelState.IsValid)
            {
                db.LvTypes.Add(lvtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lvtype);
        }

        // GET: /LeaveType/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvType lvtype = db.LvTypes.Find(id);
            if (lvtype == null)
            {
                return HttpNotFound();
            }
            return View(lvtype);
        }

        // POST: /LeaveType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="LvTypeID,LvDesc,FldName,HalfLeave,HalfLvCode,HalfAbCode,MaxDays,UpdateBalance,Enable,CarryForward,CarryForwardYear,CarryForwardDays,CountGZDays,CountRestDays,MaxDaysConsective,MaxDaysMonth")] LvType lvtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lvtype).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lvtype);
        }

        // GET: /LeaveType/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvType lvtype = db.LvTypes.Find(id);
            if (lvtype == null)
            {
                return HttpNotFound();
            }
            return View(lvtype);
        }

        // POST: /LeaveType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LvType lvtype = db.LvTypes.Find(id);
            db.LvTypes.Remove(lvtype);
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

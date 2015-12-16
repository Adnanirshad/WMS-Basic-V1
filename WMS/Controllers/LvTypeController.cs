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
    public class LvTypeController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LvType/
        public ActionResult Index()
        {
            return View(db.LvTypes.ToList());
        }

        // GET: /LvType/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LvType lvtype = db.LvTypes.Find(id);
        //    if (lvtype == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(lvtype);
        //}

        //// GET: /LvType/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: /LvType/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="LvType1,LvDesc,FldName,HalfLeave,HalfLvCode,HalfAbCode,MaxDays,WorkDays,VacDays,MonthProcess,UpdateBalance,LvStatus,Enable,CompanyID")] LvType lvtype)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.LvTypes.Add(lvtype);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(lvtype);
        //}

        // GET: /LvType/Edit/5
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

        // POST: /LvType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="LvType1,LvDesc,FldName,HalfLeave,HalfLvCode,HalfAbCode,MaxDays,UpdateBalance,Enable,CarryForward,CountVacDays")] LvType lvtype)
        {
           
            if (ModelState.IsValid)
            {
                string fldName = "X";
                string halfLvCode = "HA";
                string halfAbCode = "Y";
                lvtype.FldName = fldName;
                lvtype.HalfLvCode = halfLvCode;
                lvtype.HalfAbCode = halfAbCode;
                db.Entry(lvtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lvtype);
        }

        // GET: /LvType/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LvType lvtype = db.LvTypes.Find(id);
        //    if (lvtype == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(lvtype);
        //}

        //// POST: /LvType/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    LvType lvtype = db.LvTypes.Find(id);
        //    db.LvTypes.Remove(lvtype);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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

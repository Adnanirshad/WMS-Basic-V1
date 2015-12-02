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
using WMS.HelperClass;
namespace WMS.Controllers
{
     [CustomControllerAttributes]
    public class LvShortController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LvShort/
        public ActionResult Index()
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            var lvshorts = db.LvShorts.OrderBy(s=>s.CreatedDate).ToList();
            return View(lvshorts);
        }

        // GET: /LvShort/Details/5
             [CustomActionAttribute]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvShort lvshort = db.LvShorts.Find(id);
            if (lvshort == null)
            {
                return HttpNotFound();
            }
            return View(lvshort);
        }

        // GET: /LvShort/Create
             [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /LvShort/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "SlID,EmpID,DutyDate,EmpDate,SHour,EHour,THour,Remarks,CreatedBy,ApprovedBy,Status")] LvShort lvshort,string SHour,string EHour)
        {
            string STimeIn = SHour;
            string STimeOut = EHour;
            string STimeInH = STimeIn.Substring(0, 2);
            string STimeInM = STimeIn.Substring(2, 2);
            string STimeOutH = STimeOut.Substring(0, 2);
            string STimeOutM = STimeOut.Substring(2, 2);
            lvshort.SHour = new TimeSpan(Convert.ToInt16(STimeInH), Convert.ToInt16(STimeInM), 0);
            lvshort.EHour = new TimeSpan(Convert.ToInt16(STimeOutH), Convert.ToInt16(STimeOutM), 0);
            if (lvshort.EHour < lvshort.SHour)
            {
                ModelState.AddModelError("EHour", "End hour required");
            }
            if (Request.Form["EmpNo"].ToString() == "")
            {
                ModelState.AddModelError("EmpNo", "Emplyee No is required!");
            }
            else
            {
                string _EmpNo = Request.Form["EmpNo"].ToString();
                List<Emp> _emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo).OrderBy(s=>s.EmpNo).ToList();
                if (_emp.Count == 0)
                {
                    ModelState.AddModelError("EmpNo", "Emp No not exist");
                }
                else
                {
                    lvshort.EmpID = _emp.FirstOrDefault().EmpID;
                    lvshort.EmpDate = _emp.FirstOrDefault().EmpID.ToString() + lvshort.DutyDate.Value.ToString("yyMMdd");
                    lvshort.CreatedDate = DateTime.Today;
                    lvshort.THour = lvshort.EHour - lvshort.SHour;
                }
            }
            if (lvshort.DutyDate == null)
            {
                ModelState.AddModelError("DutyDate", "DutyDate is required!");
            }
            if (lvshort.SHour == null)
            {
                ModelState.AddModelError("SHour", "Start Time is required!");
            }
            if (lvshort.EHour == null)
            {
                ModelState.AddModelError("EHour", "Ending Time is required!");
            }
            if (ModelState.IsValid)
            {
                LeaveController LvProcessController = new LeaveController();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                User LoggedInUser = Session["LoggedUser"] as User;
                lvshort.CreatedBy = _userID;
                lvshort.Status = true;
                db.LvShorts.Add(lvshort);
                db.SaveChanges();
                LvProcessController.AddShortLeaveToAttData(lvshort);
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.ShortLeave, (byte)MyEnums.Operation.Add, DateTime.Now);

                return RedirectToAction("Index");
            }

            return View(lvshort);
        }

        // GET: /LvShort/Edit/5
             [CustomActionAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvShort lvshort = db.LvShorts.Find(id);
            if (lvshort == null)
            {
                return HttpNotFound();
            }
            return View(lvshort);
        }

        // POST: /LvShort/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "SlID,EmpID,DutyDate,EmpDate,SHour,EHour,THour,Remarks,CreatedBy,ApprovedBy,Status")] LvShort lvshort)
        {
            if (lvshort.EHour < lvshort.SHour)
            {
                ModelState.AddModelError("EHour", "End hour required");
            }
            if (lvshort.DutyDate == null)
            {
                ModelState.AddModelError("DutyDate", "DutyDate is required!");
            }
            if (ModelState.IsValid)
            {
                lvshort.CreatedDate = DateTime.Today;
                lvshort.THour = lvshort.EHour - lvshort.SHour;
                db.Entry(lvshort).State = EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.ShortLeave, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
            return View(lvshort);
        }

        // GET: /LvShort/Delete/5
             [CustomActionAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvShort lvshort = db.LvShorts.Find(id);
            if (lvshort == null)
            {
                return HttpNotFound();
            }
            return View(lvshort);
        }

        // POST: /LvShort/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            LvShort lvshort = db.LvShorts.Find(id);
            db.LvShorts.Remove(lvshort);
            db.SaveChanges();
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.ShortLeave, (byte)MyEnums.Operation.Delete, DateTime.Now);
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

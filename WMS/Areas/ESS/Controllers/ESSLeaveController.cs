using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Controllers;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.Models;

namespace WMS.Areas.ESS.Controllers
{
    public class ESSLeaveController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /ESS/ESSLeave/
        public ActionResult Index()
        {
            var lvapplications = db.LvApplications.Include(l => l.Emp).Include(l => l.User).Include(l => l.LvType1);
            return View(lvapplications.ToList());
        }

        // GET: /ESS/ESSLeave/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvApplication lvapplication = db.LvApplications.Find(id);
            if (lvapplication == null)
            {
                return HttpNotFound();
            }
            return View(lvapplication);
        }

        // GET: /ESS/ESSLeave/Create
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo");
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "UserName");
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc");
            //Check Leave Quota if not created disable all buttons and show message
            User LoggedInUser = Session["LoggedUser"] as User;
            Emp employee = db.Emps.Where(aa => aa.EmpID == LoggedInUser.EmpID).FirstOrDefault();
            int lvconsumed = db.LvConsumeds.Where(aa => aa.EmpID == employee.EmpID).Count();
            if (lvconsumed > 0)
                ViewBag.QuotaMade = true;
            else
                ViewBag.QuotaMade = false;
            return View();
        }
        public decimal CheckLeaveBalance(string LeaveType)
        {
            decimal RemainingLeaves=-1;

            
                using (var ctx = new TAS2013Entities())
                {
                      User LoggedInUser = Session["LoggedUser"] as User;
                      Emp employee = db.Emps.Where(aa => aa.EmpID == LoggedInUser.EmpID).FirstOrDefault();
                    List<LvConsumed> _lvConsumed = new List<LvConsumed>();
                    string empLvType = employee.EmpID.ToString() + LeaveType;
                    _lvConsumed = ctx.LvConsumeds.Where(aa => aa.EmpLvType == empLvType).ToList();
                    if(_lvConsumed.Count()!=0)
                    RemainingLeaves = (decimal)_lvConsumed.FirstOrDefault().YearRemaining;
                   


                }
           

            return RemainingLeaves;

        }
        // POST: /ESS/ESSLeave/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "LvID,LvDate,LvType,EmpID,FromDate,ToDate,NoOfDays,IsHalf,FirstHalf,HalfAbsent,LvReason,LvAddress,CreatedBy,ApprovedBy,Status")] LvApplication lvapplication)
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            if (lvapplication.FromDate.Date > lvapplication.ToDate.Date)
                ModelState.AddModelError("FromDate", "From Date should be smaller than To Date");
            string _EmpNo = db.Emps.Where(aa => aa.EmpID == LoggedInUser.EmpID).FirstOrDefault().ToString();
            List<Emp> _emp = db.Emps.Where(aa => aa.EmpNo == _EmpNo).ToList();
            if (_emp.Count == 0)
            {
                ModelState.AddModelError("EmpNo", "Emp No not exist");
            }
            else
            {
                lvapplication.EmpID = _emp.FirstOrDefault().EmpID;
            }
            if (ModelState.IsValid)
            {
                LvType lvType = db.LvTypes.First(aa => aa.LvType1 == lvapplication.LvType);
                LeaveController LvProcessController = new LeaveController();
                if (LvProcessController.HasLeaveQuota(lvapplication.EmpID, lvapplication.LvType, lvType))
                {
                    if (lvapplication.IsHalf != true)
                    {
                        lvapplication.NoOfDays = (float)((lvapplication.ToDate - lvapplication.FromDate).TotalDays) + 1;
                        lvapplication.Active = false;
                        lvapplication.IsRevoked = false;
                        if (LvProcessController.CheckDuplicateLeave(lvapplication))
                        {
                            //Check leave Balance
                            if (LvProcessController.CheckLeaveBalance(lvapplication, lvType))
                            {
                                lvapplication.LvDate = DateTime.Today;
                                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                                lvapplication.CreatedBy = _userID;
                                lvapplication.Active = true;
                                db.LvApplications.Add(lvapplication);
                                if (db.SaveChanges() > 0)
                                {
                                    HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Leave, (byte)MyEnums.Operation.Add, DateTime.Now);
                                  //  LvProcessController.AddLeaveToLeaveData(lvapplication, lvType);
                                  //  LvProcessController.AddLeaveToLeaveAttData(lvapplication, lvType);
                                    ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s => s.EmpName), "EmpID", "EmpNo");
                                    ViewBag.LvType = new SelectList(db.LvTypes.Where(aa => aa.Enable == true).OrderBy(s => s.LvType1).ToList(), "LvType1", "LvDesc");
                                    return RedirectToAction("Create");
                                }
                                else
                                {
                                    ModelState.AddModelError("LvType", "There is an error while creating leave.");
                                }

                            }
                            else
                                ModelState.AddModelError("LvType", "Leave Balance Exceeds, Please check the balance");
                        }
                        else
                            ModelState.AddModelError("FromDate", "This Employee already has leave of this date ");
                    }
                    else
                    {
                        lvapplication.NoOfDays = (float)0.5;
                        if (lvapplication.FromDate.Date == lvapplication.ToDate.Date)
                        {
                            if (LvProcessController.CheckDuplicateLeave(lvapplication))
                            {
                                if (LvProcessController.CheckHalfLeaveBalance(lvapplication, lvType))
                                {
                                    lvapplication.LvDate = DateTime.Today;
                                    int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                                    lvapplication.CreatedBy = _userID;
                                    lvapplication.Active = true;
                                    db.LvApplications.Add(lvapplication);
                                    if (db.SaveChanges() > 0)
                                    {
                                        HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Leave, (byte)MyEnums.Operation.Add, DateTime.Now);
                                        LvProcessController.AddHalfLeaveToLeaveData(lvapplication, lvType);
                                        LvProcessController.AddHalfLeaveToAttData(lvapplication, lvType);
                                        ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s => s.EmpName), "EmpID", "EmpNo");
                                        ViewBag.LvType = new SelectList(db.LvTypes.Where(aa => aa.Enable == true).OrderBy(s => s.LvType1).ToList(), "LvType1", "LvDesc");
                                        return RedirectToAction("Create");
                                    }
                                }
                                else
                                    ModelState.AddModelError("LvType", "Leave Balance Exceeds, Please check the balance");
                            }
                            else
                                ModelState.AddModelError("FromDate", "This Employee already has leave of this date ");
                        }
                        else
                            ModelState.AddModelError("FromDate", "Half Leave should be entered of same date");
                    }
                }
                else
                    ModelState.AddModelError("LvType", "Leave Quota does not exist");
            }
            else
                ModelState.AddModelError("LvType", "Leave is not created. Please contact with network administrator");
            ViewBag.EmpID = new SelectList(db.Emps.OrderBy(s => s.EmpName), "EmpID", "EmpNo", lvapplication.EmpID);
            ViewBag.LvType = new SelectList(db.LvTypes.Where(aa => aa.Enable == true).OrderBy(s => s.LvType1), "LvType1", "LvDesc", lvapplication.LvType);
            return View(lvapplication);
        }

        // GET: /ESS/ESSLeave/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvApplication lvapplication = db.LvApplications.Find(id);
            if (lvapplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvapplication.EmpID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "UserName", lvapplication.CreatedBy);
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc", lvapplication.LvType);
            return View(lvapplication);
        }

        // POST: /ESS/ESSLeave/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="LvID,LvDate,LvType,EmpID,FromDate,ToDate,NoOfDays,IsHalf,FirstHalf,HalfAbsent,LvReason,LvAddress,CreatedBy,ApprovedBy,LvStatus,Active,IsRevoked")] LvApplication lvapplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lvapplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.Emps, "EmpID", "EmpNo", lvapplication.EmpID);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserID", "UserName", lvapplication.CreatedBy);
            ViewBag.LvType = new SelectList(db.LvTypes, "LvType1", "LvDesc", lvapplication.LvType);
            return View(lvapplication);
        }

        // GET: /ESS/ESSLeave/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LvApplication lvapplication = db.LvApplications.Find(id);
            if (lvapplication == null)
            {
                return HttpNotFound();
            }
            return View(lvapplication);
        }
        public ActionResult LeaveBalance(string id)
        {

           decimal sd= CheckLeaveBalance(id);

           return Json(new { id = 1, value = sd }, JsonRequestBehavior.AllowGet);
        
        }
        // POST: /ESS/ESSLeave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LvApplication lvapplication = db.LvApplications.Find(id);
            db.LvApplications.Remove(lvapplication);
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

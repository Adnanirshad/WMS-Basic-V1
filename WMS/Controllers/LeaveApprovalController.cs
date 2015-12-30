using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class LeaveApprovalController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LeaveApproval/ ----- returns the Leave View
        public ActionResult Index()
        {
            return View();
        }

        // GET: /LeaveApproval/PendingLeavesList -- Returns list of emps acc to the user info
        public JsonResult PendingLeavesList()
        {
            var collection = db.LvApplications.Where(lv => lv.Stage == 1 && lv.IsRevoked != true)
                .Select(x => new
            {
                LvID = x.LvID,
                EmpName = x.Emp.EmpName,
                EmpNo = x.Emp.EmpNo,
                Reason = x.LvReason,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                IsApproved = x.Active,
                IsRevoked = x.IsRevoked
            });
            return Json(collection, JsonRequestBehavior.AllowGet);
        }

        // POST: /LeaveApproval -- post with selected Leaves Data -- post back the selected Leave to Approve
        [HttpPost]
        public ActionResult Index(List<LvApplication> apps)
        {
            if (apps == null)
                return View();
            foreach (LvApplication app in apps)
            {
                ManageSingleLeaveInDB(app.LvID, true);
            }
            return View();
        }

        // GET: /LeaveApproval/ApproveAll  -- Get request to approve all leaves
        public ActionResult ApproveAll()
        {
            using (var context = new TAS2013Entities())
            {
                foreach (LvApplication app in context.LvApplications)
                {
                    ManageSingleLeaveInDB(app.LvID, true);
                }
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        // Get: /LeaveApproval/ApproveLeave -- Request to approve single leave with ID
        public JsonResult ApproveLeave(int LvID)
        {
            string result = ManageSingleLeaveInDB(LvID, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Get: /LeaveApproval/RevokeLeave -- Request to revoke single leave with ID
        public JsonResult RevokeLeave(int LvID)
        {
            string result = ManageSingleLeaveInDB(LvID, false);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string ManageSingleLeaveInDB(int LvID, Boolean status)
        {
            
            LvApplication leave = db.LvApplications.First(ep => ep.LvID == LvID);
            if (status)
            {
                leave.Active = status;
                //TODO : call leave controller functions to create leave data
                leave.Stage = 2;
            }
            else
            {
                leave.IsRevoked = true;
                leave.Stage = 2;
            }
            if (db.SaveChanges() > 0)
                return "success";
            else
                return "error";
        }            
	}
}
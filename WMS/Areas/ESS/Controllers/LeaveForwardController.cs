using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Areas.ESS.Controllers
{
    public class LeaveForwardController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /LeaveForward/ ----- returns the Leave View
        public ActionResult Index()
        {
            return View();
        }

        // GET: /LeaveForward/PendingLeavesList -- Returns list of emps acc to the user info
        public JsonResult PendingLeavesList()
        {
            var collection = db.LvApplications.Where(lv => lv.Stage < 1)
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

        // POST: /LeaveForward -- post with selected Leaves Data -- post back the selected Leave to Approve
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

        // GET: /LeaveForward/ApproveAll  -- Get request to approve all leaves
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

        // Get: /LeaveForward/ApproveLeave -- Request to approve single leave with ID
        public JsonResult ApproveLeave(int LvID)
        {
            string result = ManageSingleLeaveInDB(LvID, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Get: /LeaveForward/RevokeLeave -- Request to revoke single leave with ID
        public JsonResult RevokeLeave(int LvID)
        {
            string result = ManageSingleLeaveInDB(LvID, false);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string ManageSingleLeaveInDB(int LvID, Boolean status)
        {

            LvApplication leave = db.LvApplications.First(ep => ep.LvID == LvID);
            if (status)
                leave.Stage = 1;
            else
            {
                leave.IsRevoked = true;
                leave.Stage = 1;
            }
            if (db.SaveChanges() > 0)
                return "success";
            else
                return "error";
        }            
	}
}